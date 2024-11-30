/*
 * DeleteINISecInstruction.java
 */
using Java.Io;
using Java.Util;
using Nsl;

namespace Nsl.Instruction
{
    /// <remarks>@authorStuart</remarks>
    public class DeleteINISecInstruction : AssembleExpression
    {
        public static readonly string name = "DeleteINISec";
        private readonly Expression iniFile;
        private readonly Expression sectionName;
        public DeleteINISecInstruction(int returns)
        {
            if (!SectionInfo.In() && !FunctionInfo.In())
                throw new NslContextException(EnumSet.Of(NslContext.Section, NslContext.Function), name);
            if (returns > 0)
                throw new NslReturnValueException(name);
            List<Expression> paramsList = Expression.MatchList();
            if (paramsList.Count != 2)
                throw new NslArgumentException(name, 2);
            this.iniFile = paramsList[0];
            this.sectionName = paramsList[1];
        }

        /// <summary>
        /// Assembles the source code.
        /// </summary>
        public override void Assemble()
        {
            Expression varOrIniFile = AssembleExpression.GetRegisterOrExpression(this.iniFile);
            Expression varOrSectionName = AssembleExpression.GetRegisterOrExpression(this.sectionName);
            ScriptParser.WriteLine(name + " " + varOrIniFile + " " + varOrSectionName);
            varOrIniFile.SetInUse(false);
            varOrSectionName.SetInUse(false);
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