using System;
using System.IO;
using System.Text;
using System.Threading.Tasks;

namespace MS_SQL_Parser___Insert_To_Merge
{
    class Program
    {
        static async Task Main(string[] args)
        {
            var path = GetPath(args);

            var builder = new MergeScriptBuilder();
            using (StreamReader reader = new StreamReader(path))
            {
                string? line;

                while ((line = await reader.ReadLineAsync()) != null)
                {
                    builder.AddData(Parser.GetNodes(line));
                }
            }
            var res = builder.GetResult();
            var outPath = "MergeScript.sql";
            using (StreamWriter writer = new StreamWriter(outPath, false))
            {
                await writer.WriteLineAsync(res);
            }
        }

        private static string GetPath(string[] args)
        {
            if (args.Length == 0)
            {
                string dirPath = Directory.GetCurrentDirectory();
                Console.WriteLine($"Enter the name of the source file (example: \"input.sql\")");
                var fileName = Console.ReadLine();
                var filePath = $"{dirPath}\\{fileName}";
                if (!File.Exists(filePath))
                {
                    Console.WriteLine("File doesn't exist in current directory!");
                    throw new Exception("File doesn't exist in current directory!");
                }
                return filePath;
            }
            else
            {
                return args[0];
            }
        }
    }
}
