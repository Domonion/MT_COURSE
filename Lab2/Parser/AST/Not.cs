namespace Parser.AST
{
    public class Not : TreeNode
    {
        public override MegaVizualizer GetInternalRepresentation()
        {
            return new MegaVizualizer("Not: !");
        }

        public override string ToString()
        {
            return "!";
        }
    }
}