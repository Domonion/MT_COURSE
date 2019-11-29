using System.Collections.Generic;
using System.Text;

namespace Parser.AST
{
    public class Rule : ITreeNode
    {
        private readonly string myCreatedBy;
        public IEnumerable<ITreeNode> Descendants { get; }

        public Rule(IEnumerable<ITreeNode> descendants, string createdBy)
        {
            myCreatedBy = createdBy;
            Descendants = descendants;
        }

        public override string ToString()
        {
            var builder = new StringBuilder();
            foreach (var node in Descendants)
            {
                builder.Append(node);
            }

            return builder.ToString();
        }

        public MegaVizualizer GetInternalRepresentation()
        {
            var res = new MegaVizualizer(nameof(Rule) + ": " + myCreatedBy);
            foreach (var node in Descendants)
            {
                res.Add(node?.GetInternalRepresentation());
            }

            return res;
        }
    }
}