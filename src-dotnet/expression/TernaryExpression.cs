/*
 * TernaryExpression.java
 */
using Java.Io;
using Nsl;

namespace Nsl.Expression
{
    public class TernaryExpression : AssembleExpression
    {
        private readonly Expression leftOperand;
        private readonly Expression ifTrue;
        private readonly Expression ifFalse;
        public TernaryExpression(Expression leftOperand, Expression ifTrue, Expression ifFalse)
        {
            this.leftOperand = leftOperand;
            this.ifTrue = ifTrue;
            this.ifFalse = ifFalse;
            if (!(leftOperand is BooleanExpression) && !(leftOperand is ComparisonExpression) && !leftOperand.GetType().Equals(ExpressionType.Boolean))
                throw new NslException("The left operand must be an equality or relational expression", true);
        }

        /// <summary>
        /// Assembles the source code.
        /// </summary>
        public override void Assemble()
        {
            throw new NotSupportedException("Not supported.");
        }

        /// <summary>
        /// Assembles the source code.
        /// </summary>
        public override void Assemble(Register var)
        {
            if (this.leftOperand.IsLiteral())
            {
                if (this.leftOperand.booleanValue == true)
                {
                    if (this.ifTrue is AssembleExpression)
                        ((AssembleExpression)this.ifTrue).Assemble(var);
                    else
                        ScriptParser.WriteLine("StrCpy " + var + " " + this.ifTrue);
                }
                else
                {
                    if (this.ifFalse is AssembleExpression)
                        ((AssembleExpression)this.ifFalse).Assemble(var);
                    else
                        ScriptParser.WriteLine("StrCpy " + var + " " + this.ifFalse);
                }
            }
            else if (this.leftOperand is ConditionalExpression)
            {
                Label gotoA = LabelList.GetCurrent().GetNext();
                Label gotoB = LabelList.GetCurrent().GetNext();
                Label gotoEnd = LabelList.GetCurrent().GetNext();
                ((ConditionalExpression)this.leftOperand).Assemble(gotoA, gotoB);
                gotoA.Write();
                if (this.ifTrue is AssembleExpression)
                    ((AssembleExpression)this.ifTrue).Assemble(var);
                else
                    ScriptParser.WriteLine("StrCpy " + var + " " + this.ifTrue);
                ScriptParser.WriteLine("Goto " + gotoEnd);
                gotoB.Write();
                if (this.ifFalse is AssembleExpression)
                    ((AssembleExpression)this.ifFalse).Assemble(var);
                else
                    ScriptParser.WriteLine("StrCpy " + var + " " + this.ifFalse);
                gotoEnd.Write();
            }
            else
                throw new NslException("Expression is not a Boolean expression");
        }
    }
}