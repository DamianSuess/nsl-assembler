/*
 * DeleteINIStrInstruction.java
 */
using Java.Io;
using Java.Util;
using Nsl;

namespace Nsl.Instruction
{
    /// <remarks>@authorStuart</remarks>
    public class DeleteINIStrInstruction : AssembleExpression
    {
        public static readonly string name = "DeleteINIStr";
        private readonly Expression iniFile;
        private readonly Expression sectionName;
        private readonly Expression valueName;
        public DeleteINIStrInstruction(int returns)
        {
            if (!SectionInfo.In() && !FunctionInfo.In())
                throw new NslContextException(EnumSet.Of(NslContext.Section, NslContext.Function), name);
            if (returns > 0)
                throw new NslReturnValueException(name);
            List<Expression> paramsList = Expression.MatchList();
            if (paramsList.Count != 3)
                throw new NslArgumentException(name, 3);
            this.iniFile = paramsList[0];
            this.sectionName = paramsList[1];
            this.valueName = paramsList[2];
        }

        /// <summary>
        /// Assembles the source code.
        /// </summary>
        public override void Assemble()
        {
            Expression varOrIniFile = AssembleExpression.GetRegisterOrExpression(this.iniFile);
            Expression varOrSectionName = AssembleExpression.GetRegisterOrExpression(this.sectionName);
            Expression varOrValueName = AssembleExpression.GetRegisterOrExpression(this.valueName);
            ScriptParser.WriteLine(name + " " + varOrIniFile + " " + varOrSectionName + " " + varOrValueName);
            varOrIniFile.SetInUse(false);
            varOrSectionName.SetInUse(false);
            varOrValueName.SetInUse(false);
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