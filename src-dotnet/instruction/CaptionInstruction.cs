/*
 * CaptionInstruction.java
 */
using Java.Io;
using Java.Util;
using Nsl;

namespace Nsl.Instruction
{
    /// <remarks>@authorStuart</remarks>
    public class CaptionInstruction : AssembleExpression
    {
        public static readonly string name = "Caption";
        private readonly Expression value;
        public CaptionInstruction(int returns)
        {
            if (!ScriptParser.InGlobalContext() && !PageExInfo.In())
                throw new NslContextException(EnumSet.Of(NslContext.Global, NslContext.PageEx), name);
            if (returns > 0)
                throw new NslReturnValueException(name);
            List<Expression> paramsList = Expression.MatchList();
            int paramsCount = paramsList.Count;
            if (paramsCount != 1)
                throw new NslArgumentException(name, 1);
            this.value = paramsList[0];
        }

        /// <summary>
        /// Assembles the source code.
        /// </summary>
        public override void Assemble()
        {
            Expression varOrValue = AssembleExpression.GetRegisterOrExpression(this.value);
            ScriptParser.WriteLine(name + " " + varOrValue);
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