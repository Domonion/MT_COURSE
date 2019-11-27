using System;
using System.Collections.Generic;
using System.Text;

namespace Parser.AST
{
    public class MegaVizualizer
    {
        private string head;
        List<MegaVizualizer> sons = new List<MegaVizualizer>();

        public MegaVizualizer(string val)
        {
            head = val;
        }

        public void Add(MegaVizualizer vizualizer)
        {
            sons.Add(vizualizer);
        }

        private List<string> GetLines()
        {
            var res = new List<string>();
            res.Add(head);
            var offset = new StringBuilder().Append(' ', head.Length + 2).ToString();
            foreach (var son in sons)
                if (son != null)
                {
                    List<string> sonLines = son.GetLines();
                    foreach (var line in sonLines)
                    {
                        res.Add(line.Insert(0, offset));
                    }
                }
                else
                {
                    res.Add(offset + ".");
                }

            for (var i = 1; i < res.Count; i++)
                if (res[i].Length == head.Length + 3)
                {
                    for (var j = 1; j <= i; j++)
                    {
                        var now = res[j].ToCharArray();
                        now[0] = '|';
                        res[j] = new String(now);
                    }

                    var kek = res[i].ToCharArray();
                    for (var j = 1; j < head.Length + 2; j++)
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