/*
 * SetFontInstruction.java
 */
using Java.Io;
using Java.Util;
using Nsl;

namespace Nsl.Instruction
{
    /// <remarks>@authorStuart</remarks>
    public class SetFontInstruction : AssembleExpression
    {
        public static readonly string name = "SetFont";
        private readonly Expression fontFace;
        private readonly Expression fontSize;
        private readonly Expression langId;
        public SetFontInstruction(int returns)
        {
            if (!ScriptParser.InGlobalContext())
                throw new NslContextException(EnumSet.Of(NslContext.Global), name);
            if (returns > 0)
                throw new NslReturnValueException(name);
            List<Expression> paramsList = Expression.MatchList();
            int paramsCount = paramsList.Count;
            if (paramsCount < 2 || paramsCount > 3)
                throw new NslArgumentException(name, 1);
            this.fontFace = paramsList[0];
            if (!ExpressionType.IsString(this.fontFace))
                throw new NslArgumentException(name, 1, ExpressionType.String);
            this.fontSize = paramsList[1];
            if (!ExpressionType.IsInteger(this.fontSize))
                throw new NslArgumentException(name, 2, ExpressionType.Integer);
            if (paramsCount > 1)
            {
                this.langId = paramsList[2];
                if (!ExpressionType.IsInteger(this.langId))
                    throw new NslArgumentException(name, 3, ExpressionType.Integer);
            }
            else
            {
                this.langId = null;
            }
        }

        /// <summary>
        /// Assembles the source code.
        /// </summary>
        public override void Assemble()
        {
            AssembleExpression.AssembleIfRequired(this.fontFace);
            AssembleExpression.AssembleIfRequired(this.fontSize);
            string write = name + " " + this.fontFace + " " + this.fontSize;
            if (this.langId != null)
            {
                AssembleExpression.AssembleIfRequired(this.langId);
                write += " " + this.langId;
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