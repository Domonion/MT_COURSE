namespace Parser.AST
{
    public class Terminal : TreeNode
    {
        private readonly Token myToken;

        public override MegaVizualizer GetInternalRepresentation()
        {
            return new MegaVizualizer(nameof(Terminal) + ": " + myToken.Value);
        }

        public Terminal(Token token)
        {
            myToken = token;
        }

        public override string ToString()
        {
            return myToken.Value.ToString();
        }
    }
}