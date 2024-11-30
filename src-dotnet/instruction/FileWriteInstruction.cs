/*
 * FileWriteInstruction.java
 */
using Java.Io;
using Java.Util;
using Nsl;

namespace Nsl.Instruction
{
    /// <remarks>@authorStuart</remarks>
    public class FileWriteInstruction : AssembleExpression
    {
        public static readonly string name = "FileWrite";
        private readonly Expression handle;
        private readonly Expression text;
        public FileWriteInstruction(int returns)
        {
            if (!SectionInfo.In() && !FunctionInfo.In())
                throw new NslContextException(EnumSet.Of(NslContext.Section, NslContext.Function), name);
            if (returns > 0)
                throw new NslReturnValueException(name);
            List<Expression> paramsList = Expression.MatchList();
            int paramsCount = paramsList.Count;
            if (paramsCount != 2)
                throw new NslArgumentException(name, 2);
            this.handle = paramsList[0];
            if (!this.handle.GetType().Equals(ExpressionType.Register))
                throw new NslArgumentException(name, 1, ExpressionType.Register);
            this.text = paramsList[1];
        }

        /// <summary>
        /// Assembles the source code.
        /// </summary>
        public override void Assemble()
        {
            AssembleExpression.AssembleIfRequired(this.handle);
            Expression varOrText = AssembleExpression.GetRegisterOrExpression(this.text);
            ScriptParser.WriteLine(name + " " + this.handle + " " + varOrText);
            varOrText.SetInUse(false);
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