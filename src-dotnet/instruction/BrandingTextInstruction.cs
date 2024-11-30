/*
 * BrandingTextInstruction.java
 */
using Java.Io;
using Java.Util;
using Nsl;

namespace Nsl.Instruction
{
    /// <remarks>@authorStuart</remarks>
    public class BrandingTextInstruction : AssembleExpression
    {
        public static readonly string name = "BrandingText";
        private readonly Expression text;
        private readonly Expression trim;
        public BrandingTextInstruction(int returns)
        {
            if (!ScriptParser.InGlobalContext())
                throw new NslContextException(EnumSet.Of(NslContext.Global), name);
            if (returns > 0)
                throw new NslReturnValueException(name);
            List<Expression> paramsList = Expression.MatchList();
            int paramsCount = paramsList.Count;
            if (paramsCount < 1 || paramsCount > 2)
                throw new NslArgumentException(name, 1, 2);
            this.text = paramsList[0];
            if (!ExpressionType.IsString(this.text))
                throw new NslArgumentException(name, 1, ExpressionType.String);
            if (paramsCount > 1)
            {
                this.trim = paramsList[1];
                if (!ExpressionType.IsString(this.trim))
                    throw new NslArgumentException(name, 2, ExpressionType.String);
            }
            else
            {
                this.trim = null;
            }
        }

        /// <summary>
        /// Assembles the source code.
        /// </summary>
        public override void Assemble()
        {
            AssembleExpression.AssembleIfRequired(this.text);
            string write = name;
            if (this.trim != null)
            {
                AssembleExpression.AssembleIfRequired(this.trim);
                write += " /TRIM" + this.trim.ToString(true);
            }

            ScriptParser.WriteLine(write + " " + this.text);
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