/*
 * UninstallTextInstruction.java
 */
using Java.Io;
using Java.Util;
using Nsl;

namespace Nsl.Instruction
{
    /// <remarks>@authorStuart</remarks>
    public class UninstallTextInstruction : AssembleExpression
    {
        public static readonly string name = "UninstallText";
        private readonly Expression text;
        private readonly Expression subText;
        public UninstallTextInstruction(int returns)
        {
            if (!ScriptParser.InGlobalContext() && !PageExInfo.In())
                throw new NslContextException(EnumSet.Of(NslContext.Global, NslContext.PageEx), name);
            if (returns > 0)
                throw new NslReturnValueException(name);
            List<Expression> paramsList = Expression.MatchList();
            int paramsCount = paramsList.Count;
            if (paramsCount < 1 || paramsCount > 2)
                throw new NslArgumentException(name, 1, 2);
            this.text = paramsList[0];
            if (paramsCount > 1)
                this.subText = paramsList[1];
            else
                this.subText = null;
        }

        /// <summary>
        /// Assembles the source code.
        /// </summary>
        public override void Assemble()
        {
            Expression varOrText = AssembleExpression.GetRegisterOrExpression(this.text);
            if (this.subText == null)
            {
                ScriptParser.WriteLine(name + " " + varOrText);
            }
            else
            {
                Expression varOrSubText = AssembleExpression.GetRegisterOrExpression(this.subText);
                ScriptParser.WriteLine(name + " " + varOrText + " " + varOrSubText);
                varOrSubText.SetInUse(false);
            }

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