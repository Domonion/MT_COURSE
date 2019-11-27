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

    public interface IAnalyzer
    {
    }

    //S  -> AS`
    //S` -> eps
    //S` -> ^S 
    //A  -> BA`
    //A` -> eps
    //A` -> |A 
    //B  -> CB`
    //B` -> &B 
    //C  -> !C 
    //C  -> VAR
    //C  -> (S)
    public class Analyzer : IAnalyzer
    {
        [NotNull] [ItemNotNull] private ILexer<IToken> myLexer;

        public Analyzer(ILexer<IToken> lexer)
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
                    List<ITreeNode> list = new List<ITreeNode>();
                    list.Add(new Terminal(now));
                    myLexer.MoveNext();
                    list.Add(C());
                    return new Rule(list);
                }
                case Type.LPAREN:
                {
                    List<ITreeNode> list = new List<ITreeNode>();
                    list.Add(new Terminal(now));
                    myLexer.MoveNext();
                    list.Add(S());
                    list.Add(new Terminal(now));
                    myLexer.MoveNext();
                    return new Rule(list);
                }
                case Type.VAR:
                    var res = new Terminal(now);
                    myLexer.MoveNext();
                    return res;
                default:
                    throw new ParseException(myLexer.Pos);
            }
        }

        private ITreeNode B_SHTRIH()
        {
            var now = myLexer.Current;
            switch (now.Type)
            {
                case Type.AND:
                    var list = new List<ITreeNode>();
                    list.Add(new Terminal(now));
                    myLexer.MoveNext();
                    list.Add(S());
                    return new Rule(list);
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
                    list.Add(B_SHTRIH());
                    return new Rule(list);
                default:
                    throw new ParseException(myLexer.Pos);
            }
        }

        private ITreeNode A_SHTRIH()
        {
            var now = myLexer.Current;
            switch (now.Type)
            {
                case Type.OR:
                    var list = new List<ITreeNode>();
                    list.Add(new Terminal(now));
                    myLexer.MoveNext();
                    list.Add(S());
                    return new Rule(list);
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
                    list.Add(A_SHTRIH());
                    return new Rule(list);
                default:
                    throw new ParseException(myLexer.Pos);
            }
        }

        private ITreeNode S_SHTRIH()
        {
            var now = myLexer.Current;
            switch (now.Type)
            {
                case Type.XOR:
                    var list = new List<ITreeNode>();
                    list.Add(new Terminal(now));
                    myLexer.MoveNext();
                    list.Add(S());
                    return new Rule(list);
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
                    list.Add(S_SHTRIH());
                    return new Rule(list);
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