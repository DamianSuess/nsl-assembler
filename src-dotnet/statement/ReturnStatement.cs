/*
 * ReturnStatement.java
 */
using Java.Io;
using Java.Util;
using Nsl;

namespace Nsl.Statement
{
    public class ReturnStatement : Statement
    {
        private readonly List<Expression> returns;
        private bool last;
        /// <summary>
        /// Class constructor.
        /// </summary>
        public ReturnStatement()
        {
            if (!FunctionInfo.In())
                throw new NslContextException(EnumSet.Of(NslContext.Function), "return");
            if (ScriptParser.tokenizer.TokenIs('('))
            {
                this.returns = Expression.MatchList();
            }
            else
            {
                this.returns = new List<Expression>();
                this.returns.Add(Expression.MatchComplex());
            }

            ScriptParser.tokenizer.MatchEolOrDie();
            if (FunctionInfo.GetCurrent().GetReturns() == -1)
                FunctionInfo.GetCurrent().SetReturns(this.returns.Count);
            else if (FunctionInfo.GetCurrent().GetReturns() != this.returns.Count)
                throw new NslException("return statement has differing number of return values", true);
        }

        /// <summary>
        /// Class constructor.
        /// </summary>
        public virtual void SetLast(bool last)
        {
            this.last = last;
        }

        /// <summary>
        /// Class constructor.
        /// </summary>
        /// <summary>
        /// Assembles the source code.
        /// </summary>
        public override void Assemble()
        {
            for (int i = this.returns.size() - 1; i >= 0; i--)
            {
                Expression varOrReturn = AssembleExpression.GetRegisterOrExpression(this.returns[i]);
                ScriptParser.WriteLine("Push " + varOrReturn);
                varOrReturn.SetInUse(false);
            }

            if (!this.last)
                ScriptParser.WriteLine("Return");
        }
    }
}