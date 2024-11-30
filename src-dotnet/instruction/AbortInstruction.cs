/*
 * AbortInstruction.java
 */
using Java.Io;
using Java.Util;
using Nsl;

namespace Nsl.Instruction
{
    /// <remarks>@authorStuart</remarks>
    public class AbortInstruction : AssembleExpression
    {
        public static readonly string name = "Abort";
        private readonly Expression userMessage;
        public AbortInstruction(int returns)
        {
            if (!SectionInfo.In() && !FunctionInfo.In())
                throw new NslContextException(EnumSet.Of(NslContext.Section, NslContext.Function), name);
            if (returns > 0)
                throw new NslReturnValueException(name);
            List<Expression> paramsList = Expression.MatchList();
            int paramsCount = paramsList.Count;
            if (paramsCount > 1)
                throw new NslArgumentException(name, 0, 1);
            if (paramsCount > 0)
                this.userMessage = paramsList[0];
            else
                this.userMessage = null;
        }

        /// <summary>
        /// Assembles the source code.
        /// </summary>
        public override void Assemble()
        {
            if (this.userMessage == null)
            {
                ScriptParser.WriteLine(name);
            }
            else
            {
                Expression varOrUserMessage = AssembleExpression.GetRegisterOrExpression(this.userMessage);
                ScriptParser.WriteLine(name + " " + varOrUserMessage);
                varOrUserMessage.SetInUse(false);
            }
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