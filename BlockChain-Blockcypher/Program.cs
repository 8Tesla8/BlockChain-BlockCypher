using System;
using System.IO;

namespace BlockChainBlockcypher
{
    class MainClass
    {
        public static void Main(string[] args)
        {
            Console.WriteLine("Hello World!");


            var commandExecutor = new CommandExecutor();

            //type path to json file that store account info
            var pathToJsonFile = GetPathToJsonFile();
            commandExecutor.Execute("i " + pathToJsonFile);
            //

            commandExecutor.ShowInsctruction();
            Console.WriteLine("Type empty string for exit.\n");

            while (true)
            {
                Console.WriteLine("Type command: ");
                var userParams = Console.ReadLine();

                if (string.IsNullOrWhiteSpace(userParams))
                    break;

                commandExecutor.Execute(userParams);
            }

        }

        public static string GetPathToJsonFile()
        {
            var currentDirectory = Path.GetDirectoryName(AppDomain.CurrentDomain.BaseDirectory);

            //up on 2 folders
            var mainFolder = Path.GetFullPath(Path.Combine(currentDirectory, $@"..{Path.DirectorySeparatorChar}..{Path.DirectorySeparatorChar}"));
            var pathToJsonFile = $"{mainFolder}ignore-json{Path.DirectorySeparatorChar}data.json";

            return pathToJsonFile;
        }
    }
}
