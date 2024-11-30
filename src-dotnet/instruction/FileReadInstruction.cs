/*
 * FileReadInstruction.java
 */
using Java.Io;
using Java.Util;
using Nsl;

namespace Nsl.Instruction
{
    /// <remarks>@authorStuart</remarks>
    public class FileReadInstruction : AssembleExpression
    {
        public static readonly string name = "FileRead";
        private readonly Expression handle;
        private readonly Expression maxLen;
        public FileReadInstruction(int returns)
        {
            if (!SectionInfo.In() && !FunctionInfo.In())
                throw new NslContextException(EnumSet.Of(NslContext.Section, NslContext.Function), name);
            if (returns != 1)
                throw new NslReturnValueException(name, 1);
            List<Expression> paramsList = Expression.MatchList();
            int paramsCount = paramsList.Count;
            if (paramsCount < 1 || paramsCount > 2)
                throw new NslArgumentException(name, 1, 2);
            this.handle = paramsList[0];
            if (!this.handle.GetType().Equals(ExpressionType.Register))
                throw new NslArgumentException(name, 1, ExpressionType.Register);
            if (paramsCount > 1)
                this.maxLen = paramsList[1];
            else
                this.maxLen = null;
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
            if (this.maxLen == null)
            {
                ScriptParser.WriteLine(name + " " + this.handle + " " + var);
            }
            else
            {
                AssembleExpression.AssembleIfRequired(this.maxLen);
                ScriptParser.WriteLine(name + " " + this.handle + " " + var + " " + this.maxLen);
            }
        }
    }
}