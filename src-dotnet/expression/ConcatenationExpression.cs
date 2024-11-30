/*
 * ConcatenationExpression.java
 */
using Java.Io;
using Nsl;

namespace Nsl.Expression
{
    public class ConcatenationExpression : LogicalExpression
    {
        /// <summary>
        /// Class constructor
        /// </summary>
        protected ConcatenationExpression() : base(null, null, null)
        {
        }

        /// <summary>
        /// Class constructor
        /// </summary>
        public ConcatenationExpression(Expression leftOperand, Expression rightOperand) : base(leftOperand, ".", rightOperand)
        {
            if (leftOperand.type.Equals(ExpressionType.StringSpecial) || rightOperand.type.Equals(ExpressionType.StringSpecial))
                this.type = ExpressionType.StringSpecial;
            else
                this.type = ExpressionType.String;
        }

        /// <summary>
        /// Class constructor
        /// </summary>
        private static string AssembleConcat(Expression expression)
        {
            if (expression is ConcatenationExpression)
            {
                return ((ConcatenationExpression)expression).AssembleConcat();
            }

            if (expression is AssembleExpression)
            {
                Register varLeft = RegisterList.GetCurrent().GetNext();
                ((AssembleExpression)expression).Assemble(varLeft);
                string value = varLeft.ToString();
                varLeft.SetInUse(false);
                return value;
            }

            if (ExpressionType.IsString(expression))
                return expression.stringValue;
            return expression.ToString();
        }

        /// <summary>
        /// Class constructor
        /// </summary>
        private string AssembleConcat()
        {
            return AssembleConcat(this.leftOperand) + AssembleConcat(this.rightOperand);
        }

        /// <summary>
        /// Class constructor
        /// </summary>
        /// <summary>
        /// Assembles the source code.
        /// </summary>
        public override void Assemble()
        {
            throw new NotSupportedException("Not supported.");
        }

        /// <summary>
        /// Class constructor
        /// </summary>
        /// <summary>
        /// Assembles the source code.
        /// </summary>
        public override void Assemble(Register var)
        {
            var.Substitute(this.AssembleConcat());
        }
    }
}