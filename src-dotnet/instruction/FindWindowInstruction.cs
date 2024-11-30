/*
 * FindWindowInstruction.java
 */
using Java.Io;
using Java.Util;
using Nsl;

namespace Nsl.Instruction
{
    /// <remarks>@authorStuart</remarks>
    public class FindWindowInstruction : AssembleExpression
    {
        public static readonly string name = "FindWindow";
        private readonly Expression windowClass;
        private readonly Expression windowTitle;
        private readonly Expression windowParent;
        private readonly Expression childAfter;
        public FindWindowInstruction(int returns)
        {
            if (PageExInfo.In())
                throw new NslContextException(EnumSet.Of(NslContext.Section, NslContext.Function, NslContext.Global), name);
            if (returns != 1)
                throw new NslReturnValueException(name);
            List<Expression> paramsList = Expression.MatchList();
            int paramsCount = paramsList.Count;
            if (paramsCount < 1 || paramsCount > 4)
                throw new NslArgumentException(name, 1, 4);
            this.windowClass = paramsList[0];
            if (paramsCount > 1)
            {
                this.windowTitle = paramsList[1];
                if (paramsCount > 2)
                {
                    this.windowParent = paramsList[2];
                    if (paramsCount > 3)
                    {
                        this.childAfter = paramsList[3];
                    }
                    else
                    {
                        this.childAfter = null;
                    }
                }
                else
                {
                    this.windowParent = null;
                    this.childAfter = null;
                }
            }
            else
            {
                this.windowTitle = null;
                this.windowParent = null;
                this.childAfter = null;
            }
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
            Expression varOrWindowClass = AssembleExpression.GetRegisterOrExpression(this.windowClass);
            string write = name + " " + var + " " + varOrWindowClass;
            if (this.windowTitle != null)
            {
                Expression varOrWindowTitle = AssembleExpression.GetRegisterOrExpression(this.windowTitle);
                write += " " + varOrWindowTitle;
                if (this.windowParent != null)
                {
                    Expression varOrWindowParent = AssembleExpression.GetRegisterOrExpression(this.windowParent);
                    write += " " + varOrWindowParent;
                    if (this.childAfter != null)
                    {
                        Expression varOrChildAfter = AssembleExpression.GetRegisterOrExpression(this.childAfter);
                        write += " " + varOrChildAfter;
                        varOrChildAfter.SetInUse(false);
                    }

                    varOrWindowParent.SetInUse(false);
                }

                varOrWindowTitle.SetInUse(false);
            }

            ScriptParser.WriteLine(write);
            varOrWindowClass.SetInUse(false);
        }
    }
}