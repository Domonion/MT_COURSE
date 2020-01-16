using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Antlr4.Runtime;

namespace ParserGenerator
{
    public static class ParserGenerator
    {
        private static readonly Token myEps = new Token("EPS");

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

        private static void CountFollow(Dictionary<Rule, HashSet<IAtom>> first, out Dictionary<Rule, HashSet<IAtom>> follow, HashSet<Rule> rules)
        {
            follow = null;
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
                //myTODO EPS and SKIP
                CountFirst(out Dictionary<Rule, HashSet<IAtom>> first, rules);
                CountFollow(first, out var follow, rules);
//                Log(first, writer);
//                Log(follow, writer);
                
                //6. генерировать
                //да буду я строить дерево разбора, и на этом дерево разбора будут навешаны атрибуты хуле.
            }
        }
    }
}