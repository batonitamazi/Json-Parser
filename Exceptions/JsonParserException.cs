namespace JsonParser.Exceptions;

public class JsonParserException : Exception
{
    public JsonParserException(string message) : base(message)
    {
    }

    public JsonParserException(string message, Exception innerException) : base(message, innerException)
    {
    }


}