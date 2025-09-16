using System.Text;
using JsonParser;
using JsonParser.Exceptions;
using JsonParser.Models;

public static class JsonFileReader
{
    
    public static int CheckJsonValidity(string path)
    {
        // Validate input
        if (string.IsNullOrWhiteSpace(path))
        {
            Console.Error.WriteLine("Error: File path cannot be empty");
            return ExitCodes.Error;
        }

        if (!File.Exists(path))
        {
            Console.Error.WriteLine($"Error: File '{path}' not found");
            return ExitCodes.Error;
        }
        
        
        string content;
        try
        {
            content = File.ReadAllText(path, Encoding.UTF8).Trim();
            
            if (string.IsNullOrEmpty(content))
            {
                Console.WriteLine("Invalid JSON: Empty file");
                return ExitCodes.Error;
            }
        }
        catch (UnauthorizedAccessException)
        {
            Console.Error.WriteLine($"Error: Access denied to file '{path}'");
            return ExitCodes.Error;
        }
        catch (IOException ex)
        {
            Console.Error.WriteLine($"Error reading file: {ex.Message}");
            return ExitCodes.Error;
        }

        try
        {
            var Lexer = new Lexer();
            var tokens = Lexer.Tokenize(content);
            var parser = new Parser(tokens);
            parser.ParseJson();
            
            Console.WriteLine("Valid JSON");
            return ExitCodes.Success;
        }
        catch (JsonParserException ex)
        {
            Console.WriteLine($"Invalid JSON: {ex.Message}");
            return ExitCodes.Error;
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"Unexpected error: {ex.Message}");
            return ExitCodes.Error;
        }
    }
}