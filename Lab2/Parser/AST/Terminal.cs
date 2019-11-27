namespace Parser.AST
{
    public class Terminal : ITreeNode
    {
        public readonly IToken Token;
        public char Level { get; }
        public string GetInternalRepresentation()
        {
            return Level.ToString();
        }

        public Terminal(IToken token, char level)
        {
            Level = level;
            Token = token;
        }

        public override string ToString()
        {
            return Token.Value.ToString();
        }
    }
}