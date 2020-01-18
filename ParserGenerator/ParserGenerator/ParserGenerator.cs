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
        private static readonly Token myEof = new Token("EOF");
        private static readonly Token mySkip = new Token("SKIP");

        private static List<Token> GetSubFirst(List<IAtom> body, Dictionary<Rule, HashSet<Token>> first)
        {
            if (body.Count == 0)
            {
                throw new Exception("body has no rules, should be impossible");
            }

            if (body[0].Equals(myEps))
            {
                return new List<Token> {myEps};
            }

            if (!body[0].IsRule)
            {
                return new List<Token> {(Token) body[0]};
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

            return first[rule].ToList();
        }

        private static void CountFirst(out Dictionary<Rule, HashSet<Token>> first, HashSet<Rule> rules)
        {
            first = rules.ToDictionary(rule => rule, rule => new HashSet<Token>());
            var changed = true;
            while (changed)
            {
                changed = false;
                foreach (var rule in rules)
                {
                    foreach (var body in rule.Bodies)
                    {
                        var subFirst = GetSubFirst(body.Atoms, first);
                        var beforeCount = first[rule].Count;
                        first[rule].UnionWith(subFirst);
                        var afterCount = first[rule].Count;
                        changed |= beforeCount != afterCount;
                    }
                }
            }
        }

        private static void CountFollow(Dictionary<Rule, HashSet<Token>> first, out Dictionary<Rule, HashSet<Token>> follow, HashSet<Rule> rules,
            Rule start)
        {
            follow = rules.ToDictionary(rule => rule, rule => new HashSet<Token>());
            follow[start].Add(myEof);
            var changed = true;
            while (changed)
            {
                changed = false;
                foreach (var rule in rules)
                {
                    foreach (var body in rule.Bodies)
                    {
                        var bodyList = body.Atoms.ToList();
                        foreach (var atom in body.Atoms)
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

        private static void Log(Dictionary<Rule, HashSet<Token>> toDump, string outputFile, string identifier)
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

        private static string ReplaceAction(string action, int ind)
        {
            if (action != null)
            {
                action = action.Replace("$me", "myReturn").Replace("$text", "text");
                for (var i = 0; i < ind; i++)
                {
                    action = action.Replace("$" + i, "ret" + i);
                }
            }

            return action;
        }

        private static string CreateIfFollow(Dictionary<Rule, HashSet<Token>> follow, Rule rule)
        {
            var res = "";
            foreach (var token in follow[rule])
            {
                res += "Token." + token.Name + " == CurrentToken || ";
            }
            res += "false";
            return res;
        }

        private static string ReplaceAttributes(string attributes)
        {
            if (attributes != null)
            {
                attributes = attributes.Remove(0, 1);
                attributes = attributes.Remove(attributes.Length - 1, 1);
                attributes = attributes.Replace(',', ';') + ';';
                attributes = attributes.Replace(":", " ");
                attributes = "public " + attributes;
                attributes = attributes.Replace(";", ";public");
                attributes = attributes.Substring(0, attributes.Length - 6);
            }

            return attributes;
        }

        private static string CreateIfFirst(Dictionary<Rule, HashSet<Token>> first, Body body, ref bool hasEps)
        {
            var res = "";
            var firstList = GetSubFirst(body.Atoms, first);
            hasEps |= firstList.Contains(myEps);
            firstList.Remove(myEps);
            foreach (var token in firstList)
            {
                res += "Token." + token.Name + " == CurrentToken || ";
            }

            res += "false";
            return res;
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
                for (var i = 0; i < body.Atoms.Count; i++)
                {
                    body.Atoms[i] = body.Atoms[i].Convert(rules);
                }
            }

            using (var writer = File.CreateText(outputFile))
            {
                CountFirst(out var first, rules);
                CountFollow(first, out var follow, rules, visitor.Start);
                Log(first, outputFile, nameof(first));
                Log(follow, outputFile, nameof(follow));
                writer.WriteLine("using System;");
                writer.WriteLine("using System.Collections.Generic;");
                writer.WriteLine("namespace Generated{");
                writer.WriteLine("public class GeneratedParser{");
                writer.WriteLine("private readonly GeneratedLexer myLexer;");
                writer.WriteLine("public GeneratedParser(GeneratedLexer lexer){");
                writer.WriteLine("myLexer = lexer;");
                writer.WriteLine("Next();");
                writer.WriteLine("}");
                foreach (var rule in rules)
                {
                    writer.WriteLine("public class " + rule.Name + "return{");
                    writer.WriteLine(ReplaceAttributes(rule.Attributes));
                    writer.WriteLine("}");
                }

                writer.WriteLine("private Token CurrentToken;");
                writer.WriteLine("private string CurrentTokenString;");
                writer.WriteLine("private bool eofOnce = false;");
                writer.WriteLine("private void Next(){");
                writer.WriteLine("var next = myLexer.NextToken();");
                writer.WriteLine("while(next == Token.SKIP){");
                writer.WriteLine("next = myLexer.NextToken();");
                writer.WriteLine("}");
                writer.WriteLine("if(next == Token.EOF){");
                writer.WriteLine("if(eofOnce){");
                writer.WriteLine("throw new Exception(\"Lexer has ended.\");");
                writer.WriteLine("}");
                writer.WriteLine("eofOnce = true;");
                writer.WriteLine("}");
                writer.WriteLine("CurrentToken = next;");
                writer.WriteLine("CurrentTokenString = myLexer.CurrentString;");
                writer.WriteLine("}");
                foreach (var rule in rules)
                {
                    writer.WriteLine("public " + rule.Name + "return " + rule.Name + "(){");
                    var hasEps = false;
                    Body epsBody = null;
                    foreach (var body in rule.Bodies)
                    {
                        if (!(body.Atoms.Count == 1 && body.Atoms[0].Equals(myEps)))
                        {
                            var was = hasEps;
                            writer.WriteLine("if(" + CreateIfFirst(first, body, ref hasEps) + "){");
                            if (was != hasEps)
                            {
                                epsBody = body;
                            }

                            writer.WriteLine("var text = new List<string>();");
                            writer.WriteLine("var myReturn = new " + rule.Name + "return();");
                            var ind = 0;
                            foreach (var atom in body.Atoms)
                            {
                                if (atom.IsRule)
                                {
                                    writer.WriteLine("text.Add(\"\");");
                                    writer.WriteLine("var ret" + ind + " = " + atom.Name + "();");
                                }
                                else
                                {
                                    writer.WriteLine("text.Add(CurrentTokenString);");
                                    writer.WriteLine("Next();");
                                }

                                ind++;
                            }

                            writer.WriteLine(ReplaceAction(body.Action, ind));
                            writer.WriteLine("return myReturn;");
                            writer.WriteLine("} else ");
                        }
                        else
                        {
                            hasEps = true;
                            epsBody = body;
                        }
                    }

                    if (hasEps)
                    {
                        //1. i have direct epsilon - then i have body and have to use its action
                        //2. i have indirect epsilon - it means i should use body of this
                        writer.WriteLine("if(" + CreateIfFollow(follow, rule) + "){");
                        writer.WriteLine("var text = new List<string>();");
                        writer.WriteLine("var myReturn = new " + rule.Name + "return();");
                        var ind = 0;
                        if (!(epsBody.Atoms.Count == 1 && epsBody.Atoms[0].Equals(myEps)))
                        {
                            foreach (var atom in epsBody.Atoms)
                            {
                                if (atom.IsRule)
                                {
                                    writer.WriteLine("text.Add(\"\");");
                                    writer.WriteLine("Next();");
                                    writer.WriteLine("var ret" + ind + " = " + atom.Name + "();");
                                }
                                else
                                {
                                    writer.WriteLine("text.Add(CurrentTokenString);");
                                    writer.WriteLine("Next();");
                                }
                            }
                            ind++;
                        }

                        writer.WriteLine(ReplaceAction(epsBody.Action, ind));
                        writer.WriteLine("return myReturn;");
                        writer.WriteLine("} else ");
                    }

                    writer.WriteLine("{");
                    writer.WriteLine("throw new Exception(\"incorrect token\");");
                    writer.WriteLine("}");
                    writer.WriteLine("}");
                }

                //inherited attribute can be implemened via parameters + indicating order of rules evaluating in actions
                writer.WriteLine("}");
                writer.WriteLine("}");
            }
        }
    }
}