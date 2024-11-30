/*
 * DetailsButtonTextInstruction.java
 */
using Java.Io;
using Java.Util;
using Nsl;

namespace Nsl.Instruction
{
    /// <remarks>@authorStuart</remarks>
    public class DetailsButtonTextInstruction : AssembleExpression
    {
        public static readonly string name = "DetailsButtonText";
        private readonly Expression text;
        public DetailsButtonTextInstruction(int returns)
        {
            if (!ScriptParser.InGlobalContext() && !PageExInfo.In())
                throw new NslContextException(EnumSet.Of(NslContext.Global, NslContext.PageEx), name);
            if (returns > 0)
                throw new NslReturnValueException(name);
            List<Expression> paramsList = Expression.MatchList();
            if (paramsList.Count != 1)
                throw new NslArgumentException(name, 1);
            this.text = paramsList[0];
        }

        /// <summary>
        /// Assembles the source code.
        /// </summary>
        public override void Assemble()
        {
            Expression varOrText = AssembleExpression.GetRegisterOrExpression(this.text);
            ScriptParser.WriteLine(name + " " + varOrText);
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