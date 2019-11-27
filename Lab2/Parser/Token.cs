using System;

namespace Parser
{
    public interface IToken
    {
        Type Type { get; }
        Char Value { get; }
    }

    public struct MyToken : IToken
    {
        public Type Type { get; }
        public char Value { get; }

        public MyToken(Type type, Char value)
        {
            Type = type;
            Value = value;
        }

        public override string ToString()
        {
            return Value.ToString();
        }
    }
}