/*
 * AssignmentStatement.java
 */
using Java.Io;
using Java.Util;
using Nsl;

namespace Nsl.Statement
{
    public class AssignmentStatement : Statement
    {
        private readonly Expression assignmentExpression;
        /// <summary>
        /// Class constructor.
        /// </summary>
        public AssignmentStatement()
        {
            if (PageExInfo.In())
                throw new NslContextException(EnumSet.Of(NslContext.Global, NslContext.Section, NslContext.Function), "assignment statement");
            this.assignmentExpression = Expression.MatchComplex();
            if (!(this.assignmentExpression is AssignmentExpression))
                throw new NslExpectedException("an assignment expression");
            ScriptParser.tokenizer.MatchEolOrDie();
        }

        /// <summary>
        /// Class constructor.
        /// </summary>
        public AssignmentStatement(Expression assignmentExpression)
        {
            if (PageExInfo.In())
                throw new NslContextException(EnumSet.Of(NslContext.Global, NslContext.Section, NslContext.Function), "assignment statement");
            this.assignmentExpression = assignmentExpression;
        }

        /// <summary>
        /// Class constructor.
        /// </summary>
        public virtual Expression GetExpression()
        {
            return this.assignmentExpression;
        }

        /// <summary>
        /// Class constructor.
        /// </summary>
        /// <summary>
        /// Assembles the source code.
        /// </summary>
        public override void Assemble()
        {
            ((AssembleExpression)this.assignmentExpression).Assemble();
        }
    }
}