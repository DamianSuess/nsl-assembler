/*
 * SpaceTextsInstruction.java
 */
using Java.Io;
using Java.Util;
using Nsl;

namespace Nsl.Instruction
{
    /// <remarks>@authorStuart</remarks>
    public class SpaceTextsInstruction : AssembleExpression
    {
        public static readonly string name = "SpaceTexts";
        private readonly Expression reqText;
        private readonly Expression availText;
        public SpaceTextsInstruction(int returns)
        {
            if (!ScriptParser.InGlobalContext())
                throw new NslContextException(EnumSet.Of(NslContext.Global), name);
            if (returns > 0)
                throw new NslReturnValueException(name);
            List<Expression> paramsList = Expression.MatchList();
            int paramsCount = paramsList.Count;
            if (paramsCount < 1 || paramsCount > 2)
                throw new NslArgumentException(name, 1, 2);
            this.reqText = paramsList[0];
            if (paramsCount > 1)
                this.availText = paramsList[1];
            else
                this.availText = null;
        }

        /// <summary>
        /// Assembles the source code.
        /// </summary>
        public override void Assemble()
        {
            Expression varOrReqText = AssembleExpression.GetRegisterOrExpression(this.reqText);
            if (this.availText == null)
            {
                ScriptParser.WriteLine(name + " " + varOrReqText);
            }
            else
            {
                Expression varOrAvailText = AssembleExpression.GetRegisterOrExpression(this.availText);
                ScriptParser.WriteLine(name + " " + varOrReqText + " " + varOrAvailText);
                varOrAvailText.SetInUse(false);
            }

            varOrReqText.SetInUse(false);
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