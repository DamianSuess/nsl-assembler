/*
 * ComponentTextInstruction.java
 */
using Java.Io;
using Java.Util;
using Nsl;

namespace Nsl.Instruction
{
    /// <remarks>@authorStuart</remarks>
    public class ComponentTextInstruction : AssembleExpression
    {
        public static readonly string name = "ComponentText";
        private readonly Expression text;
        private readonly Expression subText;
        private readonly Expression subText2;
        public ComponentTextInstruction(int returns)
        {
            if (!ScriptParser.InGlobalContext() && !PageExInfo.In())
                throw new NslContextException(EnumSet.Of(NslContext.Global, NslContext.PageEx), name);
            if (returns > 0)
                throw new NslReturnValueException(name);
            List<Expression> paramsList = Expression.MatchList();
            int paramsCount = paramsList.Count;
            if (paramsCount < 1 || paramsCount > 3)
                throw new NslArgumentException(name, 1, 3);
            this.text = paramsList[0];
            if (paramsCount > 1)
            {
                this.subText = paramsList[1];
                if (paramsCount > 2)
                {
                    this.subText2 = paramsList[2];
                }
                else
                {
                    this.subText2 = null;
                }
            }
            else
            {
                this.subText = null;
                this.subText2 = null;
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
                if (this.subText2 != null)
                {
                    Expression varOrSubText2 = AssembleExpression.GetRegisterOrExpression(this.subText2);
                    write += " " + varOrSubText2;
                    varOrSubText2.SetInUse(false);
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