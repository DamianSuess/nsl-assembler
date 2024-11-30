/*
 * StatementExpression.java
 */
using Java.Io;
using Nsl;

namespace Nsl.Statement
{
    public class StatementExpression : Statement
    {
        private readonly AssembleExpression expression;
        public StatementExpression(AssembleExpression expression)
        {
            this.expression = expression;
        }

        public virtual Expression GetExpression()
        {
            return this.expression;
        }

        public override void Assemble()
        {
            this.expression.Assemble();
        }
    }
}