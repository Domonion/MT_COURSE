using System.Collections.Generic;
using System.Text;

namespace Parser.AST
{
    public class Rule : ITreeNode
    {
        public IEnumerable<ITreeNode> Descendants { get; }

        public Rule(IEnumerable<ITreeNode> descendants)
        {
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
    }
}