namespace Parser.AST
{
    public class Terminal : ITreeNode
    {
        public readonly IToken Token;

        public Terminal(IToken token)
        {
            Token = token;
        }

        public override string ToString()
        {
            return Token.ToString();
        }
    }
}