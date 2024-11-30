/*
 * FunctionCallStatement.java
 */
using Java.Io;
using Java.Util;
using Nsl;

namespace Nsl.Statement
{
    public class FunctionCallStatement : Statement
    {
        private readonly List<Register> returns;
        private readonly Expression functionCallExpression;
        /// <summary>
        /// Class constructor.
        /// </summary>
        public FunctionCallStatement()
        {
            this.returns = new List<Register>();
            if (ScriptParser.tokenizer.TokenIs('('))
            {
                List<Expression> returnsList = Expression.MatchRegisterList();
                foreach (Expression ret in returnsList)
                {
                    if (!ExpressionType.IsRegister(ret))
                        throw new NslException("Return parameters in front of a function call must be registers", true);
                    Scope.GetCurrent().AddVar(ret.GetIntegerValue());
                    this.returns.Add(RegisterList.GetCurrent()[ret.GetIntegerValue()]);
                }

                ScriptParser.tokenizer.MatchOrDie('=');
            }

            this.functionCallExpression = Expression.MatchConstant(this.returns.Count);
            if (!(this.functionCallExpression is AssembleExpression))
                throw new NslException("\"" + this.functionCallExpression.ToString(true) + "\" is not a valid function call", true);
            ScriptParser.tokenizer.MatchEolOrDie();
        }

        /// <summary>
        /// Class constructor.
        /// </summary>
        public FunctionCallStatement(Expression functionCallExpression)
        {
            this.returns = new List<Register>();
            this.functionCallExpression = functionCallExpression;
        }

        /// <summary>
        /// Class constructor.
        /// </summary>
        public virtual Expression GetExpression()
        {
            return this.functionCallExpression;
        }

        /// <summary>
        /// Class constructor.
        /// </summary>
        /// <summary>
        /// Assembles the source code.
        /// </summary>
        public override void Assemble()
        {
            if (this.functionCallExpression is MultipleReturnValueAssembleExpression)
            {
                ((MultipleReturnValueAssembleExpression)this.functionCallExpression).Assemble(this.returns);
            }
            else
            {
                if (this.returns.IsEmpty())
                    ((AssembleExpression)this.functionCallExpression).Assemble();
                else
                    ((AssembleExpression)this.functionCallExpression).Assemble(RegisterList.GetCurrent()[this.returns[0].GetIntegerValue()]);
            }
        }
    }
}