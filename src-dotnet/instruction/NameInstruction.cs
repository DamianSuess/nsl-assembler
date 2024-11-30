/*
 * NameInstruction.java
 */
using Java.Io;
using Java.Util;
using Nsl;

namespace Nsl.Instruction
{
    /// <remarks>@authorStuart</remarks>
    public class NameInstruction : AssembleExpression
    {
        public static readonly string name = "Name";
        private readonly Expression value;
        private readonly Expression valueDoubleAmpersands;
        public NameInstruction(int returns)
        {
            if (!ScriptParser.InGlobalContext())
                throw new NslContextException(EnumSet.Of(NslContext.Global), name);
            if (returns > 0)
                throw new NslReturnValueException(name);
            List<Expression> paramsList = Expression.MatchList();
            int paramsCount = paramsList.Count;
            if (paramsCount < 1 || paramsCount > 2)
                throw new NslArgumentException(name, 1, 2);
            this.value = paramsList[0];
            if (paramsCount > 1)
                this.valueDoubleAmpersands = paramsList[1];
            else
                this.valueDoubleAmpersands = null;
        }

        /// <summary>
        /// Assembles the source code.
        /// </summary>
        public override void Assemble()
        {
            Expression varOrValue = AssembleExpression.GetRegisterOrExpression(this.value);
            if (this.valueDoubleAmpersands == null)
            {
                ScriptParser.WriteLine(name + " " + varOrValue);
            }
            else
            {
                Expression varOrValueDoubleAmpersands = AssembleExpression.GetRegisterOrExpression(this.valueDoubleAmpersands);
                ScriptParser.WriteLine(name + " " + varOrValue + " " + varOrValueDoubleAmpersands);
                varOrValueDoubleAmpersands.SetInUse(false);
            }

            varOrValue.SetInUse(false);
        }

        /// <summary>
        /// Assembles the source code.
        /// </summary>
        public override void Assemble(Register var)
        {
            throw new NotSupportedException("Not supported.");
        }
    }
}