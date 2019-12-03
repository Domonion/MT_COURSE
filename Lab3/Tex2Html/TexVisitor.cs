namespace Tex2Html
{
    public class TexVisitor : TexBaseVisitor<string>
    {
        public override string VisitTexExp(TexParser.TexExpContext context)
        {
            return Visit(context.@string());
        }

        public override string VisitStringExp(TexParser.StringExpContext context)
        {
            return Visit(context.expression());
        }

        public override string VisitEqExp(TexParser.EqExpContext context)
        {
            return $"<mrow> {Visit(context.expression(0))} <mo> = </mo> {Visit(context.expression(1))} </mrow>";
        }

        public override string VisitParentExp(TexParser.ParentExpContext context)
        {
            return $"<mrow> <mo> ( </mo> {Visit(context.expression())} <mo> ) </mo> </mrow>";
        }

        public override string VisitMulDivExp(TexParser.MulDivExpContext context)
        {
            var left = Visit(context.expression(0));
            var right = Visit(context.expression(1));
            var res = "";
            if (context.MULTIPLY() != null)
                res = $"<mrow> {left} <mo>*</mo> {right} </mrow>";
            if (context.DIVISION() != null)
                res = $"<mfrac> {left} {right} </mfrac>";
            return res;
        }

        public override string VisitAddSubExp(TexParser.AddSubExpContext context)
        {
            var left = Visit(context.expression(0));
            var right = Visit(context.expression(1));
            var res = "";
            if (context.PLUS() != null)
                res = $"<mrow> {left} <mo>+</mo> {right} </mrow>";
            if (context.MINUS() != null)
                res = $"<mrow> {left} <mo>-</mo> {right} </mrow>";
            return res;
        }

        public override string VisitPowExp(TexParser.PowExpContext context)
        {
            var left = Visit(context.expression(0));
            var right = Visit(context.expression(1));
            var res = "";
            if (context.UP() != null)
                res = $"<msup> {left} {right} </msup>";
            if (context.DOWN() != null)
                res = $"<msub> {left} {right} </msub>";
            return res;
        }

        public override string VisitSupsubExp(TexParser.SupsubExpContext context)
        {
            return $"<msubsup> {Visit(context.expression(0))} {Visit(context.expression(2))} {Visit(context.expression(1))} </msubsup>";
        }
        public override string VisitSubsupExp(TexParser.SubsupExpContext context)
        {
            return $"<msubsup> {Visit(context.expression(0))} {Visit(context.expression(1))} {Visit(context.expression(2))} </msubsup>";
        }

        public override string VisitNumAtomExp(TexParser.NumAtomExpContext context)
        {
            return $"<mn> {context.NUMBER()} </mn>";
        }

        public override string VisitIdAtomExp(TexParser.IdAtomExpContext context)
        {
            return $"<mi> {context.ID()} </mi>";
        }

        public override string VisitUnaryExp(TexParser.UnaryExpContext context)
        {
            return $"<mo> - </mo> {Visit(context.expression())}";
        }
    }
}