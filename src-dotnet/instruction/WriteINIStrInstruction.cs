/*
 * WriteINIStrInstruction.java
 */
using Java.Io;
using Java.Util;
using Nsl;

namespace Nsl.Instruction
{
    /// <remarks>@authorStuart</remarks>
    public class WriteINIStrInstruction : AssembleExpression
    {
        public static readonly string name = "WriteINIStr";
        private readonly Expression iniFile;
        private readonly Expression sectionName;
        private readonly Expression valueName;
        private readonly Expression value;
        public WriteINIStrInstruction(int returns)
        {
            if (!SectionInfo.In() && !FunctionInfo.In())
                throw new NslContextException(EnumSet.Of(NslContext.Section, NslContext.Function), name);
            if (returns > 0)
                throw new NslReturnValueException(name);
            List<Expression> paramsList = Expression.MatchList();
            if (paramsList.Count != 4)
                throw new NslArgumentException(name, 4);
            this.iniFile = paramsList[0];
            this.sectionName = paramsList[1];
            this.valueName = paramsList[2];
            this.value = paramsList[3];
        }

        /// <summary>
        /// Assembles the source code.
        /// </summary>
        public override void Assemble()
        {
            Expression varOrIniFile = AssembleExpression.GetRegisterOrExpression(this.iniFile);
            Expression varOrSectionName = AssembleExpression.GetRegisterOrExpression(this.sectionName);
            Expression varOrValueName = AssembleExpression.GetRegisterOrExpression(this.valueName);
            Expression varOrValue = AssembleExpression.GetRegisterOrExpression(this.value);
            ScriptParser.WriteLine(name + " " + varOrIniFile + " " + varOrSectionName + " " + varOrValueName + " " + varOrValue);
            varOrIniFile.SetInUse(false);
            varOrSectionName.SetInUse(false);
            varOrValueName.SetInUse(false);
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