/*
 * IfFileExistsInstruction.java
 */
using Java.Io;
using Java.Util;
using Nsl;
using Nsl.Statement;

namespace Nsl.Instruction
{
    public class IfFileExistsInstruction : JumpExpression
    {
        public static readonly string name = "FileExists";
        private readonly Expression file;
        public IfFileExistsInstruction(int returns)
        {
            if (PageExInfo.In())
                throw new NslContextException(EnumSet.Of(NslContext.Section, NslContext.Function, NslContext.Global), name);
            if (returns != 1)
                throw new NslReturnValueException(name, 1);
            List<Expression> paramsList = Expression.MatchList();
            if (paramsList.Count != 1)
                throw new NslArgumentException(name, 1);
            this.file = paramsList[0];
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
            if (this.thrownAwayAfterOptimise != null)
                AssembleExpression.AssembleIfRequired(this.thrownAwayAfterOptimise);
            Expression varOrFile = AssembleExpression.GetRegisterOrExpression(this.file);
            if (this.booleanValue)
                ScriptParser.WriteLine("IfFileExists " + varOrFile + " 0 +3");
            else
                ScriptParser.WriteLine("IfFileExists " + varOrFile + " +3");
            ScriptParser.WriteLine("StrCpy " + var + " true");
            ScriptParser.WriteLine("Goto +2");
            ScriptParser.WriteLine("StrCpy " + var + " false");
            varOrFile.SetInUse(false);
        }

        /// <summary>
        /// Assembles the source code.
        /// </summary>
        public override void Assemble(Label gotoA, Label gotoB)
        {
            if (this.thrownAwayAfterOptimise != null)
                AssembleExpression.AssembleIfRequired(this.thrownAwayAfterOptimise);
            Expression varOrFile = AssembleExpression.GetRegisterOrExpression(this.file);
            if (this.booleanValue == true)
                ScriptParser.WriteLine("IfFileExists " + varOrFile + " " + gotoA + " " + gotoB);
            else
                ScriptParser.WriteLine("IfFileExists " + varOrFile + " " + gotoB + " " + gotoA);
            varOrFile.SetInUse(false);
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
            ScriptParser.WriteLine("IfFileExists" + gotoA + gotoB);
        }
    }
}