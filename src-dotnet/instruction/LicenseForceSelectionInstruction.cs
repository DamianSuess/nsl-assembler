/*
 * LicenseForceSelectionInstruction.java
 */
using Java.Io;
using Java.Util;
using Nsl;

namespace Nsl.Instruction
{
    /// <remarks>@authorStuart</remarks>
    public class LicenseForceSelectionInstruction : AssembleExpression
    {
        public static readonly string name = "LicenseForceSelection";
        private readonly Expression value;
        private readonly Expression acceptText;
        private readonly Expression declineText;
        public LicenseForceSelectionInstruction(int returns)
        {
            if (!ScriptParser.InGlobalContext() && !PageExInfo.In())
                throw new NslContextException(EnumSet.Of(NslContext.Global, NslContext.PageEx), name);
            if (returns > 0)
                throw new NslReturnValueException(name);
            List<Expression> paramsList = Expression.MatchList();
            int paramsCount = paramsList.Count;
            if (paramsCount < 1 || paramsCount > 3)
                throw new NslArgumentException(name, 1);
            this.value = paramsList[0];
            if (!ExpressionType.IsString(this.value))
                throw new NslArgumentException(name, 1, ExpressionType.String);
            if (paramsCount > 1)
            {
                this.acceptText = paramsList[1];
                if (!ExpressionType.IsString(this.acceptText))
                    throw new NslArgumentException(name, 2, ExpressionType.String);
                if (paramsCount > 2)
                {
                    this.declineText = paramsList[2];
                    if (!ExpressionType.IsString(this.declineText))
                        throw new NslArgumentException(name, 3, ExpressionType.String);
                }
                else
                {
                    this.declineText = null;
                }
            }
            else
            {
                this.acceptText = null;
                this.declineText = null;
            }
        }

        /// <summary>
        /// Assembles the source code.
        /// </summary>
        public override void Assemble()
        {
            AssembleExpression.AssembleIfRequired(this.value);
            string write = name + " " + this.value;
            if (this.acceptText != null)
            {
                AssembleExpression.AssembleIfRequired(this.acceptText);
                write += " " + this.acceptText;
                if (this.declineText != null)
                {
                    AssembleExpression.AssembleIfRequired(this.declineText);
                    write += " " + this.declineText;
                }
            }

            ScriptParser.WriteLine(write);
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