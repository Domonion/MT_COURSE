namespace Parser.AST
{
    public class Parenthesis : TreeNode
    {

        private char myParen;

        public Parenthesis(char paren)
        {
            myParen = paren;
        }
        public override MegaVizualizer GetInternalRepresentation()
        {
            return new MegaVizualizer("Parenthesis: " + myParen);
        }

        public override string ToString()
        {
            return myParen.ToString();
        }
    }
}