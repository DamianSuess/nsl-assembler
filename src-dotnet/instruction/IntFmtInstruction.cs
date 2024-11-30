/*
 * IntFmtInstruction.java
 */
using Java.Io;
using Java.Util;
using Nsl;

namespace Nsl.Instruction
{
    /// <remarks>@authorStuart</remarks>
    public class IntFmtInstruction : AssembleExpression
    {
        public static readonly string name = "IntFmt";
        private readonly Expression format;
        private readonly Expression number;
        public IntFmtInstruction(int returns)
        {
            if (PageExInfo.In())
                throw new NslContextException(EnumSet.Of(NslContext.Section, NslContext.Function, NslContext.Global), name);
            if (returns != 1)
                throw new NslReturnValueException(name, 1);
            List<Expression> paramsList = Expression.MatchList();
            if (paramsList.Count != 2)
                throw new NslArgumentException(name, 2);
            this.format = paramsList[0];
            this.number = paramsList[1];
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
            Expression varOrFormat = AssembleExpression.GetRegisterOrExpression(this.format);
            Expression varOrNumber = AssembleExpression.GetRegisterOrExpression(this.number);
            ScriptParser.WriteLine(name + " " + var + " " + varOrFormat + " " + varOrNumber);
            varOrFormat.SetInUse(false);
            varOrNumber.SetInUse(false);
        }
    }
}