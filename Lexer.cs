using JsonParser.Models;

namespace JsonParser;

public class Lexer
{
    public static List<Token> LexicalAnalyser(string input)
    {
        List<Token> tokens = new List<Token>();
        int i = 0;

        while (i < input.Length)
        {
            char c = input[i];

            switch (c)
            {
                case '{':
                    tokens.Add(new Token(TokenType.LeftBrace, "{"));
                    i++;
                    break;
                case '}':
                    tokens.Add(new Token(TokenType.RightBrace, "}"));
                    i++;
                    break;
                case '[':
                    tokens.Add(new Token(TokenType.LeftBracket, "["));
                    i++;
                    break;
                case ']':
                    tokens.Add(new Token(TokenType.RightBracket, "]"));
                    i++;
                    break;
                case ':':
                    tokens.Add(new Token(TokenType.Colon, ":"));
                    i++;
                    break;
                case ',':
                    tokens.Add(new Token(TokenType.Comma, ","));
                    i++;
                    break;
                case '"':
                    int startIndex = ++i;
                    while (i < input.Length && input[i] != '"') i++;
                    if (i >= input.Length) throw new Exception("Unterminated string");
                    string strValue = input[startIndex..i];
                    tokens.Add(new Token(TokenType.String, strValue));
                    i++;
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
                        int numStart = i;
                        i++;
                        while (i < input.Length && (char.IsDigit(input[i]) || input[i] == '.'))
                            i++;
                        string numValue = input[numStart..i];
                        tokens.Add(new Token(TokenType.Number, numValue));
                        break;
                    }
                    else if (char.IsLetter(c))
                    {
                        int start = i;
                        while (i < input.Length && char.IsLetter(input[i])) i++;
                        string word = input[start..i].ToLower();
                        switch (word)
                        {
                            case "true":
                                tokens.Add(new Token(TokenType.True, word));
                                break;
                            case "false":
                                tokens.Add(new Token(TokenType.False, word));
                                break;
                            case "null":
                                tokens.Add(new Token(TokenType.Null, word));
                                break;
                            default:
                                throw new Exception($"Unexpected identifier: {word}");
                        }
                    }
                    else
                    {
                        throw new Exception($"Unexpected character: {c}");
                    }
                    break;
            }
        }

        tokens.Add(new Token(TokenType.Eof, ""));
        return tokens;
    }
}