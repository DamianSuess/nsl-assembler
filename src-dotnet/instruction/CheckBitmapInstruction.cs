/*
 * CheckBitmapInstruction.java
 */
using Java.Io;
using Java.Util;
using Nsl;

namespace Nsl.Instruction
{
    /// <remarks>@authorStuart</remarks>
    public class CheckBitmapInstruction : AssembleExpression
    {
        public static readonly string name = "CheckBitmap";
        private readonly Expression checkBitmap;
        public CheckBitmapInstruction(int returns)
        {
            if (!ScriptParser.InGlobalContext())
                throw new NslContextException(EnumSet.Of(NslContext.Global), name);
            if (returns > 0)
                throw new NslReturnValueException(name);
            List<Expression> paramsList = Expression.MatchList();
            if (paramsList.Count != 1)
                throw new NslArgumentException(name, 1);
            this.checkBitmap = paramsList[0];
            if (!ExpressionType.IsString(this.checkBitmap))
                throw new NslArgumentException(name, 1, ExpressionType.String);
        }

        /// <summary>
        /// Assembles the source code.
        /// </summary>
        public override void Assemble()
        {
            AssembleExpression.AssembleIfRequired(this.checkBitmap);
            ScriptParser.WriteLine(name + " " + this.checkBitmap);
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