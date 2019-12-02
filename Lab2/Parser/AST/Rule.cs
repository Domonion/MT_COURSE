using System.Collections.Generic;
using System.Text;

namespace Parser.AST
{
    public class Rule : TreeNode
    {
        private readonly string myCreatedBy;
        private readonly IEnumerable<TreeNode> myDescendants;

        public Rule(IEnumerable<TreeNode> descendants, string createdBy)
        {
            myCreatedBy = createdBy;
            myDescendants = descendants;
        }

        public override string ToString()
        {
            var builder = new StringBuilder();
            foreach (var node in myDescendants)
            {
                builder.Append(node);
            }

            return builder.ToString();
        }

        public override MegaVizualizer GetInternalRepresentation()
        {
            var res = new MegaVizualizer(nameof(Rule) + ": " + myCreatedBy);
            foreach (var node in myDescendants)
            {
                res.Add(node?.GetInternalRepresentation());
            }

            return res;
        }
    }
}