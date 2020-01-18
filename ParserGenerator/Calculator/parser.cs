using System;
using System.Collections.Generic;

namespace Generated
{
    public class GeneratedParser
    {
        private readonly GeneratedLexer myLexer;

        public GeneratedParser(GeneratedLexer lexer)
        {
            myLexer = lexer;
            Next();
        }

        public class ereturn
        {
            public double res;
        }

        public class eereturn
        {
            public double res;
        }

        public class treturn
        {
            public double res;
        }

        public class ttreturn
        {
            public double res;
        }

        public class freturn
        {
            public double res;
        }

        private Token CurrentToken;
        private string CurrentTokenString;
        private bool eofOnce = false;

        private void Next()
        {
            var next = myLexer.NextToken();
            while (next == Token.SKIP)
            {
                next = myLexer.NextToken();
            }

            if (next == Token.EOF)
            {
                if (eofOnce)
                {
                    throw new Exception("Lexer has ended.");
                }

                eofOnce = true;
            }

            CurrentToken = next;
            CurrentTokenString = myLexer.CurrentString;
        }

        public ereturn e()
        {
            if (Token.NUMBER == CurrentToken || Token.OPEN == CurrentToken || false)
            {
                var text = new List<string>();
                var myReturn = new ereturn();
                text.Add("");
                var ret0 = t();
                text.Add("");
                var ret1 = ee();
                {
                    myReturn.res = ret0.res + ret1.res;
                }
                return myReturn;
            }
            else
            {
                throw new Exception("incorrect token");
            }
        }

        public eereturn ee()
        {
            if (Token.TIERA == CurrentToken || false)
            {
                var text = new List<string>();
                var myReturn = new eereturn();
                text.Add(CurrentTokenString);
                Next();
                text.Add("");
                var ret1 = t();
                text.Add("");
                var ret2 = ee();
                {
                    if (text[0] == "+")
                    {
                        myReturn.res = ret1.res + ret2.res;
                    }
                    else
                    {
                        myReturn.res = -ret1.res + ret2.res;
                    }
                }
                return myReturn;
            }
            else if (Token.EOF == CurrentToken || Token.CLOSE == CurrentToken || false)
            {
                var text = new List<string>();
                var myReturn = new eereturn();
                {
                    myReturn.res = 0;
                }
                return myReturn;
            }
            else
            {
                throw new Exception("incorrect token");
            }
        }

        public treturn t()
        {
            if (Token.NUMBER == CurrentToken || Token.OPEN == CurrentToken || false)
            {
                var text = new List<string>();
                var myReturn = new treturn();
                text.Add("");
                var ret0 = f();
                text.Add("");
                var ret1 = tt();
                {
                    myReturn.res = ret0.res * ret1.res;
                }
                return myReturn;
            }
            else
            {
                throw new Exception("incorrect token");
            }
        }

        public ttreturn tt()
        {
            if (Token.TIERB == CurrentToken || false)
            {
                var text = new List<string>();
                var myReturn = new ttreturn();
                text.Add(CurrentTokenString);
                Next();
                text.Add("");
                var ret1 = f();
                text.Add("");
                var ret2 = tt();
                {
                    if (text[0] == "*")
                    {
                        myReturn.res = ret1.res * ret2.res;
                    }
                    else
                    {
                        myReturn.res = 1 / ret1.res * ret2.res;
                    }
                }
                return myReturn;
            }
            else if (Token.EOF == CurrentToken || Token.TIERA == CurrentToken || Token.CLOSE == CurrentToken || false)
            {
                var text = new List<string>();
                var myReturn = new ttreturn();
                {
                    myReturn.res = 1;
                }
                return myReturn;
            }
            else
            {
                throw new Exception("incorrect token");
            }
        }

        public freturn f()
        {
            if (Token.NUMBER == CurrentToken || false)
            {
                var text = new List<string>();
                var myReturn = new freturn();
                text.Add(CurrentTokenString);
                Next();
                {
                    myReturn.res = double.Parse(text[0]);
                }
                return myReturn;
            }
            else if (Token.OPEN == CurrentToken || false)
            {
                var text = new List<string>();
                var myReturn = new freturn();
                text.Add(CurrentTokenString);
                Next();
                text.Add("");
                var ret1 = e();
                text.Add(CurrentTokenString);
                Next();
                {
                    myReturn.res = ret1.res;
                }
                return myReturn;
            }
            else
            {
                throw new Exception("incorrect token");
            }
        }
    }
}