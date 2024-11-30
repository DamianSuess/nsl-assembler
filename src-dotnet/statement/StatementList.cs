/*
 * StatementList.java
 */
using Java.Io;
using Java.Util;

namespace Nsl.Statement
{
    public class StatementList : Statement
    {
        private readonly List<Statement> statementList;
        private readonly List<Statement> queuedStatementList;
        private static StatementList current;
        public static StatementList GetCurrent()
        {
            return current;
        }

        /// <summary>
        /// Class constructor.
        /// </summary>
        private StatementList()
        {
            this.statementList = new List<Statement>();
            this.queuedStatementList = new List<Statement>();
        }

        /// <summary>
        /// Class constructor.
        /// </summary>
        public static StatementList Match()
        {
            StatementList statementListParent = current;
            StatementList statementList = new StatementList();
            current = statementList;
            Statement statement;
            while ((statement = Statement.Match()) != null)
            {

                // Add the current statement.
                statementList.statementList.Add(statement);

                // Add any queued statements (i.e. contents of a macro) and then dequeue
                // them.
                if (!statementList.queuedStatementList.IsEmpty())
                {
                    statementList.statementList.AddAll(statementList.queuedStatementList);
                    statementList.queuedStatementList.Clear();
                }
            }

            current = statementListParent;
            return statementList;
        }

        /// <summary>
        /// Class constructor.
        /// </summary>
        // Add the current statement.
        // Add any queued statements (i.e. contents of a macro) and then dequeue
        // them.
        public virtual void Add(Statement statement)
        {
            this.statementList.Add(statement);
        }

        /// <summary>
        /// Class constructor.
        /// </summary>
        // Add the current statement.
        // Add any queued statements (i.e. contents of a macro) and then dequeue
        // them.
        public virtual void AddQueued(Statement statement)
        {
            this.queuedStatementList.Add(statement);
        }

        /// <summary>
        /// Class constructor.
        /// </summary>
        // Add the current statement.
        // Add any queued statements (i.e. contents of a macro) and then dequeue
        // them.
        public virtual bool IsEmpty()
        {
            return this.statementList.IsEmpty();
        }

        /// <summary>
        /// Class constructor.
        /// </summary>
        // Add the current statement.
        // Add any queued statements (i.e. contents of a macro) and then dequeue
        // them.
        public virtual Statement GetLast()
        {
            if (this.statementList.IsEmpty())
                return null;
            return this.statementList[this.statementList.Count - 1];
        }

        /// <summary>
        /// Class constructor.
        /// </summary>
        // Add the current statement.
        // Add any queued statements (i.e. contents of a macro) and then dequeue
        // them.
        /// <summary>
        /// Assembles the source code.
        /// </summary>
        public override void Assemble()
        {
            foreach (Statement statement in this.statementList)
                statement.Assemble();
        }
    }
}