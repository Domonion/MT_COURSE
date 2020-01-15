using System;
using System.Collections.Generic;

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
                var currentCommand = Console.ReadLine().Trim();
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
                    case "lexer":
                    {
                        Console.Write("path to grammar file: ");
                        var pathToGrammar = Console.ReadLine();
                        Console.Write("path to lexer: ");
                        var pathToLexer = Console.ReadLine();
                        last = new Command(new List<string> {pathToGrammar, pathToLexer}, list =>
                        {
                            try
                            {
                                LexerGenerator.LexerGenerator.GenerateLexer(list[0], list[1]);
                            }
                            catch (Exception e)
                            {
                                Console.WriteLine(e);
                            }
                        });
                        last.Execute();
                        break;
                    }
                    case "":
                    {
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
/*
grammar Lexer;

grammatix : rule_list token_list;

rule_list : rule1 | rule1 rule_list;
rule1 : RULE_NAME DDOT atom_list DC;

//attributes : SB attributes_list SCB;
//attributes_list : attribute attributes_end?;
//attributes_end : COMMA attribute attributes_end?;
//attribute : TYPE ANY_NAME;

atom_list : atom | atom atom_list;
atom : RULE_NAME | TOKEN_NAME;

token_list : token+;
token : TOKEN_NAME DDOT REGEX DC;

REGEX : '${' .*? '}$';
fragment LOW_CHAR : 'a'..'z';
fragment UP_CHAR : 'A'..'Z';
grammar_name : GRAMMAR NAME DC;
GRAMMAR : 'grammar';
NAME : CHAR+;
DDOT : ':';
DC : ';';
//RULE_NAME : [a-z][a-z]*;
//TOKEN_NAME : [A-Z][A-Z]*;
WS : [ \n\t\r]+ -> skip;
RULE_NAME : LOW_CHAR+;
TOKEN_NAME : UP_CHAR+;
*/