/*
 * FindNextInstruction.java
 */
using Java.Io;
using Java.Util;
using Nsl;

namespace Nsl.Instruction
{
    /// <remarks>@authorStuart</remarks>
    public class FindNextInstruction : AssembleExpression
    {
        public static readonly string name = "FindNext";
        private readonly Expression handle;
        public FindNextInstruction(int returns)
        {
            if (!SectionInfo.In() && !FunctionInfo.In())
                throw new NslContextException(EnumSet.Of(NslContext.Section, NslContext.Function), name);
            if (returns != 1)
                throw new NslReturnValueException(name, 1);
            List<Expression> paramsList = Expression.MatchList();
            if (paramsList.Count != 1)
                throw new NslArgumentException(name, 1);
            this.handle = paramsList[0];
            if (!ExpressionType.IsRegister(this.handle))
                throw new NslArgumentException(name, 1, ExpressionType.Register);
        }

        /// <summary>
        /// Assembles the source code.
        /// </summary>
        public override void Assemble()
        {
            throw new NotSupportedException("Not supported.");
        }

        /// <summary>
        /// Assembles the source code.
        /// </summary>
        public override void Assemble(Register var)
        {
            AssembleExpression.AssembleIfRequired(this.handle);
            ScriptParser.WriteLine(name + " " + this.handle + " " + var);
        }
    }
}