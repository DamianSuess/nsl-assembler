/*
 * GetFunctionAddressInstruction.java
 */
using Java.Io;
using Java.Util;
using Nsl;

namespace Nsl.Instruction
{
    /// <remarks>@authorStuart</remarks>
    public class GetFunctionAddressInstruction : AssembleExpression
    {
        public static readonly string name = "GetFunctionAddress";
        private readonly Expression functionName;
        public GetFunctionAddressInstruction(int returns)
        {
            if (!SectionInfo.In() && !FunctionInfo.In())
                throw new NslContextException(EnumSet.Of(NslContext.Section, NslContext.Function), name);
            if (returns != 1)
                throw new NslReturnValueException(name, 1);
            List<Expression> paramsList = Expression.MatchList();
            if (paramsList.Count != 1)
                throw new NslArgumentException(name, 1);
            this.functionName = paramsList[0];
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
            Expression varOrFunctionName = AssembleExpression.GetRegisterOrExpression(this.functionName);
            ScriptParser.WriteLine(name + " " + var + " " + varOrFunctionName);
            varOrFunctionName.SetInUse(false);
        }
    }
}