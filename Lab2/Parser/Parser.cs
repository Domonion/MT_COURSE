using System.Collections.Generic;
using JetBrains.Annotations;
using Parser.AST;

namespace Parser
{
    public partial class ParseException
    {
        public ParseException(int pos) : base("Incorrect grammar: " + pos)
        {
        }
    }

    public interface IParser
    {
    }

    public class Parser : IParser
    {
        [NotNull] private readonly ILexer myLexer;

        public Parser(ILexer lexer)
        {
            myLexer = lexer;
        }

        private ITreeNode C()
        {
            var now = myLexer.Current;
            switch (now.Type)
            {
                case Type.NOT:
                {
                    var list = new List<ITreeNode> {new Terminal(now)};
                    myLexer.MoveNext();
                    list.Add(C());
                    return new Rule(list, 'C');
                }
                case Type.LPAREN:
                {
                    var list = new List<ITreeNode> {new Terminal(now)};
                    myLexer.MoveNext();
                    list.Add(S());
                    list.Add(new Terminal(myLexer.Current));
                    myLexer.MoveNext();
                    return new Rule(list, 'C');
                }
                case Type.VAR:
                    var res = new Terminal(now);
                    myLexer.MoveNext();
                    return res;
                default:
                    throw new ParseException(myLexer.Pos);
            }
        }

        private ITreeNode D()
        {
            var now = myLexer.Current;
            switch (now.Type)
            {
                case Type.AND:
                    var list = new List<ITreeNode>();
                    list.Add(new Terminal(now));
                    myLexer.MoveNext();
                    list.Add(B());
                    return new Rule(list, 'D');
                case Type.OR:
                case Type.RPAREN:
                case Type.XOR:
                case Type.END:
                    return null;
                default:
                    throw new ParseException(myLexer.Pos);
            }
        }

        private ITreeNode B()
        {
            var now = myLexer.Current;
            switch (now.Type)
            {
                case Type.NOT:
                case Type.LPAREN:
                case Type.VAR:
                    var list = new List<ITreeNode>();
                    list.Add(C());
                    list.Add(D());
                    return new Rule(list, 'B');
                default:
                    throw new ParseException(myLexer.Pos);
            }
        }

        private ITreeNode E()
        {
            var now = myLexer.Current;
            switch (now.Type)
            {
                case Type.OR:
                    var list = new List<ITreeNode>();
                    list.Add(new Terminal(now));
                    myLexer.MoveNext();
                    list.Add(A());
                    return new Rule(list, 'E');
                case Type.RPAREN:
                case Type.XOR:
                case Type.END:
                    return null;
                default:
                    throw new ParseException(myLexer.Pos);
            }
        }

        private ITreeNode A()
        {
            var now = myLexer.Current;
            switch (now.Type)
            {
                case Type.NOT:
                case Type.LPAREN:
                case Type.VAR:
                    var list = new List<ITreeNode>();
                    list.Add(B());
                    list.Add(E());
                    return new Rule(list, 'A');
                default:
                    throw new ParseException(myLexer.Pos);
            }
        }

        private ITreeNode F()
        {
            var now = myLexer.Current;
            switch (now.Type)
            {
                case Type.XOR:
                    var list = new List<ITreeNode>();
                    list.Add(new Terminal(now));
                    myLexer.MoveNext();
                    list.Add(S());
                    return new Rule(list, 'F');
                case Type.RPAREN:
                case Type.END:
                    return null;
                default:
                    throw new ParseException(myLexer.Pos);
            }
        }

        private ITreeNode S()
        {
            var now = myLexer.Current;
            switch (now.Type)
            {
                case Type.NOT:
                case Type.LPAREN:
                case Type.VAR:
                    var list = new List<ITreeNode>();
                    list.Add(A());
                    list.Add(F());
                    return new Rule(list, 'S');
                default:
                    throw new ParseException(myLexer.Pos);
            }
        }

        public ITreeNode Parse()
        {
            return S();
        }
    }
}