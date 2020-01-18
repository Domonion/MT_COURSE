using System;
using System.IO;
using System.Text.RegularExpressions;
using Antlr4.Runtime;

namespace LexerGenerator
{
    public static class LexerGenerator
    {
        public static void GenerateLexer(string inputFile, string outputFile)
        {
            var inputStream = new AntlrFileStream(inputFile);
            var spreadsheetLexer = new LexerLexer(inputStream);
            var commonTokenStream = new CommonTokenStream(spreadsheetLexer);
            var lexerParser = new LexerParser(commonTokenStream);
            var grammatixContext = lexerParser.grammatix();
            var visitor = new LexerVisitor();
            visitor.Visit(grammatixContext);
            var tokens = visitor.TokenContainer.Tokens;

            using (var writer = File.CreateText(outputFile))
            {
                writer.WriteLine("using System.IO;");
                writer.WriteLine("using System.Text.RegularExpressions;");
                writer.WriteLine("namespace Generated{");
                writer.WriteLine("public enum Token{");
                var hasSkip = false;
                foreach (var (name, _) in tokens)
                {
                    hasSkip |= name == "SKIP";
                    writer.WriteLine(name);
                    writer.WriteLine(",");
                }
                writer.WriteLine("EOF");
                if (!hasSkip)
                {
                    writer.WriteLine(",");
                    writer.WriteLine("SKIP");
                }
                writer.WriteLine("}");
                writer.WriteLine("public class GeneratedLexer{");
                writer.WriteLine("public string CurrentString { get; private set; }");
                foreach (var (name, text) in tokens)
                {
                    writer.WriteLine("private static readonly Regex " + name + "REGEX = new Regex(\"^\" + @\"" + text + "\");");
                }

                writer.WriteLine("private readonly string myInput;");
                writer.WriteLine("private int myIndex;");
                writer.WriteLine("public GeneratedLexer(string input){");
                writer.WriteLine("myInput = input;");
                writer.WriteLine("}");
                writer.WriteLine("public Token NextToken(){");
                foreach (var (name, _) in tokens)
                {
                    if (name == "EPS" || name == "EOF") throw new Exception($"Token name {name} reserved, choose another name.");
                    writer.WriteLine("if(" + name + "REGEX.IsMatch(myInput.Substring(myIndex))){");
                    writer.WriteLine("var match = " + name + "REGEX.Match(myInput.Substring(myIndex));");
                    writer.WriteLine("CurrentString = match.Value;");
                    writer.WriteLine("myIndex += match.Length;");
                    writer.WriteLine("return Token." + name + ";");
                    writer.WriteLine("}");
                }

                writer.WriteLine("if(myInput.Length == myIndex){");
                writer.WriteLine("return Token.EOF;");
                writer.WriteLine("}");
                writer.WriteLine("throw new InvalidDataException();");
                writer.WriteLine("}");
                writer.WriteLine("}");
                writer.WriteLine("}");
            }
        }
    }
}