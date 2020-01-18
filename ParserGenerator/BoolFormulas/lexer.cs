using System.IO;
using System.Text.RegularExpressions;

namespace Generated
{
    public enum Token
    {
        XOR,
        OR,
        AND,
        NOT,
        OPEN,
        CLOSE,
        VAR,
        SKIP,
        EOF
    }

    public class GeneratedLexer
    {
        public string CurrentString { get; private set; }
        private static readonly Regex XORREGEX = new Regex("^" + @"\^");
        private static readonly Regex ORREGEX = new Regex("^" + @"\|");
        private static readonly Regex ANDREGEX = new Regex("^" + @"&");
        private static readonly Regex NOTREGEX = new Regex("^" + @"!");
        private static readonly Regex OPENREGEX = new Regex("^" + @"\(");
        private static readonly Regex CLOSEREGEX = new Regex("^" + @"\)");
        private static readonly Regex VARREGEX = new Regex("^" + @"(false|true)");
        private static readonly Regex SKIPREGEX = new Regex("^" + @"\s+");
        private readonly string myInput;
        private int myIndex;

        public GeneratedLexer(string input)
        {
            myInput = input;
        }

        public Token NextToken()
        {
            if (XORREGEX.IsMatch(myInput.Substring(myIndex)))
            {
                var match = XORREGEX.Match(myInput.Substring(myIndex));
                CurrentString = match.Value;
                myIndex += match.Length;
                return Token.XOR;
            }

            if (ORREGEX.IsMatch(myInput.Substring(myIndex)))
            {
                var match = ORREGEX.Match(myInput.Substring(myIndex));
                CurrentString = match.Value;
                myIndex += match.Length;
                return Token.OR;
            }

            if (ANDREGEX.IsMatch(myInput.Substring(myIndex)))
            {
                var match = ANDREGEX.Match(myInput.Substring(myIndex));
                CurrentString = match.Value;
                myIndex += match.Length;
                return Token.AND;
            }

            if (NOTREGEX.IsMatch(myInput.Substring(myIndex)))
            {
                var match = NOTREGEX.Match(myInput.Substring(myIndex));
                CurrentString = match.Value;
                myIndex += match.Length;
                return Token.NOT;
            }

            if (OPENREGEX.IsMatch(myInput.Substring(myIndex)))
            {
                var match = OPENREGEX.Match(myInput.Substring(myIndex));
                CurrentString = match.Value;
                myIndex += match.Length;
                return Token.OPEN;
            }

            if (CLOSEREGEX.IsMatch(myInput.Substring(myIndex)))
            {
                var match = CLOSEREGEX.Match(myInput.Substring(myIndex));
                CurrentString = match.Value;
                myIndex += match.Length;
                return Token.CLOSE;
            }

            if (VARREGEX.IsMatch(myInput.Substring(myIndex)))
            {
                var match = VARREGEX.Match(myInput.Substring(myIndex));
                CurrentString = match.Value;
                myIndex += match.Length;
                return Token.VAR;
            }

            if (SKIPREGEX.IsMatch(myInput.Substring(myIndex)))
            {
                var match = SKIPREGEX.Match(myInput.Substring(myIndex));
                CurrentString = match.Value;
                myIndex += match.Length;
                return Token.SKIP;
            }

            if (myInput.Length == myIndex)
            {
                return Token.EOF;
            }

            throw new InvalidDataException();
        }
    }
}