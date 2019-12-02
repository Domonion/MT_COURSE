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

    public class Parser
    {
        [NotNull] private readonly Lexer myLexer;

        public Parser(Lexer lexer)
        {
            myLexer = lexer;
        }

        private TreeNode C()
        {
            var now = myLexer.Current;
            switch (now.Type)
            {
                case Type.NOT:
                {
                    var list = new List<TreeNode> {new Not()};
                    myLexer.MoveNext();
                    list.Add(C());
                    return new Rule(list, nameof(C));
                }
                case Type.LPAREN:
                {
                    var list = new List<TreeNode> {new Parenthesis(now.Value)};
                    myLexer.MoveNext();
                    list.Add(S());
                    list.Add(new Parenthesis(')'));
                    myLexer.MoveNext();
                    return new Rule(list, nameof(C));
                }
                case Type.VAR:
                {
                    var list = new List<TreeNode> {new Terminal(now)};
                    myLexer.MoveNext();
                    return new Rule(list, nameof(C));
                }
                default:
                    throw new ParseException(myLexer.Pos);
            }
        }

        private TreeNode D()
        {
            var now = myLexer.Current;
            switch (now.Type)
            {
                case Type.AND:
                    var list = new List<TreeNode>();
                    list.Add(new And());
                    myLexer.MoveNext();
                    list.Add(B());
                    return new Rule(list, nameof(D));
                case Type.OR:
                case Type.RPAREN:
                case Type.XOR:
                case Type.END:
                    return null;
                default:
                    throw new ParseException(myLexer.Pos);
            }
        }

        private TreeNode B()
        {
            var now = myLexer.Current;
            switch (now.Type)
            {
                case Type.NOT:
                case Type.LPAREN:
                case Type.VAR:
                    var list = new List<TreeNode>();
                    list.Add(C());
                    list.Add(D());
                    return new Rule(list, nameof(B));
                default:
                    throw new ParseException(myLexer.Pos);
            }
        }

        private TreeNode E()
        {
            var now = myLexer.Current;
            switch (now.Type)
            {
                case Type.OR:
                    var list = new List<TreeNode> {new Or()};
                    myLexer.MoveNext();
                    list.Add(A());
                    return new Rule(list, nameof(E));
                case Type.RPAREN:
                case Type.XOR:
                case Type.END:
                    return null;
                default:
                    throw new ParseException(myLexer.Pos);
            }
        }

        private TreeNode A()
        {
            var now = myLexer.Current;
            switch (now.Type)
            {
                case Type.NOT:
                case Type.LPAREN:
                case Type.VAR:
                    var list = new List<TreeNode>();
                    list.Add(B());
                    list.Add(E());
                    return new Rule(list, nameof(A));
                default:
                    throw new ParseException(myLexer.Pos);
            }
        }

        private TreeNode F()
        {
            var now = myLexer.Current;
            switch (now.Type)
            {
                case Type.XOR:
                    var list = new List<TreeNode>{new Xor()};
                    myLexer.MoveNext();
                    list.Add(S());
                    return new Rule(list, nameof(F));
                case Type.RPAREN:
                case Type.END:
                    return null;
                default:
                    throw new ParseException(myLexer.Pos);
            }
        }

        private TreeNode S()
        {
            var now = myLexer.Current;
            switch (now.Type)
            {
                case Type.NOT:
                case Type.LPAREN:
                case Type.VAR:
                    var list = new List<TreeNode>();
                    list.Add(A());
                    list.Add(F());
                    return new Rule(list, nameof(S));
                default:
                    throw new ParseException(myLexer.Pos);
            }
        }

        public TreeNode Parse()
        {
            return S();
        }
    }
}