/*
 * CopyFilesInstruction.java
 */
using Java.Io;
using Java.Util;
using Nsl;

namespace Nsl.Instruction
{
    /// <remarks>@authorStuart</remarks>
    public class CopyFilesInstruction : AssembleExpression
    {
        public static readonly string name = "CopyFiles";
        private readonly Expression source;
        private readonly Expression dest;
        private readonly Expression silent;
        private readonly Expression filesOnly;
        private readonly Expression size;
        public CopyFilesInstruction(int returns)
        {
            if (!SectionInfo.In() && !FunctionInfo.In())
                throw new NslContextException(EnumSet.Of(NslContext.Section, NslContext.Function), name);
            if (returns > 0)
                throw new NslReturnValueException(name);
            List<Expression> paramsList = Expression.MatchList();
            int paramsCount = paramsList.Count;
            if (paramsCount < 2 || paramsCount > 5)
                throw new NslArgumentException(name, 2, 5);
            this.source = paramsList[0];
            this.dest = paramsList[1];
            if (paramsCount > 2)
            {
                this.silent = paramsList[2];
                if (!ExpressionType.IsBoolean(this.silent))
                    throw new NslArgumentException(name, 3, ExpressionType.Boolean);
                if (paramsCount > 3)
                {
                    this.filesOnly = paramsList[3];
                    if (!ExpressionType.IsBoolean(this.filesOnly))
                        throw new NslArgumentException(name, 4, ExpressionType.Boolean);
                    if (paramsCount > 4)
                    {
                        this.size = paramsList[4];
                        if (!ExpressionType.IsInteger(this.size))
                            throw new NslArgumentException(name, 5, ExpressionType.Integer);
                    }
                    else
                        this.size = null;
                }
                else
                {
                    this.filesOnly = null;
                    this.size = null;
                }
            }
            else
            {
                this.silent = null;
                this.filesOnly = null;
                this.size = null;
            }
        }

        /// <summary>
        /// Assembles the source code.
        /// </summary>
        public override void Assemble()
        {
            Expression varOrSource = AssembleExpression.GetRegisterOrExpression(this.source);
            Expression varOrDest = AssembleExpression.GetRegisterOrExpression(this.dest);
            string write = name + " ";
            if (this.silent != null)
            {
                AssembleExpression.AssembleIfRequired(this.silent);
                if (this.silent.GetBooleanValue() == true)
                    write += "/SILENT ";
            }

            if (this.filesOnly != null)
            {
                AssembleExpression.AssembleIfRequired(this.filesOnly);
                if (this.filesOnly.GetBooleanValue() == true)
                    write += "/FILESONLY ";
            }

            write += varOrSource + " " + varOrDest;
            if (this.size != null)
            {
                AssembleExpression.AssembleIfRequired(this.size);
                write += " " + this.size;
            }

            ScriptParser.WriteLine(write);
            varOrSource.SetInUse(false);
            varOrDest.SetInUse(false);
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