/*
 * AddSizeInstruction.java
 */
using Java.Io;
using Java.Util;
using Nsl;

namespace Nsl.Instruction
{
    /// <remarks>@authorStuart</remarks>
    public class AddSizeInstruction : AssembleExpression
    {
        public static readonly string name = "AddSize";
        private readonly Expression value;
        public AddSizeInstruction(int returns)
        {
            if (!SectionInfo.In())
                throw new NslContextException(EnumSet.Of(NslContext.Section), name);
            if (returns > 0)
                throw new NslReturnValueException(name);
            List<Expression> paramsList = Expression.MatchList();
            if (paramsList.Count != 1)
                throw new NslArgumentException(name, 1);
            this.value = paramsList[0];
            if (!ExpressionType.IsInteger(this.value))
                throw new NslArgumentException(name, 1, ExpressionType.Integer);
        }

        /// <summary>
        /// Assembles the source code.
        /// </summary>
        public override void Assemble()
        {
            AssembleExpression.AssembleIfRequired(this.value);
            ScriptParser.WriteLine(name + " " + this.value);
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