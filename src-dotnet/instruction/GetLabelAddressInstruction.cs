/*
 * GetLabelAddressInstruction.java
 */
using Java.Io;
using Java.Util;
using Nsl;

namespace Nsl.Instruction
{
    /// <remarks>@authorStuart</remarks>
    public class GetLabelAddressInstruction : AssembleExpression
    {
        public static readonly string name = "GetLabelAddress";
        private readonly Expression label;
        public GetLabelAddressInstruction(int returns)
        {
            if (!SectionInfo.In() && !FunctionInfo.In())
                throw new NslContextException(EnumSet.Of(NslContext.Section, NslContext.Function), name);
            if (returns != 1)
                throw new NslReturnValueException(name, 1);
            List<Expression> paramsList = Expression.MatchList();
            if (paramsList.Count != 1)
                throw new NslArgumentException(name, 1);
            this.label = paramsList[0];
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
            Expression varOrLabel = AssembleExpression.GetRegisterOrExpression(this.label);
            ScriptParser.WriteLine(name + " " + var + " " + varOrLabel);
            varOrLabel.SetInUse(false);
        }
    }
}