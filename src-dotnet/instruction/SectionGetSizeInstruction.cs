/*
 * SectionGetSizeInstruction.java
 */
using Java.Io;
using Java.Util;
using Nsl;

namespace Nsl.Instruction
{
    /// <remarks>@authorStuart</remarks>
    public class SectionGetSizeInstruction : AssembleExpression
    {
        public static readonly string name = "SectionGetSize";
        private readonly Expression index;
        public SectionGetSizeInstruction(int returns)
        {
            if (!SectionInfo.In() && !FunctionInfo.In() && !PageExInfo.In())
                throw new NslContextException(EnumSet.Of(NslContext.Section, NslContext.PageEx), name);
            if (returns != 1)
                throw new NslReturnValueException(name, 1);
            List<Expression> paramsList = Expression.MatchList();
            if (paramsList.Count != 1)
                throw new NslArgumentException(name, 1);
            this.index = paramsList[0];
        }

        public override void Assemble()
        {
            throw new NotSupportedException("Not supported.");
        }

        public override void Assemble(Register var)
        {
            Expression varOrIndex = AssembleExpression.GetRegisterOrExpression(this.index);
            ScriptParser.WriteLine(name + " " + var + " " + varOrIndex);
            varOrIndex.SetInUse(false);
        }
    }
}