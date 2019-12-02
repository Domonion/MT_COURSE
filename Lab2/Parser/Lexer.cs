using System;
using System.IO;
using JetBrains.Annotations;

namespace Parser
{
    public partial class ParseException : Exception
    {
        public ParseException(char was) : base("Unexpected token: " + was)
        {
        }
    }
    public class Lexer
    {
        [NotNull] private readonly TextReader myInput;
        private Type myType;
        private char myChar;
        public int Pos { get; private set; }

        public Lexer([NotNull] TextReader input)
        {
            myInput = input;
            MoveNext();
        }

        private char GetChar()
        {
            var last = myInput.Read();
            if (last == -1)
            {
                return '$';
            }

            return (char) last;
        }

        public void MoveNext()
        {
            if (myChar != '$')
            {
                ++Pos;
            }

            do
            {
                myChar = GetChar();
            } while (char.IsWhiteSpace(myChar));

            switch (myChar)
            {
                case '(':
                    myType = Type.LPAREN;
                    break;
                case ')':
                    myType = Type.RPAREN;
                    break;
                case '&':
                    myType = Type.AND;
                    break;
                case '|':
                    myType = Type.OR;
                    break;
                case '^':
                    myType = Type.XOR;
                    break;
                case '!':
                    myType = Type.NOT;
                    break;
                case '$':
                    myType = Type.END;
                    break;
                default:
                    if (char.IsLetter(myChar))
                    {
                        myType = Type.VAR;
                        break;
                    }

                    throw new ParseException(myChar);
            }
        }

        public Token Current => new Token(myType, myChar);
    }
}