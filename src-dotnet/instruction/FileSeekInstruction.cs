/*
 * FileSeekInstruction.java
 */
using Java.Io;
using Java.Util;
using Nsl;

namespace Nsl.Instruction
{
    /// <remarks>@authorStuart</remarks>
    public class FileSeekInstruction : AssembleExpression
    {
        public static readonly string name = "FileSeek";
        private readonly Expression handle;
        private readonly Expression offset;
        private readonly Expression mode;
        public FileSeekInstruction(int returns)
        {
            if (!SectionInfo.In() && !FunctionInfo.In())
                throw new NslContextException(EnumSet.Of(NslContext.Section, NslContext.Function), name);
            if (returns > 1)
                throw new NslReturnValueException(name, 0, 1);
            List<Expression> paramsList = Expression.MatchList();
            int paramsCount = paramsList.Count;
            if (paramsCount < 2 || paramsCount > 3)
                throw new NslArgumentException(name, 2, 3);
            this.handle = paramsList[0];
            if (!this.handle.GetType().Equals(ExpressionType.Register))
                throw new NslArgumentException(name, 1, ExpressionType.Register);
            this.offset = paramsList[1];
            if (paramsCount > 2)
            {
                this.mode = paramsList[2];
                if (!ExpressionType.IsString(this.mode))
                    throw new NslArgumentException(name, 3, ExpressionType.String);
            }
            else
            {
                if (returns == 1)
                    throw new NslReturnValueException(name, 0);
                this.mode = null;
            }
        }

        /// <summary>
        /// Assembles the source code.
        /// </summary>
        public override void Assemble()
        {
            AssembleExpression.AssembleIfRequired(this.handle);
            Expression varOrOffset = AssembleExpression.GetRegisterOrExpression(this.offset);
            if (this.mode == null)
            {
                ScriptParser.WriteLine(name + " " + this.handle + " " + varOrOffset);
            }
            else
            {
                AssembleExpression.AssembleIfRequired(this.mode);
                ScriptParser.WriteLine(name + " " + this.handle + " " + varOrOffset + " " + this.mode);
            }

            varOrOffset.SetInUse(false);
        }

        /// <summary>
        /// Assembles the source code.
        /// </summary>
        public override void Assemble(Register var)
        {
            AssembleExpression.AssembleIfRequired(this.handle);
            Expression varOrOffset = AssembleExpression.GetRegisterOrExpression(this.offset);
            if (this.mode == null)
            {
                ScriptParser.WriteLine(name + " " + this.handle + " " + varOrOffset + var);
            }
            else
            {
                AssembleExpression.AssembleIfRequired(this.mode);
                ScriptParser.WriteLine(name + " " + this.handle + " " + varOrOffset + " " + this.mode + var);
            }

            varOrOffset.SetInUse(false);
        }
    }
}