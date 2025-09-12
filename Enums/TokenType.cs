namespace JsonParser;

public enum TokenType
{
    LeftBrace, RightBrace,
    LeftBracket, RightBracket,
    Colon, Comma,
    String, Number, True, False, Null,  
    Eof
}