/*
 * ReserveFileInstruction.java
 */
using Java.Io;
using Java.Util;
using Nsl;

namespace Nsl.Instruction
{
    public class ReserveFileInstruction : AssembleExpression
    {
        public static readonly string name = "ReserveFile";
        private readonly Expression file;
        private readonly Expression nonFatal;
        public ReserveFileInstruction(int returns)
        {
            if (!SectionInfo.In() && !FunctionInfo.In())
                throw new NslContextException(EnumSet.Of(NslContext.Section, NslContext.Function), name);
            if (returns > 0)
                throw new NslReturnValueException(name);
            List<Expression> paramsList = Expression.MatchList();
            int paramsCount = paramsList.Count;
            if (paramsCount < 1 || paramsCount > 2)
                throw new NslArgumentException(name, 1, 2);
            this.file = paramsList[0];
            if (!ExpressionType.IsString(this.file))
                throw new NslArgumentException(name, 1, ExpressionType.String);
            if (paramsCount > 1)
            {
                this.nonFatal = paramsList[1];
                if (!ExpressionType.IsBoolean(this.nonFatal))
                    throw new NslArgumentException(name, 2, ExpressionType.Boolean);
            }
            else
                this.nonFatal = null;
        }

        /// <summary>
        /// Assembles the source code.
        /// </summary>
        public override void Assemble()
        {
            string write = name + " ";
            AssembleExpression.AssembleIfRequired(this.file);
            if (this.nonFatal != null)
            {
                AssembleExpression.AssembleIfRequired(this.nonFatal);
                if (this.nonFatal.GetBooleanValue())
                    write += "/nonfatal ";
            }

            ScriptParser.WriteLine(write + this.file);
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