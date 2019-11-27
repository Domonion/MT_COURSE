using System;
using System.Collections.Generic;
using System.Text;

namespace Parser.AST
{
    public class Rule : ITreeNode
    {
        public char Level { get; }
        public IEnumerable<ITreeNode> Descendants { get; }

        public Rule(IEnumerable<ITreeNode> descendants, char level)
        {
            Level = level;
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

        public string GetInternalRepresentation()
        {
            var builder = new StringBuilder();
            builder.Append("(");
            builder.Append(Level);
            foreach (var node in Descendants)
            {
                builder.Append(node?.GetInternalRepresentation());
            }

            builder.Append(")");
            return builder.ToString();
        }
    }
}