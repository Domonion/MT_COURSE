using System.Collections.Generic;
using System.Text;

namespace Parser.AST
{
    public class MegaVizualizer
    {
        private readonly string myHead;
        private readonly List<MegaVizualizer> mySons = new List<MegaVizualizer>();

        public MegaVizualizer(string val)
        {
            myHead = val;
        }

        public void Add(MegaVizualizer vizualizer)
        {
            mySons.Add(vizualizer);
        }

        private IEnumerable<string> GetLines()
        {
            var res = new List<string> {myHead};
            var difference = myHead.Length + 3;
            var offset = new StringBuilder().Append(' ', difference).ToString();
            foreach (var son in mySons)
                if (son != null)
                {
                    var sonLines = son.GetLines();
                    foreach (var line in sonLines)
                    {
                        res.Add(line.Insert(0, offset));
                    }
                }
                else
                {
                    res.Add(offset + "none");
                }

            for (var i = 1; i < res.Count; i++)
                if (res[i].StartsWith(offset + nameof(Rule)) || res[i].StartsWith(offset + nameof(Terminal)) || res[i].StartsWith(offset + "none") ||
                    res[i].StartsWith(offset + nameof(Parenthesis))
                    || res[i].StartsWith(offset + nameof(Not)) || res[i].StartsWith(offset + nameof(Xor)) || res[i].StartsWith(offset + nameof(And)) ||
                    res[i].StartsWith(offset + nameof(Or)))
                {
                    for (var j = 1; j <= i; j++)
                    {
                        var now = res[j].ToCharArray();
                        now[0] = '|';
                        res[j] = new string(now);
                    }

                    var kek = res[i].ToCharArray();
                    for (var j = 1; j < myHead.Length + 2; j++)
                    {
                        kek[j] = '-';
                    }

                    res[i] = new string(kek);
                }

            return res;
        }

        public override string ToString()
        {
            var builder = new StringBuilder();
            var list = GetLines();
            foreach (var str in list)
            {
                builder.Append(str);
                builder.Append('\n');
            }

            return builder.ToString();
        }
    }
}