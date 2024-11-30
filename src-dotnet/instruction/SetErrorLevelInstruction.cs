/*
 * SetErrorLevelInstruction.java
 */
using Java.Io;
using Java.Util;
using Nsl;

namespace Nsl.Instruction
{
    /// <remarks>@authorStuart</remarks>
    public class SetErrorLevelInstruction : AssembleExpression
    {
        public static readonly string name = "SetErrorLevel";
        private readonly Expression errorLevel;
        public SetErrorLevelInstruction(int returns)
        {
            if (!SectionInfo.In() && !FunctionInfo.In())
                throw new NslContextException(EnumSet.Of(NslContext.Section, NslContext.Function), name);
            if (returns > 0)
                throw new NslReturnValueException(name);
            List<Expression> paramsList = Expression.MatchList();
            if (paramsList.Count != 1)
                throw new NslArgumentException(name, 1);
            this.errorLevel = paramsList[0];
        }

        /// <summary>
        /// Assembles the source code.
        /// </summary>
        public override void Assemble()
        {
            Expression varOrErrorLevel = AssembleExpression.GetRegisterOrExpression(this.errorLevel);
            ScriptParser.WriteLine(name + " " + varOrErrorLevel);
            varOrErrorLevel.SetInUse(false);
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