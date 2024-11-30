/*
 * GetDLLVersionInstruction.java
 */
using Java.Io;
using Java.Util;
using Nsl;

namespace Nsl.Instruction
{
    /// <remarks>@authorStuart</remarks>
    public class GetDLLVersionInstruction : MultipleReturnValueAssembleExpression
    {
        public static readonly string name = "GetDLLVersion";
        private readonly Expression file;
        public GetDLLVersionInstruction(int returns)
        {
            if (!SectionInfo.In() && !FunctionInfo.In())
                throw new NslContextException(EnumSet.Of(NslContext.Section, NslContext.Function), name);
            if (returns != 2)
                throw new NslReturnValueException(name, 2);
            List<Expression> paramsList = Expression.MatchList();
            if (paramsList.Count != 1)
                throw new NslArgumentException(name, 1);
            this.file = paramsList[0];
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
            throw new NotSupportedException("Not supported.");
        }

        /// <summary>
        /// Assembles the source code.
        /// </summary>
        public override void Assemble(List<Register> vars)
        {
            Expression varOrFile = AssembleExpression.GetRegisterOrExpression(this.file);
            ScriptParser.WriteLine(name + " " + varOrFile + " " + vars[0] + " " + vars[1]);
            varOrFile.SetInUse(false);
        }
    }
}