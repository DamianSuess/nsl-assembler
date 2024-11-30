/*
 * ExpandEnvStringsInstruction.java
 */
using Java.Io;
using Java.Util;
using Nsl;

namespace Nsl.Instruction
{
    /// <remarks>@authorStuart</remarks>
    public class ExpandEnvStringsInstruction : AssembleExpression
    {
        public static readonly string name = "ExpandEnvStrings";
        private readonly Expression string;
        public ExpandEnvStringsInstruction(int returns)
        {
            if (PageExInfo.In())
                throw new NslContextException(EnumSet.Of(NslContext.Section, NslContext.Function, NslContext.Global), name);
            if (returns != 1)
                throw new NslReturnValueException(name, 1);
            List<Expression> paramsList = Expression.MatchList();
            if (paramsList.Count != 1)
                throw new NslArgumentException(name, 1);
            this.@string = paramsList[0];
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
            Expression varOrString = AssembleExpression.GetRegisterOrExpression(this.@string);
            ScriptParser.WriteLine(name + " " + var + " " + varOrString);
            varOrString.SetInUse(false);
        }
    }
}