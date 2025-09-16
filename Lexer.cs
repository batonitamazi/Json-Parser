using System.Text;
using JsonParser.Exceptions;
using JsonParser.Models;

namespace JsonParser;

public class Lexer
{
    public static List<Token> Tokenize(string input)
    {
        List<Token> tokens = new List<Token>();
        int i = 0;

        while (i < input.Length)
        {
            char c = input[i];

            switch (c)
            {
                case '{':
                    tokens.Add(new Token(TokenType.LeftBrace, "{", i));
                    i++;
                    break;
                case '}':
                    tokens.Add(new Token(TokenType.RightBrace, "}", i));
                    i++;
                    break;
                case '[':
                    tokens.Add(new Token(TokenType.LeftBracket, "[", i));
                    i++;
                    break;
                case ']':
                    tokens.Add(new Token(TokenType.RightBracket, "]", i));
                    i++;
                    break;
                case ':':
                    tokens.Add(new Token(TokenType.Colon, ":", i));
                    i++;
                    break;
                case ',':
                    tokens.Add(new Token(TokenType.Comma, ",", i));
                    i++;
                    break;
                case '"':
                    var (token, newPos) = ParseString(input, i);
                    tokens.Add(token);
                    i = newPos;
                    break;
                case ' ':
                case '\n':
                case '\t':
                case '\r':
                    i++;
                    break;
                default:
                    if (char.IsDigit(c) || c == '-')
                    {
                        var (numberToken, numberPos) = ParseNumber(input, i);
                        tokens.Add(numberToken);
                        i = numberPos;
                    }

                    else if (char.IsLetter(c))
                    {
                        var (keywordToken, keywordPos) = ParseKeyWord(input, i);
                        tokens.Add(keywordToken);
                        i = keywordPos;
                    }
                    else
                    {
                        throw new JsonParserException($"Unexpected character '{c}' at position {i}");
                    }
                    break;
            }
        }

        tokens.Add(new Token(TokenType.Eof, ""));
        return tokens;
    }

    private static (Token token, int position) ParseString(string input, int startIndex)
    {
        var value = new StringBuilder();
        int position = startIndex + 1;
        while (position < input.Length)
        {
            char current = input[position];
            if (current == '"')
            {
                return (new Token(TokenType.String, value.ToString()), position + 1);
            }
            
            if (current == '\\')
            {
                if (position + 1 >= input.Length)
                    throw new JsonParserException($"Unterminated escape sequence at position {position}");

                char nextChar = input[position + 1];
                switch (nextChar)
                {
                    case '"':
                        value.Append('"');
                        break;
                    case '\\':
                        value.Append('\\');
                        break;
                    case '/':
                        value.Append('/');
                        break;
                    case 'b':
                        value.Append('\b');
                        break;
                    case 'f':
                        value.Append('\f');
                        break;
                    case 'n':
                        value.Append('\n');
                        break;
                    case 'r':
                        value.Append('\r');
                        break;
                    case 't':
                        value.Append('\t');
                        break;
                    case 'u':
                        // Unicode escape sequence \uXXXX
                        if (position + 5 >= input.Length)
                            throw new JsonParserException($"Invalid unicode escape sequence at position {position}");

                        string hexDigits = input.Substring(position + 2, 4);
                        if (!IsValidHex(hexDigits))
                            throw new JsonParserException($"Invalid unicode escape sequence at position {position}");

                        int unicodeValue = Convert.ToInt32(hexDigits, 16);
                        value.Append((char)unicodeValue);
                        position += 4; // Skip the 4 hex digits
                        break;
                    default:
                        throw new JsonParserException($"Invalid escape sequence '\\{nextChar}' at position {position}");
                }

                position += 2;
            }


            value.Append(current);
            position++;
        }

        throw new JsonParserException($"Unterminated string starting at position {startIndex}");
    }

    private static  (Token token, int position) ParseNumber(string input, int startIndex)
    {
        int position = startIndex;
        var value = new StringBuilder();

        if (input[position] == '-')
        {
            value.Append("-");
            position++;
            if (position >= input.Length || !char.IsDigit(input[position]))
            {
                throw new JsonParserException($"Invalid number format at position {startIndex}");
            }
        }
        if (input[position] == '0')
        {
            value.Append('0');
            position++;
        }
        else if (char.IsDigit(input[position]))
        {
            while (position < input.Length && char.IsDigit(input[position]))
            {
                value.Append(input[position]);
                position++;
            }
        }
        
        // Parse decimal part
        if (position < input.Length && input[position] == '.')
        {
            value.Append('.');
            position++;
            
            if (position >= input.Length || !char.IsDigit(input[position]))
                throw new JsonParserException($"Invalid number format at position {startIndex}");
            
            while (position < input.Length && char.IsDigit(input[position]))
            {
                value.Append(input[position]);
                position++;
            }
        }
        return (new Token(TokenType.Number, value.ToString(), startIndex), position);

    }

    private static (Token token, int position) ParseKeyWord(string input, int startIndex)
    {
        int position = startIndex;
        var value = new StringBuilder();
        
        while (position < input.Length && char.IsLetter(input[position]))
        {
            value.Append(input[position]);
            position++;
        }
        string keyword = value.ToString().ToLower();
        TokenType tokenType = keyword switch
        {
            "true" => TokenType.True,
            "false" => TokenType.False,
            "null" => TokenType.Null,
            _ => throw new JsonParserException($"Unknown keyword '{keyword}' at position {startIndex}")
        };

        return (new Token(tokenType, keyword, startIndex), position);
    }
    
    private static bool IsValidHex(string hex)
    {
        return hex.All(c => char.IsDigit(c) || 
                            (c >= 'a' && c <= 'f') || 
                            (c >= 'A' && c <= 'F'));
    }
}