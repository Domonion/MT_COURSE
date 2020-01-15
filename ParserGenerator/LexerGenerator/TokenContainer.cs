using System.Collections.Generic;

namespace LexerGenerator
{
    public class TokenContainer
    {
        public readonly HashSet<(string, string)> Tokens = new HashSet<(string, string)>();

        public void AddToken(string name, string value)
        {
            Tokens.Add((name, value));
        }
    }
}