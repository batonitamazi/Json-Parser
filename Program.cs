using JsonParser;

if (args.Length == 0)
{
    Console.WriteLine("Usage: <jsonparser> <file>");
}

string filePath = args[0];

JsonFileReader.CheckJsonValidity(filePath);