namespace JsonParser.Models;

public record Token(TokenType Type, string Value, int Position = 0);