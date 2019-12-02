using System;

namespace Parser
{
    public struct Token
    {
        public Type Type { get; }
        public char Value { get; }

        public Token(Type type, char value)
        {
            Type = type;
            Value = value;
        }
    }
}