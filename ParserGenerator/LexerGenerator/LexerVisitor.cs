namespace LexerGenerator
{
    internal class LexerVisitor : LexerBaseVisitor<string>
    {
        public readonly TokenContainer TokenContainer = new TokenContainer();

        public override string VisitToken(LexerParser.TokenContext context)
        {
            TokenContainer.AddToken(context.TOKEN_NAME().GetText(), context.regex().GetText().Substring(1, context.regex().GetText().Length - 2));
            return base.VisitToken(context);
        }
    }
}