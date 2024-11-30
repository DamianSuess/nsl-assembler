/*
 * InstallColorsInstruction.java
 */
using Java.Io;
using Java.Util;
using Nsl;

namespace Nsl.Instruction
{
    /// <remarks>@authorStuart</remarks>
    public class InstallColorsInstruction : AssembleExpression
    {
        public static readonly string name = "InstallColors";
        private readonly Expression value1;
        private readonly Expression value2;
        public InstallColorsInstruction(int returns)
        {
            if (!ScriptParser.InGlobalContext())
                throw new NslContextException(EnumSet.Of(NslContext.Global), name);
            if (returns > 0)
                throw new NslReturnValueException(name);
            List<Expression> paramsList = Expression.MatchList();
            int paramsCount = paramsList.Count;
            if (paramsCount < 1 || paramsCount > 2)
                throw new NslArgumentException(name, 1, 2);
            this.value1 = paramsList[0];
            if (!ExpressionType.IsBoolean(this.value1) && !ExpressionType.IsString(this.value1))
                throw new NslArgumentException(name, 1, ExpressionType.Boolean, ExpressionType.String);

            // If 1st value is a string then 2nd value is required.
            if (ExpressionType.IsString(this.value1) && paramsCount == 1)
                throw new NslArgumentException(name, 2);
            if (paramsCount > 1)
            {
                if (!ExpressionType.IsString(this.value1))
                    throw new NslArgumentException(name, 1, ExpressionType.String);
                this.value2 = paramsList[1];
                if (!ExpressionType.IsString(this.value2))
                    throw new NslArgumentException(name, 2, ExpressionType.String);
            }
            else
                this.value2 = null;
        }

        // If 1st value is a string then 2nd value is required.
        /// <summary>
        /// Assembles the source code.
        /// </summary>
        public override void Assemble()
        {
            AssembleExpression.AssembleIfRequired(this.value1);
            if (this.value1.GetType().Equals(ExpressionType.Boolean))
            {
                if (this.value2 != null)
                    AssembleExpression.AssembleIfRequired(this.value2);
                ScriptParser.WriteLine(name + " /windows");
            }
            else
            {
                if (this.value2 != null)
                {
                    AssembleExpression.AssembleIfRequired(this.value2);
                    ScriptParser.WriteLine(name + " " + this.value1 + " " + this.value2);
                }
            }
        }

        // If 1st value is a string then 2nd value is required.
        /// <summary>
        /// Assembles the source code.
        /// </summary>
        public override void Assemble(Register var)
        {
            throw new NotSupportedException("Not supported.");
        }
    }
}