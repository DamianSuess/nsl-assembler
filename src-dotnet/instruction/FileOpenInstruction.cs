/*
 * FileOpenInstruction.java
 */
using Java.Io;
using Java.Util;
using Nsl;

namespace Nsl.Instruction
{
    /// <remarks>@authorStuart</remarks>
    public class FileOpenInstruction : AssembleExpression
    {
        public static readonly string name = "FileOpen";
        private readonly Expression fileName;
        private readonly Expression openMode;
        public FileOpenInstruction(int returns)
        {
            if (!SectionInfo.In() && !FunctionInfo.In())
                throw new NslContextException(EnumSet.Of(NslContext.Section, NslContext.Function), name);
            if (returns != 1)
                throw new NslReturnValueException(name, 1);
            List<Expression> paramsList = Expression.MatchList();
            if (paramsList.Count != 2)
                throw new NslArgumentException(name, 2);
            this.fileName = paramsList[0];
            this.openMode = paramsList[1];
            if (!ExpressionType.IsString(this.openMode))
                throw new NslArgumentException(name, 2, ExpressionType.String);
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
            AssembleExpression.AssembleIfRequired(this.openMode);
            Expression varOrFileName = AssembleExpression.GetRegisterOrExpression(this.fileName);
            ScriptParser.WriteLine(name + " " + var + " " + varOrFileName + " " + this.openMode);
            varOrFileName.SetInUse(false);
        }
    }
}