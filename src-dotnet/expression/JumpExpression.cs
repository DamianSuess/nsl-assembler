/*
 * JumpExpression.java
 */
using Java.Io;
using Java.Util;
using Nsl;
using Nsl.Statement;

namespace Nsl.Expression
{
    public abstract class JumpExpression : ComparisonExpression
    {
        protected Expression thrownAwayAfterOptimise;
        /// <summary>
        /// Assembles the source code.
        /// </summary>
        public abstract override void Assemble();
        /// <summary>
        /// Assembles the source code.
        /// </summary>
        public abstract override void Assemble(Register var);
        /// <summary>
        /// Assembles the source code.
        /// </summary>
        public abstract override void Assemble(Label gotoA, Label gotoB);
        /// <summary>
        /// Assembles the source code.
        /// </summary>
        public abstract void Assemble(List<SwitchCaseStatement> switchCases);
        /// <summary>
        /// Assembles the source code.
        /// </summary>
        public virtual void CheckSwitchCases(List<SwitchCaseStatement> switchCases, int switchLineNo)
        {
            foreach (SwitchCaseStatement caseStatement in switchCases)
                if (!caseStatement.GetMatch().type.Equals(ExpressionType.Boolean))
                    throw new NslException("Invalid \"case\" value of " + caseStatement.GetMatch(), caseStatement.GetLineNo());
        }

        /// <summary>
        /// Assembles the source code.
        /// </summary>
        public virtual bool Optimise(Expression returnCheck, string @operator)
        {
            if (returnCheck.type.Equals(ExpressionType.Boolean))
            {
                this.thrownAwayAfterOptimise = returnCheck;
                this.booleanValue = @operator.Equals("==") ? returnCheck.GetBooleanValue() : !returnCheck.GetBooleanValue();
                return true;
            }

            return false;
        }
    }
}