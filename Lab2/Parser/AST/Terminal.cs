namespace Parser.AST
{
    public class Terminal : ITreeNode
    {
        public readonly IToken Token;
        public MegaVizualizer GetInternalRepresentation()
        {
            return new MegaVizualizer(nameof(Terminal) + ": " + Token.Value);
        }

        public Terminal(IToken token)
        {
            Token = token;
        }

        public override string ToString()
        {
            return Token.Value.ToString();
        }
    }
}