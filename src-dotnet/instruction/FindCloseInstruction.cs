/*
 * FindCloseInstruction.java
 */
using Java.Io;
using Java.Util;
using Nsl;

namespace Nsl.Instruction
{
    /// <remarks>@authorStuart</remarks>
    public class FindCloseInstruction : AssembleExpression
    {
        public static readonly string name = "FindClose";
        private readonly Expression handle;
        public FindCloseInstruction(int returns)
        {
            if (!SectionInfo.In() && !FunctionInfo.In())
                throw new NslContextException(EnumSet.Of(NslContext.Section, NslContext.Function), name);
            if (returns > 0)
                throw new NslReturnValueException(name);
            List<Expression> paramsList = Expression.MatchList();
            if (paramsList.Count != 1)
                throw new NslArgumentException(name, 1);
            this.handle = paramsList[0];
            if (!this.handle.GetType().Equals(ExpressionType.Register))
                throw new NslArgumentException(name, 1, ExpressionType.Register);
        }

        /// <summary>
        /// Assembles the source code.
        /// </summary>
        public override void Assemble()
        {
            ScriptParser.WriteLine(name + " " + this.handle);
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