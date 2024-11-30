/*
 * InstTypeGetTextInstruction.java
 */
using Java.Io;
using Java.Util;
using Nsl;

namespace Nsl.Instruction
{
    /// <remarks>@authorStuart</remarks>
    public class InstTypeGetTextInstruction : AssembleExpression
    {
        public static readonly string name = "InstTypeGetText";
        private readonly Expression instType;
        public InstTypeGetTextInstruction(int returns)
        {
            if (PageExInfo.In())
                throw new NslContextException(EnumSet.Of(NslContext.Section, NslContext.Function, NslContext.Global), name);
            if (returns != 1)
                throw new NslReturnValueException(name, 1);
            List<Expression> paramsList = Expression.MatchList();
            if (paramsList.Count != 1)
                throw new NslArgumentException(name, 1);
            this.instType = paramsList[0];
        }

        public override void Assemble()
        {
            throw new NotSupportedException("Not supported.");
        }

        public override void Assemble(Register var)
        {
            Expression varOrInstType = AssembleExpression.GetRegisterOrExpression(this.instType);
            ScriptParser.WriteLine(name + " " + var + " " + varOrInstType);
            varOrInstType.SetInUse(false);
        }
    }
}