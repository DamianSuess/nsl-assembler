/*
 * ExecShellInstruction.java
 */
using Java.Io;
using Java.Util;
using Nsl;

namespace Nsl.Instruction
{
    /// <remarks>@authorStuart</remarks>
    public class ExecShellInstruction : AssembleExpression
    {
        public static readonly string name = "ExecShell";
        private readonly Expression action;
        private readonly Expression command;
        private readonly Expression parameters;
        private readonly Expression show;
        public ExecShellInstruction(int returns)
        {
            if (!SectionInfo.In() && !FunctionInfo.In())
                throw new NslContextException(EnumSet.Of(NslContext.Section, NslContext.Function), name);
            if (returns > 0)
                throw new NslReturnValueException(name);
            List<Expression> paramsList = Expression.MatchList();
            int paramsCount = paramsList.Count;
            if (paramsCount < 2 || paramsCount > 4)
                throw new NslArgumentException(name, 2, 4);
            this.action = paramsList[0];
            this.command = paramsList[1];
            if (paramsCount > 2)
            {
                this.parameters = paramsList[2];
                if (paramsCount > 3)
                    this.show = paramsList[3];
                else
                    this.show = null;
            }
            else
            {
                this.parameters = null;
                this.show = null;
            }
        }

        /// <summary>
        /// Assembles the source code.
        /// </summary>
        public override void Assemble()
        {
            Expression varOrAction = AssembleExpression.GetRegisterOrExpression(this.action);
            Expression varOrCommand = AssembleExpression.GetRegisterOrExpression(this.command);
            if (this.parameters != null)
            {
                AssembleExpression.AssembleIfRequired(this.parameters);
                if (this.show != null)
                {
                    AssembleExpression.AssembleIfRequired(this.show);
                    ScriptParser.WriteLine(name + " " + varOrAction + " " + varOrCommand + " " + this.parameters + " " + this.show);
                }
                else
                {
                    ScriptParser.WriteLine(name + " " + varOrAction + " " + varOrCommand + " " + this.parameters);
                }
            }
            else
            {
                ScriptParser.WriteLine(name + " " + varOrAction + " " + varOrCommand);
            }

            varOrAction.SetInUse(false);
            varOrCommand.SetInUse(false);
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