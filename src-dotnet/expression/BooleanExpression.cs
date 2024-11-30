/*
 * BooleanExpression.java
 */
using Java.Io;
using Nsl;

namespace Nsl.Expression
{
    public class BooleanExpression : ConditionalExpression
    {
        /// <summary>
        /// Class constructor.
        /// </summary>
        protected BooleanExpression() : base(null, null, null)
        {
        }

        /// <summary>
        /// Class constructor.
        /// </summary>
        public BooleanExpression(Expression leftOperand, string @operator, Expression rightOperand) : base(leftOperand, @operator, rightOperand)
        {
            if (!IsBooleanExpression(leftOperand))
                throw new NslException("The left operand must be an equality or relational expression", true);
            if (!IsBooleanExpression(rightOperand))
                throw new NslException("The right operand must be an equality or relational expression", true);
        }

        /// <summary>
        /// Class constructor.
        /// </summary>
        private static bool IsBooleanExpression(Expression expression)
        {
            return expression is BooleanExpression || expression is ComparisonExpression || expression.type.Equals(ExpressionType.Boolean);
        }

        /// <summary>
        /// Class constructor.
        /// </summary>
        public override void Assemble(Register var)
        {
            this.Assemble(RelativeJump.Zero, new RelativeJump("+3"));
            ScriptParser.WriteLine("StrCpy " + var + " true");
            ScriptParser.WriteLine("Goto +2");
            ScriptParser.WriteLine("StrCpy " + var + " false");
        }

        /// <summary>
        /// Class constructor.
        /// </summary>
        private static void Assemble(Expression expression, Label gotoA, Label gotoB)
        {
            if (expression.IsLiteral())
            {
                AssembleExpression.AssembleIfRequired(expression);
                if (expression.booleanValue == true)
                    ScriptParser.WriteLine("Goto " + gotoA);
                else
                    ScriptParser.WriteLine("Goto " + gotoB);
            }
            else if (expression is ConditionalExpression)
            {
                ((ConditionalExpression)expression).Assemble(gotoA, gotoB);
            }
            else
            {

                // Sanity check.
                throw new NslException("Expression is not a Boolean expression");
            }
        }

        /// <summary>
        /// Class constructor.
        /// </summary>
        // Sanity check.
        public virtual void Assemble(Label gotoA, Label gotoB)
        {

            // Switch the labels around if we the negate (!) operator was used.
            if (this.booleanValue)
            {
                Label gotoTemp = gotoA;
                gotoA = gotoB;
                gotoB = gotoTemp;
            }

            if (this.leftOperand is ComparisonExpression && this.rightOperand is ComparisonExpression)
            {
                if (this.@operator.Equals("&&"))
                {
                    ((ConditionalExpression)this.leftOperand).Assemble(RelativeJump.Zero, gotoB);
                    ((ConditionalExpression)this.rightOperand).Assemble(gotoA, gotoB);
                }
                else
                {
                    ((ConditionalExpression)this.leftOperand).Assemble(gotoA, RelativeJump.Zero);
                    ((ConditionalExpression)this.rightOperand).Assemble(gotoA, gotoB);
                }
            }
            else if (this.leftOperand is ComparisonExpression)
            {
                if (this.@operator.Equals("&&"))
                {
                    ((ConditionalExpression)this.leftOperand).Assemble(RelativeJump.Zero, gotoB);
                    Assemble(this.rightOperand, gotoA, gotoB);
                }
                else
                {
                    ((ConditionalExpression)this.leftOperand).Assemble(gotoA, RelativeJump.Zero);
                    Assemble(this.rightOperand, gotoA, gotoB);
                }
            }
            else if (this.rightOperand is ComparisonExpression)
            {
                if (this.@operator.Equals("&&"))
                {
                    Label gotoC = LabelList.GetCurrent().GetNext();
                    Assemble(this.leftOperand, gotoC, gotoB);
                    gotoC.Write();
                    ((ConditionalExpression)this.rightOperand).Assemble(gotoA, gotoB);
                }
                else
                {
                    Label gotoC = LabelList.GetCurrent().GetNext();
                    Assemble(this.leftOperand, gotoA, gotoC);
                    gotoC.Write();
                    ((ConditionalExpression)this.rightOperand).Assemble(gotoA, gotoB);
                }
            }
            else
            {
                if (this.@operator.Equals("&&"))
                {
                    Label gotoC = LabelList.GetCurrent().GetNext();
                    Assemble(this.leftOperand, gotoC, gotoB);
                    gotoC.Write();
                    Assemble(this.rightOperand, gotoA, gotoB);
                }
                else
                {
                    Label gotoC = LabelList.GetCurrent().GetNext();
                    Label gotoD = LabelList.GetCurrent().GetNext();
                    Assemble(this.leftOperand, gotoC, gotoD);
                    gotoD.Write();
                    Assemble(this.rightOperand, gotoC, gotoB);
                    gotoC.Write();
                }
            }
        }
    }
}