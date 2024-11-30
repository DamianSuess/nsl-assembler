/*
 * SetFileAttributesInstruction.java
 */
using Java.Io;
using Java.Util;
using Nsl;

namespace Nsl.Instruction
{
    /// <remarks>@authorStuart</remarks>
    public class SetFileAttributesInstruction : AssembleExpression
    {
        public static readonly string name = "SetFileAttributes";
        private readonly Expression file;
        private readonly Expression attributes;
        public SetFileAttributesInstruction(int returns)
        {
            if (!SectionInfo.In() && !FunctionInfo.In())
                throw new NslContextException(EnumSet.Of(NslContext.Section, NslContext.Function), name);
            if (returns > 0)
                throw new NslReturnValueException(name);
            List<Expression> paramsList = Expression.MatchList();
            if (paramsList.Count != 2)
                throw new NslArgumentException(name, 2);
            this.file = paramsList[0];
            this.attributes = paramsList[1];
            if (!ExpressionType.IsString(this.attributes))
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
            Expression varOrFile = AssembleExpression.GetRegisterOrExpression(this.file);
            AssembleExpression.AssembleIfRequired(this.attributes);
            ScriptParser.WriteLine(name + " " + var + " " + varOrFile + " " + this.attributes);
            varOrFile.SetInUse(false);
        }
    }
}