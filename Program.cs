using JsonParser;
using JsonParser.Models;


internal class Program
{
    private static int Main(string[] args)
    {
        if (args.Length == 0)
        {
            Console.WriteLine("Usage: JsonParser <file>");
            Console.WriteLine("       JsonParser --help");
            return ExitCodes.InvalidUsage;
        }
        
        if (args[0] == "--help" || args[0] == "-h")
        {
            DisplayHelp();
            return ExitCodes.Success;
        }
        
        string filePath = args[0];
        return JsonFileReader.CheckJsonValidity(filePath);
    }
        
    private static void DisplayHelp()
    {
        Console.WriteLine("JSON Parser - Validates JSON files");
        Console.WriteLine();
        Console.WriteLine("Usage:");
        Console.WriteLine("  JsonParser <file>     Validate a JSON file");
        Console.WriteLine("  JsonParser --help     Show this help message");
        Console.WriteLine();
        Console.WriteLine("Exit codes:");
        Console.WriteLine("  0 - Valid JSON");
        Console.WriteLine("  1 - Invalid JSON or file error");
        Console.WriteLine("  2 - Invalid usage");
    }
}
// if (args.Length == 0)
// {
//     Console.WriteLine("Usage: <jsonparser> <file>");
// }
//
// string filePath = args[0];
//
// JsonFileReader.CheckJsonValidity(filePath);