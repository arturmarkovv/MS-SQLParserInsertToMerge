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
            string path = @"C:\Users\Данил\source\repos\MS-SQL Parser - Insert To Merge\MS-SQL Parser - Insert To Merge\bin\Debug\net5.0\input.txt";
            //if (args.Length == 0)
            //{
            //    throw new Exception("Missed file");
            //}
            //path = args[0];
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
            Console.WriteLine(res);
            Console.ReadKey();
        }
    }
}
