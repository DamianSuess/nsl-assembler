/*
 * InstTypeSetTextInstruction.java
 */
using Java.Io;
using Java.Util;
using Nsl;

namespace Nsl.Instruction
{
    /// <remarks>@authorStuart</remarks>
    public class InstTypeSetTextInstruction : AssembleExpression
    {
        public static readonly string name = "InstTypeSetText";
        private readonly Expression instType;
        private readonly Expression text;
        public InstTypeSetTextInstruction(int returns)
        {
            if (!SectionInfo.In() && !FunctionInfo.In())
                throw new NslContextException(EnumSet.Of(NslContext.Section, NslContext.Function), name);
            if (returns > 0)
                throw new NslReturnValueException(name);
            List<Expression> paramsList = Expression.MatchList();
            if (paramsList.Count != 2)
                throw new NslArgumentException(name, 2);
            this.instType = paramsList[0];
            this.text = paramsList[1];
        }

        /// <summary>
        /// Assembles the source code.
        /// </summary>
        public override void Assemble()
        {
            Expression varOrInstType = AssembleExpression.GetRegisterOrExpression(this.instType);
            Expression varOrText = AssembleExpression.GetRegisterOrExpression(this.text);
            ScriptParser.WriteLine(name + " " + varOrInstType + " " + varOrText);
            varOrInstType.SetInUse(false);
            varOrText.SetInUse(false);
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