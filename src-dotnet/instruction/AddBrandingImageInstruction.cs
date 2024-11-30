/*
 * AddBrandingImageInstruction.java
 */
using Java.Io;
using Java.Util;
using Nsl;

namespace Nsl.Instruction
{
    /// <remarks>@authorStuart</remarks>
    public class AddBrandingImageInstruction : AssembleExpression
    {
        public static readonly string name = "AddBrandingImage";
        private readonly Expression position;
        private readonly Expression size;
        private readonly Expression padding;
        public AddBrandingImageInstruction(int returns)
        {
            if (!ScriptParser.InGlobalContext())
                throw new NslContextException(EnumSet.Of(NslContext.Global), name);
            if (returns > 0)
                throw new NslReturnValueException(name);
            List<Expression> paramsList = Expression.MatchList();
            int paramsCount = paramsList.Count;
            if (paramsCount < 2 || paramsCount > 3)
                throw new NslArgumentException(name, 2, 3);
            this.position = paramsList[0];
            if (!ExpressionType.IsString(this.position))
                throw new NslArgumentException(name, 1, ExpressionType.String);
            this.size = paramsList[1];
            if (!ExpressionType.IsString(this.size))
                throw new NslArgumentException(name, 2, ExpressionType.Integer);
            if (paramsCount > 2)
            {
                this.padding = paramsList[2];
                if (!ExpressionType.IsString(this.padding))
                    throw new NslArgumentException(name, 3, ExpressionType.Integer);
            }
            else
                this.padding = null;
        }

        /// <summary>
        /// Assembles the source code.
        /// </summary>
        public override void Assemble()
        {
            AssembleExpression.AssembleIfRequired(this.position);
            AssembleExpression.AssembleIfRequired(this.size);
            if (this.padding == null)
            {
                ScriptParser.WriteLine(name + " " + this.position + " " + this.size);
            }
            else
            {
                AssembleExpression.AssembleIfRequired(this.padding);
                ScriptParser.WriteLine(name + " " + this.position + " " + this.size + " " + this.padding);
            }
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