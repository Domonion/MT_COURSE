namespace LexerGenerator
{
    public class LexerVisitor : LexerBaseVisitor<string>
    {
        public readonly TokenContainer TokenContainer = new TokenContainer();

        public override string VisitToken(LexerParser.TokenContext context)
        {
            //TODO here is cyberbug when \' is in the end, it would 
            TokenContainer.AddToken(context.TOKEN_NAME().GetText(), context.regex().GetText().Substring(1, context.regex().GetText().Length - 2));
            return base.VisitToken(context);
        }
    }
}