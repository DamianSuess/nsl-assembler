/*
 * FileInstruction.java
 */
using Java.Io;
using Java.Util;
using Nsl;

namespace Nsl.Instruction
{
    public class FileInstruction : AssembleExpression
    {
        public static readonly string name = "File";
        private readonly Expression inFile;
        private readonly Expression outFile;
        private readonly Expression nonFatal;
        private readonly Expression saveAttributes;
        public FileInstruction(int returns)
        {
            if (!SectionInfo.In() && !FunctionInfo.In())
                throw new NslContextException(EnumSet.Of(NslContext.Section, NslContext.Function), name);
            if (returns > 0)
                throw new NslReturnValueException(name);
            List<Expression> paramsList = Expression.MatchList();
            int paramsCount = paramsList.Count;
            if (paramsCount < 1 || paramsCount > 4)
                throw new NslArgumentException(name, 1, 4);
            this.inFile = paramsList[0];
            if (!ExpressionType.IsString(this.inFile))
                throw new NslArgumentException(name, 1, ExpressionType.String);
            if (paramsCount > 1)
            {
                Expression outFileOrNonFatal = paramsList[1];
                if (ExpressionType.IsBoolean(outFileOrNonFatal))
                {
                    this.outFile = null;
                    this.nonFatal = outFileOrNonFatal;
                    if (paramsCount > 2)
                    {
                        this.saveAttributes = paramsList[2];
                        if (!ExpressionType.IsBoolean(this.saveAttributes))
                            throw new NslArgumentException(name, 3, ExpressionType.Boolean);
                    }
                    else
                        this.saveAttributes = null;
                }
                else
                {
                    this.outFile = outFileOrNonFatal;
                    if (paramsCount > 2)
                    {
                        this.nonFatal = paramsList[2];
                        if (!ExpressionType.IsBoolean(this.nonFatal))
                            throw new NslArgumentException(name, 3, ExpressionType.Boolean);
                        if (paramsCount > 3)
                        {
                            this.saveAttributes = paramsList[3];
                            if (!ExpressionType.IsBoolean(this.saveAttributes))
                                throw new NslArgumentException(name, 4, ExpressionType.Boolean);
                        }
                        else
                            this.saveAttributes = null;
                    }
                    else
                    {
                        this.nonFatal = null;
                        this.saveAttributes = null;
                    }
                }
            }
            else
            {
                this.outFile = null;
                this.nonFatal = null;
                this.saveAttributes = null;
            }
        }

        /// <summary>
        /// Assembles the source code.
        /// </summary>
        public override void Assemble()
        {
            string write = name + " ";
            AssembleExpression.AssembleIfRequired(this.inFile);
            if (this.nonFatal != null && this.nonFatal.GetBooleanValue())
                write += "/nonfatal ";
            if (this.saveAttributes != null && this.saveAttributes.GetBooleanValue())
                write += "/a ";
            if (this.outFile != null)
            {
                Expression varOrOutFile = AssembleExpression.GetRegisterOrExpression(this.outFile);
                write += "\"/oname=" + varOrOutFile.ToString(true) + "\" ";
                varOrOutFile.SetInUse(false);
            }

            if (this.nonFatal != null)
                AssembleExpression.AssembleIfRequired(this.nonFatal);
            if (this.saveAttributes != null)
                AssembleExpression.AssembleIfRequired(this.saveAttributes);
            ScriptParser.WriteLine(write + this.inFile);
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