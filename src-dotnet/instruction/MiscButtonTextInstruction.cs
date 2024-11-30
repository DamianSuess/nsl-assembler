/*
 * MiscButtonTextInstruction.java
 */
using Java.Io;
using Java.Util;
using Nsl;

namespace Nsl.Instruction
{
    /// <remarks>@authorStuart</remarks>
    public class MiscButtonTextInstruction : AssembleExpression
    {
        public static readonly string name = "MiscButtonText";
        private readonly Expression backButtonText;
        private readonly Expression nextButtonText;
        private readonly Expression cancelButtonText;
        private readonly Expression closeButtonText;
        public MiscButtonTextInstruction(int returns)
        {
            if (!ScriptParser.InGlobalContext())
                throw new NslContextException(EnumSet.Of(NslContext.Global), name);
            if (returns > 0)
                throw new NslReturnValueException(name);
            List<Expression> paramsList = Expression.MatchList();
            int paramsCount = paramsList.Count;
            if (paramsCount < 1 || paramsCount > 4)
                throw new NslArgumentException(name, 1, 4);
            this.backButtonText = paramsList[0];
            if (paramsCount > 1)
            {
                this.nextButtonText = paramsList[1];
                if (paramsCount > 2)
                {
                    this.cancelButtonText = paramsList[2];
                    if (paramsCount > 3)
                        this.closeButtonText = paramsList[3];
                    else
                        this.closeButtonText = null;
                }
                else
                {
                    this.cancelButtonText = null;
                    this.closeButtonText = null;
                }
            }
            else
            {
                this.nextButtonText = null;
                this.cancelButtonText = null;
                this.closeButtonText = null;
            }
        }

        /// <summary>
        /// Assembles the source code.
        /// </summary>
        public override void Assemble()
        {
            Expression varOrbackButtonText = AssembleExpression.GetRegisterOrExpression(this.backButtonText);
            if (this.nextButtonText != null)
            {
                Expression varOrNextButtonText = AssembleExpression.GetRegisterOrExpression(this.nextButtonText);
                if (this.cancelButtonText != null)
                {
                    Expression varOrCancelButtonText = AssembleExpression.GetRegisterOrExpression(this.cancelButtonText);
                    if (this.closeButtonText != null)
                    {
                        Expression varOrCloseButtonText = AssembleExpression.GetRegisterOrExpression(this.closeButtonText);
                        ScriptParser.WriteLine(name + " " + varOrbackButtonText + " " + varOrNextButtonText + " " + varOrCancelButtonText + " " + varOrCloseButtonText);
                        this.closeButtonText.SetInUse(false);
                    }
                    else
                    {
                        ScriptParser.WriteLine(name + " " + varOrbackButtonText + " " + varOrNextButtonText + " " + varOrCancelButtonText);
                    }
                }
                else
                {
                    ScriptParser.WriteLine(name + " " + varOrbackButtonText + " " + varOrNextButtonText);
                }

                varOrNextButtonText.SetInUse(false);
            }
            else
            {
                ScriptParser.WriteLine(name + " " + varOrbackButtonText);
            }

            varOrbackButtonText.SetInUse(false);
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