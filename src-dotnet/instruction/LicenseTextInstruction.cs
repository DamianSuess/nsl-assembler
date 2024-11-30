/*
 * LicenseTextInstruction.java
 */
using Java.Io;
using Java.Util;
using Nsl;

namespace Nsl.Instruction
{
    /// <remarks>@authorStuart</remarks>
    public class LicenseTextInstruction : AssembleExpression
    {
        public static readonly string name = "LicenseText";
        private readonly Expression text;
        private readonly Expression buttonText;
        public LicenseTextInstruction(int returns)
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
                this.buttonText = paramsList[1];
            else
                this.buttonText = null;
        }

        /// <summary>
        /// Assembles the source code.
        /// </summary>
        public override void Assemble()
        {
            Expression varOrText = AssembleExpression.GetRegisterOrExpression(this.text);
            if (this.buttonText != null)
            {
                Expression varOrButtonText = AssembleExpression.GetRegisterOrExpression(this.buttonText);
                ScriptParser.WriteLine(name + " " + varOrText + " " + varOrButtonText);
                varOrButtonText.SetInUse(false);
            }
            else
            {
                ScriptParser.WriteLine(name + " " + varOrText);
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