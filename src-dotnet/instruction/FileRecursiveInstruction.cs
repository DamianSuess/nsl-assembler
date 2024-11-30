/*
 * FileRecursiveInstruction.java
 */
using Java.Io;
using Java.Util;
using Nsl;

namespace Nsl.Instruction
{
    public class FileRecursiveInstruction : AssembleExpression
    {
        public static readonly string name = "FileRecursive";
        private readonly Expression inFile;
        private readonly Expression nonFatal;
        private readonly Expression saveAttributes;
        private readonly List<Expression> excludesList;
        public FileRecursiveInstruction(int returns)
        {
            if (!SectionInfo.In() && !FunctionInfo.In())
                throw new NslContextException(EnumSet.Of(NslContext.Section, NslContext.Function), name);
            if (returns > 0)
                throw new NslReturnValueException(name);
            List<Expression> paramsList = Expression.MatchList();
            int paramsCount = paramsList.Count;
            if (paramsCount == 0)
                throw new NslArgumentException(name, 1, 999);
            this.inFile = paramsList[0];
            if (!ExpressionType.IsString(this.inFile))
                throw new NslArgumentException(name, 1, ExpressionType.String);
            this.excludesList = new List<Expression>();
            int excludesListStarts = 3;
            if (paramsCount > 1)
            {
                Expression nonFatalOrExclude = paramsList[1];
                if (ExpressionType.IsBoolean(nonFatalOrExclude))
                {
                    this.nonFatal = nonFatalOrExclude;
                    if (paramsCount > 2)
                    {
                        Expression saveAttributesOrExclude = paramsList[2];
                        if (ExpressionType.IsBoolean(saveAttributesOrExclude))
                        {
                            this.saveAttributes = saveAttributesOrExclude;
                        }
                        else
                        {
                            if (!ExpressionType.IsString(saveAttributesOrExclude))
                                throw new NslArgumentException(name, 3, ExpressionType.String);
                            this.excludesList.Add(saveAttributesOrExclude);
                            excludesListStarts = 3;
                            this.saveAttributes = null;
                        }
                    }
                    else
                    {
                        this.saveAttributes = null;
                    }
                }
                else
                {
                    if (!ExpressionType.IsString(nonFatalOrExclude))
                        throw new NslArgumentException(name, 2, ExpressionType.String);
                    this.excludesList.Add(nonFatalOrExclude);
                    excludesListStarts = 2;
                    this.nonFatal = null;
                    this.saveAttributes = null;
                }

                for (int i = excludesListStarts; i < paramsCount; i++)
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
                this.saveAttributes = null;
            }
        }

        public override void Assemble()
        {
            string write = "File ";
            AssembleExpression.AssembleIfRequired(this.inFile);
            if (this.nonFatal != null)
            {
                AssembleExpression.AssembleIfRequired(this.nonFatal);
                if (this.nonFatal.GetBooleanValue())
                    write += "/nonfatal ";
            }

            if (this.saveAttributes != null)
            {
                AssembleExpression.AssembleIfRequired(this.saveAttributes);
                if (this.saveAttributes.GetBooleanValue())
                    write += "/a ";
            }

            write += "/r ";
            foreach (Expression exclude in this.excludesList)
            {
                AssembleExpression.AssembleIfRequired(exclude);
                write += "/x " + exclude + " ";
            }

            ScriptParser.WriteLine(write + this.inFile);
        }

        public override void Assemble(Register var)
        {
            throw new NotSupportedException("Not supported.");
        }
    }
}