using System.IO;
using System.Text.RegularExpressions;

namespace Generated
{
    public enum Token
    {
        TIERA,
        TIERB,
        NUMBER,
        OPEN,
        CLOSE,
        SKIP,
        EOF
    }

    public class GeneratedLexer
    {
        public string CurrentString { get; private set; }
        private static readonly Regex TIERAREGEX = new Regex("^" + @"[+|-]");
        private static readonly Regex TIERBREGEX = new Regex("^" + @"[*|/]");
        private static readonly Regex NUMBERREGEX = new Regex("^" + @"[1-9][0-9]*");
        private static readonly Regex OPENREGEX = new Regex("^" + @"\(");
        private static readonly Regex CLOSEREGEX = new Regex("^" + @"\)");
        private static readonly Regex SKIPREGEX = new Regex("^" + @"\s+");
        private readonly string myInput;
        private int myIndex;

        public GeneratedLexer(string input)
        {
            myInput = input;
        }

        public Token NextToken()
        {
            if (TIERAREGEX.IsMatch(myInput.Substring(myIndex)))
            {
                var match = TIERAREGEX.Match(myInput.Substring(myIndex));
                CurrentString = match.Value;
                myIndex += match.Length;
                return Token.TIERA;
            }

            if (TIERBREGEX.IsMatch(myInput.Substring(myIndex)))
            {
                var match = TIERBREGEX.Match(myInput.Substring(myIndex));
                CurrentString = match.Value;
                myIndex += match.Length;
                return Token.TIERB;
            }

            if (NUMBERREGEX.IsMatch(myInput.Substring(myIndex)))
            {
                var match = NUMBERREGEX.Match(myInput.Substring(myIndex));
                CurrentString = match.Value;
                myIndex += match.Length;
                return Token.NUMBER;
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