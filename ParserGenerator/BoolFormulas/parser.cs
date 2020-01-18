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

        public class sreturn
        {
            public string res;
        }

        public class freturn
        {
            public string res;
        }

        public class areturn
        {
            public string res;
        }

        public class ereturn
        {
            public string res;
        }

        public class breturn
        {
            public string res;
        }

        public class dreturn
        {
            public string res;
        }

        public class creturn
        {
            public string res;
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

        public sreturn s()
        {
            if (Token.NOT == CurrentToken || Token.VAR == CurrentToken || Token.OPEN == CurrentToken || false)
            {
                var text = new List<string>();
                var myReturn = new sreturn();
                text.Add("");
                var ret0 = a();
                text.Add("");
                var ret1 = f();
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

        public freturn f()
        {
            if (Token.XOR == CurrentToken || false)
            {
                var text = new List<string>();
                var myReturn = new freturn();
                text.Add(CurrentTokenString);
                Next();
                text.Add("");
                var ret1 = s();
                {
                    myReturn.res = text[0] + ret1.res;
                }
                return myReturn;
            }
            else if (Token.EOF == CurrentToken || Token.CLOSE == CurrentToken || false)
            {
                var text = new List<string>();
                var myReturn = new freturn();
                {
                    myReturn.res = "";
                }
                return myReturn;
            }
            else
            {
                throw new Exception("incorrect token");
            }
        }

        public areturn a()
        {
            if (Token.NOT == CurrentToken || Token.VAR == CurrentToken || Token.OPEN == CurrentToken || false)
            {
                var text = new List<string>();
                var myReturn = new areturn();
                text.Add("");
                var ret0 = b();
                text.Add("");
                var ret1 = e();
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

        public ereturn e()
        {
            if (Token.OR == CurrentToken || false)
            {
                var text = new List<string>();
                var myReturn = new ereturn();
                text.Add(CurrentTokenString);
                Next();
                text.Add("");
                var ret1 = a();
                {
                    myReturn.res = text[0] + ret1.res;
                }
                return myReturn;
            }
            else if (Token.EOF == CurrentToken || Token.XOR == CurrentToken || Token.CLOSE == CurrentToken || false)
            {
                var text = new List<string>();
                var myReturn = new ereturn();
                {
                    myReturn.res = "";
                }
                return myReturn;
            }
            else
            {
                throw new Exception("incorrect token");
            }
        }

        public breturn b()
        {
            if (Token.NOT == CurrentToken || Token.VAR == CurrentToken || Token.OPEN == CurrentToken || false)
            {
                var text = new List<string>();
                var myReturn = new breturn();
                text.Add("");
                var ret0 = c();
                text.Add("");
                var ret1 = d();
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

        public dreturn d()
        {
            if (Token.AND == CurrentToken || false)
            {
                var text = new List<string>();
                var myReturn = new dreturn();
                text.Add(CurrentTokenString);
                Next();
                text.Add("");
                var ret1 = b();
                {
                    myReturn.res = text[0] + ret1.res;
                }
                return myReturn;
            }
            else if (Token.EOF == CurrentToken || Token.XOR == CurrentToken || Token.OR == CurrentToken || Token.CLOSE == CurrentToken || false)
            {
                var text = new List<string>();
                var myReturn = new dreturn();
                {
                    myReturn.res = "";
                }
                return myReturn;
            }
            else
            {
                throw new Exception("incorrect token");
            }
        }

        public creturn c()
        {
            if (Token.NOT == CurrentToken || false)
            {
                var text = new List<string>();
                var myReturn = new creturn();
                text.Add(CurrentTokenString);
                Next();
                text.Add("");
                var ret1 = c();
                {
                    myReturn.res = text[0] + ret1.res;
                }
                return myReturn;
            }
            else if (Token.VAR == CurrentToken || false)
            {
                var text = new List<string>();
                var myReturn = new creturn();
                text.Add(CurrentTokenString);
                Next();
                {
                    myReturn.res = text[0];
                }
                return myReturn;
            }
            else if (Token.OPEN == CurrentToken || false)
            {
                var text = new List<string>();
                var myReturn = new creturn();
                text.Add(CurrentTokenString);
                Next();
                text.Add("");
                var ret1 = s();
                text.Add(CurrentTokenString);
                Next();
                {
                    myReturn.res = text[0] + ret1.res + text[2];
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