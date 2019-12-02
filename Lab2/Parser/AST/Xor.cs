namespace Parser.AST
{
    public class Xor : TreeNode
    {
        public override MegaVizualizer GetInternalRepresentation()
        {
            return new MegaVizualizer("Xor: ^");
        }

        public override string ToString()
        {
            return "^";
        }
    }
}