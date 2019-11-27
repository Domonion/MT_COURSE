using System;
using System.Collections;
using System.Collections.Generic;
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

    public interface ILexer<TToken> : IEnumerable<TToken>, IEnumerator<TToken> where TToken : IToken
    {
        int Pos { get; }
    }

    public class MyLexer : ILexer<MyToken>
    {
        [NotNull] private readonly StreamReader myInput;
        private Type myType;
        private char myChar;
        public int Pos { get; private set; }

        public MyLexer([NotNull] StreamReader input)
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

        public void Dispose()
        {
            myInput.Dispose();
        }

        public IEnumerator<MyToken> GetEnumerator()
        {
            return this;
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return this;
        }

        public bool MoveNext()
        {
            if (myChar == '$')
                return false;
            ++Pos;
            myChar = GetChar();
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

            return true;
        }

        public void Reset()
        {
        }

        public MyToken Current => new MyToken(myType, myChar);

        object IEnumerator.Current => Current;
    }
}