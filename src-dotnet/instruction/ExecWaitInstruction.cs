/*
 * ExecWaitInstruction.java
 */
using Java.Io;
using Java.Util;
using Nsl;

namespace Nsl.Instruction
{
    /// <remarks>@authorStuart</remarks>
    public class ExecWaitInstruction : AssembleExpression
    {
        public static readonly string name = "ExecWait";
        private readonly Expression command;
        public ExecWaitInstruction(int returns)
        {
            if (PageExInfo.In())
                throw new NslContextException(EnumSet.Of(NslContext.Section, NslContext.Function, NslContext.Global), name);
            if (returns > 1)
                throw new NslReturnValueException(name, 0, 1);
            List<Expression> paramsList = Expression.MatchList();
            if (paramsList.Count != 1)
                throw new NslArgumentException(name, 1);
            this.command = paramsList[0];
        }

        public override void Assemble()
        {
            Expression varOrCommand = AssembleExpression.GetRegisterOrExpression(this.command);
            ScriptParser.WriteLine(name + " " + varOrCommand);
            varOrCommand.SetInUse(false);
        }

        public override void Assemble(Register var)
        {
            Expression varOrCommand = AssembleExpression.GetRegisterOrExpression(this.command);
            ScriptParser.WriteLine(name + " " + varOrCommand + " " + var);
            varOrCommand.SetInUse(false);
        }
    }
}