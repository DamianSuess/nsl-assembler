/*
 * LogicalExpression.java
 */
namespace Nsl.Expression
{
    public abstract class LogicalExpression : AssembleExpression
    {
        protected readonly Expression leftOperand;
        protected readonly string operator;
        protected readonly Expression rightOperand;
        public LogicalExpression(Expression leftOperand, string @operator, Expression rightOperand)
        {
            this.leftOperand = leftOperand;
            this.@operator = @operator;
            this.rightOperand = rightOperand;
        }

        public virtual Expression GetLeftOperand()
        {
            return this.leftOperand;
        }

        public virtual string GetOperator()
        {
            return this.@operator;
        }

        public virtual Expression GetRightOperand()
        {
            return this.rightOperand;
        }

        public override string ToString()
        {
            return "(" + this.leftOperand + " " + this.@operator + " " + this.rightOperand + ")";
        }
    }
}