/*
 * RegDLLInstruction.java
 */
using Java.Io;
using Java.Util;
using Nsl;

namespace Nsl.Instruction
{
    /// <remarks>@authorStuart</remarks>
    public class RegDLLInstruction : AssembleExpression
    {
        public static readonly string name = "RegDLL";
        private readonly Expression file;
        private readonly Expression entryPoint;
        public RegDLLInstruction(int returns)
        {
            if (!SectionInfo.In() && !FunctionInfo.In())
                throw new NslContextException(EnumSet.Of(NslContext.Section, NslContext.Function), name);
            if (returns > 0)
                throw new NslReturnValueException(name);
            List<Expression> paramsList = Expression.MatchList();
            int paramsCount = paramsList.Count;
            if (paramsCount < 1 || paramsCount > 2)
                throw new NslArgumentException(name, 1, 2);
            this.file = paramsList[0];
            if (paramsCount > 1)
                this.entryPoint = paramsList[1];
            else
                this.entryPoint = null;
        }

        /// <summary>
        /// Assembles the source code.
        /// </summary>
        public override void Assemble()
        {
            Expression varOrFile = AssembleExpression.GetRegisterOrExpression(this.file);
            if (this.entryPoint == null)
            {
                ScriptParser.WriteLine(name + " " + varOrFile);
            }
            else
            {
                Expression varOrEntryPoint = AssembleExpression.GetRegisterOrExpression(this.entryPoint);
                ScriptParser.WriteLine(name + " " + varOrFile + " " + varOrEntryPoint);
                varOrEntryPoint.SetInUse(false);
            }

            varOrFile.SetInUse(false);
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