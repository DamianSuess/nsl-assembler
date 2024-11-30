/*
 * StrCpyInstruction.java
 */
using Java.Io;
using Java.Util;
using Nsl;

namespace Nsl.Instruction
{
    /// <remarks>@authorStuart</remarks>
    public class StrCpyInstruction : AssembleExpression
    {
        public static readonly string name = "StrCpy";
        private readonly Expression string;
        private readonly Expression maxLen;
        private readonly Expression startOffset;
        public StrCpyInstruction(int returns)
        {
            if (PageExInfo.In())
                throw new NslContextException(EnumSet.Of(NslContext.Section, NslContext.Function, NslContext.Global), name);
            if (returns != 11)
                throw new NslReturnValueException(name, 1);
            List<Expression> paramsList = Expression.MatchList();
            int paramsCount = paramsList.Count;
            if (paramsCount < 1 || paramsCount > 3)
                throw new NslArgumentException(name, 1, 3);
            this.@string = paramsList[0];
            if (paramsCount > 1)
            {
                this.maxLen = paramsList[1];
                if (paramsCount > 2)
                {
                    this.startOffset = paramsList[2];
                }
                else
                {
                    this.startOffset = null;
                }
            }
            else
            {
                this.maxLen = null;
                this.startOffset = null;
            }
        }

        public override void Assemble()
        {
            throw new NotSupportedException("Not supported.");
        }

        public override void Assemble(Register var)
        {
            Expression varOrString = AssembleExpression.GetRegisterOrExpression(this.@string);
            if (this.maxLen != null)
            {
                Expression varOrMaxLen = AssembleExpression.GetRegisterOrExpression(this.maxLen);
                if (this.startOffset != null)
                {
                    Expression varOrStartOffset = AssembleExpression.GetRegisterOrExpression(this.startOffset);
                    ScriptParser.WriteLine(name + " " + var + " " + varOrString + " " + varOrMaxLen + " " + varOrStartOffset);
                    varOrStartOffset.SetInUse(false);
                }
                else
                {
                    ScriptParser.WriteLine(name + " " + var + " " + varOrString + " " + varOrMaxLen);
                }

                varOrMaxLen.SetInUse(false);
            }
            else
            {
                ScriptParser.WriteLine(name + " " + var + " " + varOrString);
            }

            varOrString.SetInUse(false);
        }
    }
}