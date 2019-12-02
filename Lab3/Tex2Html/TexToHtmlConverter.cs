using Antlr4.Runtime;

namespace Tex2Html
{
    public static class TexToHtmlConverter
    {
        public static string Convert(string input)
        {
            var inputStream = new AntlrInputStream(input);
            var spreadsheetLexer = new TexLexer(inputStream);
            var commonTokenStream = new CommonTokenStream(spreadsheetLexer);
            var spreadsheetParser = new TexParser(commonTokenStream);
            var expressionContext = spreadsheetParser.tex();
            var visitor = new TexVisitor();
            return visitor.Visit(expressionContext);
        }
    }
}