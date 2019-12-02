namespace Parser.AST
{
    public class And : TreeNode
    {
        public override MegaVizualizer GetInternalRepresentation()
        {
            return new MegaVizualizer("And: &");
        }

        public override string ToString()
        {
            return "&";
        }
    }
}