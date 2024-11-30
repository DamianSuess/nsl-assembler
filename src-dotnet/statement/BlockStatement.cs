/*
 * PageStatement.java
 */
using Java.Io;
using Java.Util;
using Nsl;

namespace Nsl.Statement
{
    public class BlockStatement : Statement
    {
        private readonly Statement statementList;
        /// <summary>
        /// Class constructor.
        /// </summary>
        public BlockStatement()
        {
            if (ScriptParser.InGlobalContext())
                throw new NslContextException(EnumSet.Of(NslContext.Section, NslContext.Function), "code block");
            Scope.Create();
            if (ScriptParser.tokenizer.Match('{'))
            {
                this.statementList = StatementList.Match();
                ScriptParser.tokenizer.MatchOrDie('}');
            }
            else
            {
                this.statementList = Statement.Match();
                if (this.statementList == null)
                    throw new NslExpectedException("a statement");
            }

            Scope.GetCurrent().End();
        }

        /// <summary>
        /// Class constructor.
        /// </summary>
        public virtual bool IsEmpty()
        {
            if (this.statementList is StatementList)
                return ((StatementList)this.statementList).IsEmpty();
            return false;
        }

        /// <summary>
        /// Class constructor.
        /// </summary>
        public virtual Statement GetLast()
        {
            if (this.statementList is StatementList)
                return ((StatementList)this.statementList).GetLast();
            if (this.statementList is BlockStatement)
                return ((BlockStatement)this.statementList).GetLast();
            return this.statementList;
        }

        /// <summary>
        /// Class constructor.
        /// </summary>
        /// <summary>
        /// Assembles the source code.
        /// </summary>
        public virtual void Assemble()
        {
            if (this.statementList != null)
                this.statementList.Assemble();
        }
    }
}