/*
 * ReadINIStrInstruction.java
 */
using Java.Io;
using Java.Util;
using Nsl;

namespace Nsl.Instruction
{
    /// <remarks>@authorStuart</remarks>
    public class ReadINIStrInstruction : AssembleExpression
    {
        public static readonly string name = "ReadINIStr";
        private readonly Expression iniFile;
        private readonly Expression sectionName;
        private readonly Expression valueName;
        public ReadINIStrInstruction(int returns)
        {
            if (PageExInfo.In())
                throw new NslContextException(EnumSet.Of(NslContext.Section, NslContext.Function, NslContext.Global), name);
            if (returns != 1)
                throw new NslReturnValueException(name, 1);
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
            throw new NotSupportedException("Not supported.");
        }

        /// <summary>
        /// Assembles the source code.
        /// </summary>
        public override void Assemble(Register var)
        {
            Expression varOrIniFile = AssembleExpression.GetRegisterOrExpression(this.iniFile);
            Expression varOrSectionName = AssembleExpression.GetRegisterOrExpression(this.sectionName);
            Expression varOrValueName = AssembleExpression.GetRegisterOrExpression(this.valueName);
            ScriptParser.WriteLine(name + " " + var + " " + varOrIniFile + " " + varOrSectionName + " " + varOrValueName);
            varOrIniFile.SetInUse(false);
            varOrSectionName.SetInUse(false);
            varOrValueName.SetInUse(false);
        }
    }
}