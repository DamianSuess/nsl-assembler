/*
 * SectionSetFlagsInstruction.java
 */
using Java.Io;
using Java.Util;
using Nsl;

namespace Nsl.Instruction
{
    /// <remarks>@authorStuart</remarks>
    public class SectionSetFlagsInstruction : AssembleExpression
    {
        public static readonly string name = "SectionSetFlags";
        private readonly Expression index;
        private readonly Expression flags;
        public SectionSetFlagsInstruction(int returns)
        {
            if (!SectionInfo.In() && !FunctionInfo.In())
                throw new NslContextException(EnumSet.Of(NslContext.Section, NslContext.Function), name);
            if (returns > 0)
                throw new NslReturnValueException(name);
            List<Expression> paramsList = Expression.MatchList();
            if (paramsList.Count != 2)
                throw new NslArgumentException(name, 2);
            this.index = paramsList[0];
            this.flags = paramsList[1];
        }

        /// <summary>
        /// Assembles the source code.
        /// </summary>
        public override void Assemble()
        {
            Expression varOrIndex = AssembleExpression.GetRegisterOrExpression(this.index);
            Expression varOrFlags = AssembleExpression.GetRegisterOrExpression(this.flags);
            ScriptParser.WriteLine(name + " " + varOrIndex + " " + varOrFlags);
            varOrIndex.SetInUse(false);
            varOrFlags.SetInUse(false);
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