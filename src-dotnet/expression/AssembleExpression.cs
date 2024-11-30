/*
 * AssembleExpression.java
 */
using Java.Io;
using Nsl;

namespace Nsl.Expression
{
    public abstract class AssembleExpression : Expression
    {
        public override bool IsLiteral()
        {
            return false;
        }

        /// <summary>
        /// Assembles the source code.
        /// </summary>
        public abstract void Assemble();
        /// <summary>
        /// Assembles the source code.
        /// </summary>
        public abstract void Assemble(Register var);
        /// <summary>
        /// Assembles the source code.
        /// </summary>
        public static void AssembleIfRequired(Expression expression)
        {
            if (expression is AssembleExpression)
                ((AssembleExpression)expression).Assemble();
        }

        /// <summary>
        /// Assembles the source code.
        /// </summary>
        public static Expression GetRegisterOrExpression(Expression expression)
        {
            if (expression is AssembleExpression)
            {
                Register var = RegisterList.GetCurrent().GetNext();
                ((AssembleExpression)expression).Assemble(var);
                return var;
            }

            return expression;
        }
    }
}