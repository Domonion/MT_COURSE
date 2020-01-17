using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Antlr4.Runtime;
using JetBrains.Annotations;

namespace ParserGenerator
{
    public static class ParserGenerator
    {
        private static readonly Token myEps = new Token("EPS");
        private static readonly Token myEof = new Token("EOF");
        private static readonly Token mySkip = new Token("SKIP");

        private static List<IAtom> GetSubFirst(List<IAtom> body, Dictionary<Rule, HashSet<IAtom>> first)
        {
            if (body.Count == 0)
            {
                throw new Exception("body has no rules, should be impossible");
            }

            if (body[0].Equals(myEps))
            {
                return new List<IAtom> {body[0]};
            }

            if (!body[0].IsRule)
            {
                return new List<IAtom> {body[0]};
            }

            var rule = (Rule) body[0];
            if (first[rule].Contains(myEps))
            {
                var firstWithoutEps = first[rule].ToHashSet();
                firstWithoutEps.Remove(myEps);
                var anotherBody = body.ToList();
                anotherBody.RemoveAt(0);
                if (anotherBody.Count == 0)
                {
                    anotherBody.Add(myEps);
                }

                var subFirstForAnotherBody = GetSubFirst(anotherBody, first);
                firstWithoutEps.UnionWith(subFirstForAnotherBody);
                return firstWithoutEps.ToList();
            }
            else
            {
                return first[rule].ToList();
            }
        }

        private static void CountFirst(out Dictionary<Rule, HashSet<IAtom>> first, HashSet<Rule> rules)
        {
            first = rules.ToDictionary(rule => rule, rule => new HashSet<IAtom>());
            var changed = true;
            while (changed)
            {
                changed = false;
                foreach (var rule in rules)
                {
                    foreach (var body in rule.Bodies)
                    {
                        var subFirst = GetSubFirst(body, first);
                        var beforeCount = first[rule].Count;
                        first[rule].UnionWith(subFirst);
                        var afterCount = first[rule].Count;
                        changed |= beforeCount != afterCount;
                    }
                }
            }
        }

        private static void CountFollow(Dictionary<Rule, HashSet<IAtom>> first, out Dictionary<Rule, HashSet<IAtom>> follow, HashSet<Rule> rules,
            Rule start)
        {
            follow = rules.ToDictionary(rule => rule, rule => new HashSet<IAtom>());
            follow[start].Add(myEof);
            var changed = true;
            while (changed)
            {
                changed = false;
                foreach (var rule in rules)
                {
                    foreach (var body in rule.Bodies)
                    {
                        var bodyList = body.ToList();
                        foreach (var atom in body)
                        {
                            bodyList.RemoveAt(0);
                            if (bodyList.Count == 0)
                            {
                                bodyList.Add(myEps);
                            }

                            if (atom.IsRule)
                            {
                                var atomRule = (Rule) atom;
                                var tailFirst = GetSubFirst(bodyList, first);
                                var followCountBefore = follow[atomRule].Count;
                                if (tailFirst.Contains(myEps))
                                {
                                    follow[atomRule].UnionWith(follow[rule]);
                                }

                                tailFirst.Remove(myEps);
                                follow[atomRule].UnionWith(tailFirst);
                                var followCountAfter = follow[atomRule].Count;
                                changed |= followCountAfter != followCountBefore;
                            }
                        }
                    }
                }
            }
        }

        private static void Log(Dictionary<Rule, HashSet<IAtom>> toDump, string outputFile, string identifier)
        {
            outputFile += $".{identifier}.log";
            using (var writer = File.CreateText(outputFile))
            {
                writer.WriteLine("currently dumping: " + identifier);
                writer.WriteLine($"Rules: {toDump.Count}");
                const string ident = "    ";
                foreach (var kvp in toDump)
                {
                    var rule = kvp.Key;
                    var set = kvp.Value;
                    writer.WriteLine($"Rule {rule.Name} has {set.Count} elements:");
                    var ind = 0;
                    foreach (var atom in set)
                    {
                        ind++;
                        writer.WriteLine(ident + $"{ind}. {atom.Name}");
                    }
                }
            }
        }

        public static void GenerateParser(string inputFile, string outputFile)
        {
            var inputStream = new AntlrFileStream(inputFile);
            var spreadsheetLexer = new ParserLexer(inputStream);
            var commonTokenStream = new CommonTokenStream(spreadsheetLexer);
            var lexerParser = new ParserParser(commonTokenStream);
            var grammatixContext = lexerParser.grammatix();
            var visitor = new ParserVisitor();
            visitor.Visit(grammatixContext);
            var rules = visitor.Rules;
            foreach (var body in rules.SelectMany(rule => rule.Bodies))
            {
                for (var i = 0; i < body.Count; i++)
                {
                    body[i] = body[i].Convert(rules);
                }
            }

            using (var writer = File.CreateText(outputFile))
            {
                CountFirst(out Dictionary<Rule, HashSet<IAtom>> first, rules);
                CountFollow(first, out var follow, rules, visitor.start);
                Log(first, outputFile, nameof(first));
                Log(follow, outputFile, nameof(follow));
                writer.Write("public class Parser{");
                writer.WriteLine("private readonly Lexer myLexer;");
                writer.WriteLine("public Parser(Lexer lexer){");
                writer.WriteLine("myLexer = lexer");
                writer.WriteLine("}");
                //1. generate all answer for every rule node 
                //2. in this answer node place attributes
                //inherited attribute can be implemened via parameters + indicating order of rules evaluating in actions
                foreach (var (token, _) in tokens.Tokens)
                {
                    
                    
                }
                writer.Write("}");
            }
        }
    }
}