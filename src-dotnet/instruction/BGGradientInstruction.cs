/*
 * BGGradientInstruction.java
 */
using Java.Io;
using Java.Util;
using Nsl;

namespace Nsl.Instruction
{
    /// <remarks>@authorStuart</remarks>
    public class BGGradientInstruction : AssembleExpression
    {
        public static readonly string name = "BGGradient";
        private readonly Expression topC;
        private readonly Expression botC;
        private readonly Expression textColor;
        public BGGradientInstruction(int returns)
        {
            if (!ScriptParser.InGlobalContext())
                throw new NslContextException(EnumSet.Of(NslContext.Global), name);
            if (returns > 0)
                throw new NslReturnValueException(name);
            List<Expression> paramsList = Expression.MatchList();
            int paramsCount = paramsList.Count;
            if (paramsCount < 1 || paramsCount > 3)
                throw new NslArgumentException(name, 1, 3);
            this.topC = paramsList[0];
            if (!ExpressionType.IsString(this.topC))
                throw new NslArgumentException(name, 1, ExpressionType.String);
            if (paramsCount > 1)
            {
                this.botC = paramsList[1];
                if (!ExpressionType.IsString(this.botC))
                    throw new NslArgumentException(name, 2, ExpressionType.String);
                if (paramsCount > 2)
                {
                    this.textColor = paramsList[2];
                    if (!ExpressionType.IsString(this.textColor))
                        throw new NslArgumentException(name, 3, ExpressionType.String);
                }
                else
                {
                    this.textColor = null;
                }
            }
            else
            {
                this.botC = null;
                this.textColor = null;
            }
        }

        /// <summary>
        /// Assembles the source code.
        /// </summary>
        public override void Assemble()
        {
            AssembleExpression.AssembleIfRequired(this.topC);
            string write = name + " " + this.topC;
            if (this.botC != null)
            {
                AssembleExpression.AssembleIfRequired(this.botC);
                write += " " + this.botC;
                if (this.textColor != null)
                {
                    AssembleExpression.AssembleIfRequired(this.textColor);
                    write += " " + this.textColor;
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