/*
 * ConditionalExpression.java
 */
using Java.Io;
using Nsl;

namespace Nsl.Expression
{
    public abstract class ConditionalExpression : LogicalExpression
    {
        public ConditionalExpression(Expression leftOperand, string @operator, Expression rightOperand) : base(leftOperand, @operator, rightOperand)
        {
            this.type = ExpressionType.Boolean;
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
        public abstract override void Assemble(Register var);
        /// <summary>
        /// Assembles the source code.
        /// </summary>
        public abstract void Assemble(Label gotoA, Label gotoB);
    }
}