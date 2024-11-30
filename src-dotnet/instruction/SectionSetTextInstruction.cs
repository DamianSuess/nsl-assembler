/*
 * SectionSetTextInstruction.java
 */
using Java.Io;
using Java.Util;
using Nsl;

namespace Nsl.Instruction
{
    /// <remarks>@authorStuart</remarks>
    public class SectionSetTextInstruction : AssembleExpression
    {
        public static readonly string name = "SectionSetText";
        private readonly Expression index;
        private readonly Expression text;
        public SectionSetTextInstruction(int returns)
        {
            if (!SectionInfo.In() && !FunctionInfo.In())
                throw new NslContextException(EnumSet.Of(NslContext.Section, NslContext.Function), name);
            if (returns > 0)
                throw new NslReturnValueException(name);
            List<Expression> paramsList = Expression.MatchList();
            if (paramsList.Count != 2)
                throw new NslArgumentException(name, 2);
            this.index = paramsList[0];
            this.text = paramsList[1];
        }

        /// <summary>
        /// Assembles the source code.
        /// </summary>
        public override void Assemble()
        {
            Expression varOrIndex = AssembleExpression.GetRegisterOrExpression(this.index);
            Expression varOrText = AssembleExpression.GetRegisterOrExpression(this.text);
            ScriptParser.WriteLine(name + " " + varOrIndex + " " + varOrText);
            varOrIndex.SetInUse(false);
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