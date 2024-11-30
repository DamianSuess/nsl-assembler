/*
 * ReserveFileRecursiveInstruction.java
 */
using Java.Io;
using Java.Util;
using Nsl;

namespace Nsl.Instruction
{
    public class ReserveFileRecursiveInstruction : AssembleExpression
    {
        public static readonly string name = "ReserveFileRecursive";
        private readonly Expression file;
        private readonly Expression nonFatal;
        private readonly List<Expression> excludesList;
        public ReserveFileRecursiveInstruction(int returns)
        {
            if (!SectionInfo.In() && !FunctionInfo.In())
                throw new NslContextException(EnumSet.Of(NslContext.Section, NslContext.Function), name);
            if (returns > 0)
                throw new NslReturnValueException(name);
            List<Expression> paramsList = Expression.MatchList();
            int paramsCount = paramsList.Count;
            if (paramsCount == 0)
                throw new NslArgumentException(name, 1, 999);
            this.file = paramsList[0];
            if (!ExpressionType.IsString(this.file))
                throw new NslArgumentException(name, 1, ExpressionType.String);
            this.excludesList = new List<Expression>();
            if (paramsCount > 1)
            {
                Expression nonFatalOrExclude = paramsList[1];
                if (ExpressionType.IsBoolean(nonFatalOrExclude))
                {
                    this.nonFatal = nonFatalOrExclude;
                }
                else
                {
                    if (!ExpressionType.IsString(nonFatalOrExclude))
                        throw new NslArgumentException(name, 2, ExpressionType.String);
                    this.excludesList.Add(nonFatalOrExclude);
                    this.nonFatal = null;
                }

                for (int i = 2; i < paramsCount; i++)
                {
                    Expression exclude = paramsList[i];
                    if (!ExpressionType.IsString(exclude))
                        throw new NslArgumentException(name, i + 1, ExpressionType.String);
                    this.excludesList.Add(exclude);
                }
            }
            else
            {
                this.nonFatal = null;
            }
        }

        /// <summary>
        /// Assembles the source code.
        /// </summary>
        public override void Assemble()
        {
            string write = "ReserveFile ";
            AssembleExpression.AssembleIfRequired(this.file);
            if (this.nonFatal != null)
            {
                AssembleExpression.AssembleIfRequired(this.nonFatal);
                if (this.nonFatal.GetBooleanValue())
                    write += "/nonfatal ";
            }

            write += "/r ";
            foreach (Expression exclude in this.excludesList)
            {
                AssembleExpression.AssembleIfRequired(exclude);
                write += "/x " + exclude + " ";
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