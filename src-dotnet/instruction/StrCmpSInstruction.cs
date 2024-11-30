/*
 * StrCmpSInstruction.java
 */
using Java.Io;
using Java.Util;
using Nsl;
using Nsl.Statement;

namespace Nsl.Instruction
{
    /// <remarks>@authorStuart</remarks>
    public class StrCmpSInstruction : JumpExpression
    {
        public static readonly string name = "StrCmpS";
        private readonly Expression str1;
        private readonly Expression str2;
        public StrCmpSInstruction(int returns)
        {
            if (PageExInfo.In())
                throw new NslContextException(EnumSet.Of(NslContext.Section, NslContext.Function, NslContext.Global), name);
            if (returns != 1)
                throw new NslReturnValueException(name, 1);
            List<Expression> paramsList = Expression.MatchList();
            if (paramsList.Count != 2)
                throw new NslArgumentException(name, 2);
            this.str1 = paramsList[0];
            this.str2 = paramsList[1];
            if (this.str1.IsLiteral() && !this.str1.GetType().Equals(ExpressionType.Register) && this.str2.IsLiteral() && !this.str2.GetType().Equals(ExpressionType.Register))
                throw new NslException("\"StrCmpS\" instruction used with literals for both arguments; use the \"==S\" or \"!=S\" equality operators instead", true);
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
            Expression varOrStr1 = AssembleExpression.GetRegisterOrExpression(this.str1);
            Expression varOrStr2 = AssembleExpression.GetRegisterOrExpression(this.str2);
            if (this.booleanValue)
                ScriptParser.WriteLine(name + " " + varOrStr1 + " " + varOrStr2 + " 0 +3");
            else
                ScriptParser.WriteLine(name + " " + varOrStr1 + " " + varOrStr2 + " +3");
            ScriptParser.WriteLine("StrCpy " + var + " true");
            ScriptParser.WriteLine("Goto +2");
            ScriptParser.WriteLine("StrCpy " + var + " false");
            varOrStr1.SetInUse(false);
            varOrStr2.SetInUse(false);
        }

        /// <summary>
        /// Assembles the source code.
        /// </summary>
        public override void Assemble(Label gotoA, Label gotoB)
        {
            if (this.thrownAwayAfterOptimise != null)
                AssembleExpression.AssembleIfRequired(this.thrownAwayAfterOptimise);
            Expression varOrStr1 = AssembleExpression.GetRegisterOrExpression(this.str1);
            Expression varOrStr2 = AssembleExpression.GetRegisterOrExpression(this.str2);
            if (this.booleanValue == true)
                ScriptParser.WriteLine(name + " " + varOrStr1 + " " + varOrStr2 + " " + gotoA + " " + gotoB);
            else
                ScriptParser.WriteLine(name + " " + varOrStr1 + " " + varOrStr2 + " " + gotoB + " " + gotoA);
            varOrStr1.SetInUse(false);
            varOrStr2.SetInUse(false);
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