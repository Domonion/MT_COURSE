namespace Parser.AST
{
    public class Or : TreeNode
    {
        public override MegaVizualizer GetInternalRepresentation()
        {
            return new MegaVizualizer("Or: |");
        }

        public override string ToString()
        {
            return "|";
        }
    }
}