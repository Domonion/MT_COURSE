using System;
using System.Collections.Generic;
using Generated;

namespace Main
{
    internal class Program
    {
        private class Command
        {
            private readonly List<string> myArgs;
            private readonly Action<List<string>> myCommand;

            public Command(List<string> args, Action<List<string>> command)
            {
                myArgs = args;
                myCommand = command;
            }

            public void Execute()
            {
                myCommand(myArgs);
            }
        }

        private static void PrintHelp()
        {
            Console.WriteLine("help lexer [exit q q!] clear last");
        }

        public static void Main()
        {
            var done = true;
            Command last = null;
            while (done)
            {
                Console.Write(":");
                var currentCommand = Console.ReadLine()?.Trim();
                switch (currentCommand)
                {
                    case "exit":
                    case "q":
                    case "q!":
                    {
                        done = false;
                        break;
                    }
                    case "help":
                    {
                        last = new Command(null, list => PrintHelp());
                        last.Execute();
                        break;
                    }
                    case "last":
                    {
                        last?.Execute();
                        break;
                    }
                    case "clear":
                    {
                        last = new Command(null, list => Console.Clear());
                        last.Execute();
                        break;
                    }
                    case "calc":
                    {
                        Console.Write("Type expression: ");
                        var expr = Console.ReadLine();
                        last = new Command(new List<string>{expr}, list =>
                        {
                            var lexer = new GeneratedLexer(list[0]);
                            var parser = new GeneratedParser(lexer);
                            var res = parser.s();
                            Console.WriteLine(res.res);
                        });
                        last.Execute();
                        break;
                    }
                    default:
                    {
                        last = new Command(null, list => Console.WriteLine("Command not recognized, type help to list available commands"));
                        last.Execute();
                        break;
                    }
                }
            }
        }
    }
}