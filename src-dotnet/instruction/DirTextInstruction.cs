/*
 * DirTextInstruction.java
 */
using Java.Io;
using Java.Util;
using Nsl;

namespace Nsl.Instruction
{
    /// <remarks>@authorStuart</remarks>
    public class DirTextInstruction : AssembleExpression
    {
        public static readonly string name = "DirText";
        private readonly Expression text;
        private readonly Expression subText;
        private readonly Expression browseButtonText;
        private readonly Expression browseDlgText;
        public DirTextInstruction(int returns)
        {
            if (!ScriptParser.InGlobalContext() && !PageExInfo.In())
                throw new NslContextException(EnumSet.Of(NslContext.Global, NslContext.PageEx), name);
            if (returns > 0)
                throw new NslReturnValueException(name);
            List<Expression> paramsList = Expression.MatchList();
            int paramsCount = paramsList.Count;
            if (paramsCount < 1 || paramsCount > 4)
                throw new NslArgumentException(name, 1, 4);
            this.text = paramsList[0];
            if (paramsCount > 1)
            {
                this.subText = paramsList[1];
                if (paramsCount > 2)
                {
                    this.browseButtonText = paramsList[2];
                    if (paramsCount > 3)
                    {
                        this.browseDlgText = paramsList[3];
                    }
                    else
                    {
                        this.browseDlgText = null;
                    }
                }
                else
                {
                    this.browseButtonText = null;
                    this.browseDlgText = null;
                }
            }
            else
            {
                this.subText = null;
                this.browseButtonText = null;
                this.browseDlgText = null;
            }
        }

        /// <summary>
        /// Assembles the source code.
        /// </summary>
        public override void Assemble()
        {
            Expression varOrText = AssembleExpression.GetRegisterOrExpression(this.text);
            string write = name + " " + varOrText;
            if (this.subText != null)
            {
                Expression varOrSubText = AssembleExpression.GetRegisterOrExpression(this.subText);
                write += " " + varOrSubText;
                if (this.browseButtonText != null)
                {
                    Expression varOrBrowseButtonText = AssembleExpression.GetRegisterOrExpression(this.browseButtonText);
                    write += " " + varOrBrowseButtonText;
                    if (this.browseDlgText != null)
                    {
                        Expression varOrBrowseDlgText = AssembleExpression.GetRegisterOrExpression(this.browseDlgText);
                        write += " " + varOrBrowseDlgText;
                        varOrBrowseDlgText.SetInUse(false);
                    }

                    varOrBrowseButtonText.SetInUse(false);
                }

                varOrSubText.SetInUse(false);
            }

            ScriptParser.WriteLine(write);
            varOrText.SetInUse(false);
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