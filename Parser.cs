using System.Diagnostics;
using JsonParser.Models;

namespace JsonParser;

public class Parser
{
    private readonly List<Token> _tokens;
    private int _position = 0;

    public Parser(List<Token> tokens)
    {
        _tokens = tokens;
    }

    private Token CurrentToken => _tokens[_position];

    private bool Match(TokenType type)
    {
        if (type != CurrentToken.Type)
        {
            return false;
        }

        _position++;
        return true;
    }

    private void Expect(TokenType type)
    {
        if (!Match(type))
            throw new Exception($"Expected {type} but found {CurrentToken.Type}");
    }

    public void ParseJson()
    {
        ParseValue();
        Expect(TokenType.Eof);
    }

    private void ParseObject()
    {
        Expect(TokenType.LeftBrace);

        ParsePair();
        
        while (Match(TokenType.Comma))
        {
            ParsePair();
        }
        Expect(TokenType.RightBrace);
    }

    private void ParsePair()
    {
        Expect(TokenType.String);
        Expect(TokenType.Colon);
        ParseValue();
    }

    private void ParseArray()
    {
        Expect(TokenType.LeftBracket);
        if (CurrentToken.Type == TokenType.RightBracket)
        {
            _position++;
            return;
        }
        ParseValue();
        while (Match(TokenType.Comma))
        {
            ParseValue();
        }
        Expect(TokenType.RightBracket);
    }
    
    

    private void ParseValue()
    {
        switch (CurrentToken.Type)
        {
            case TokenType.Number:
            case TokenType.True: 
            case TokenType.False: 
            case TokenType.Null:
            case TokenType.String:
                _position++;
                break;
            case TokenType.LeftBracket:
                ParseArray();
                break;
            case TokenType.LeftBrace:
                ParseObject();
                break;
            default:
                throw new Exception($"Unexpected token: {CurrentToken.Type}");
        }
        
    }
}