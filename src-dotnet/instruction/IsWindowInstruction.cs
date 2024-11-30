/*
 * IsWindowInstruction.java
 */
using Java.Io;
using Java.Util;
using Nsl;
using Nsl.Statement;

namespace Nsl.Instruction
{
    /// <remarks>@authorStuart</remarks>
    public class IsWindowInstruction : JumpExpression
    {
        public static readonly string name = "IsWindow";
        private readonly Expression hWnd;
        public IsWindowInstruction(int returns)
        {
            if (!SectionInfo.In() && !FunctionInfo.In())
                throw new NslContextException(EnumSet.Of(NslContext.Section, NslContext.Function), name);
            if (returns != 1)
                throw new NslReturnValueException(name, 1);
            List<Expression> paramsList = Expression.MatchList();
            if (paramsList.Count != 1)
                throw new NslArgumentException(name, 1);
            this.hWnd = paramsList[0];
            this.type = ExpressionType.Boolean;
            this.booleanValue = true;
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
            AssembleExpression.AssembleIfRequired(this.hWnd);
            if (this.thrownAwayAfterOptimise != null)
                AssembleExpression.AssembleIfRequired(this.thrownAwayAfterOptimise);
            if (this.booleanValue)
                ScriptParser.WriteLine(name + " " + this.hWnd + " 0 +3");
            else
                ScriptParser.WriteLine(name + " " + this.hWnd + " +3");
            ScriptParser.WriteLine("StrCpy " + var + " true");
            ScriptParser.WriteLine("Goto +2");
            ScriptParser.WriteLine("StrCpy " + var + " false");
        }

        /// <summary>
        /// Assembles the source code.
        /// </summary>
        public override void Assemble(Label gotoA, Label gotoB)
        {
            AssembleExpression.AssembleIfRequired(this.hWnd);
            if (this.thrownAwayAfterOptimise != null)
                AssembleExpression.AssembleIfRequired(this.thrownAwayAfterOptimise);
            if (this.booleanValue == true)
                ScriptParser.WriteLine(name + " " + this.hWnd + " " + gotoA + " " + gotoB);
            else
                ScriptParser.WriteLine(name + " " + this.hWnd + " " + gotoB + " " + gotoA);
        }

        /// <summary>
        /// Assembles the source code.
        /// </summary>
        public virtual void Assemble(List<SwitchCaseStatement> switchCases)
        {
            if (this.thrownAwayAfterOptimise != null)
                AssembleExpression.AssembleIfRequired(this.thrownAwayAfterOptimise);
            string gotoA = "";
            string gotoB = "";
            foreach (SwitchCaseStatement caseStatement in switchCases)
            {
                if (caseStatement.GetMatch().GetBooleanValue() == this.booleanValue)
                {
                    if (gotoA.IsEmpty())
                        gotoA = " " + caseStatement.GetLabel();
                }
                else
                {
                    if (gotoB.IsEmpty())
                        gotoB = " " + caseStatement.GetLabel();
                }
            }

            if (gotoA.IsEmpty())
                gotoA = " 0";
            ScriptParser.WriteLine(name + gotoA + gotoB);
        }
    }
}