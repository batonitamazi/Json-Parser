using JsonParser;
using JsonParser.Models;

public static class JsonFileReader
{
    
    public static int CheckJsonValidity(string path)
    {
        string content;
        try
        {
            content = File.ReadAllText(path).Trim();
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"Error reading file: {ex.Message}");
            return 1;
        }

        try
        {
            var Lexer = new Lexer();
            var tokens = Lexer.LexicalAnalyser(content);
            var parser = new Parser(tokens);
            parser.ParseJson();
            Console.WriteLine("Valid JSON");
            return 0;
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Invalid JSON: {ex.Message}");
            return 1;
        }
    }
}