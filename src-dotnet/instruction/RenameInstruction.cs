/*
 * RenameInstruction.java
 */
using Java.Io;
using Java.Util;
using Nsl;

namespace Nsl.Instruction
{
    /// <remarks>@authorStuart</remarks>
    public class RenameInstruction : AssembleExpression
    {
        public static readonly string name = "Rename";
        private readonly Expression sourceFile;
        private readonly Expression destFile;
        private readonly Expression rebootOK;
        public RenameInstruction(int returns)
        {
            if (!SectionInfo.In() && !FunctionInfo.In())
                throw new NslContextException(EnumSet.Of(NslContext.Section, NslContext.Function), name);
            if (returns > 0)
                throw new NslReturnValueException(name);
            List<Expression> paramsList = Expression.MatchList();
            int paramsCount = paramsList.Count;
            if (paramsCount < 1 || paramsCount > 3)
                throw new NslArgumentException(name, 1, 3);
            this.sourceFile = paramsList[0];
            if (paramsCount > 1)
            {
                this.destFile = paramsList[1];
                if (paramsCount > 2)
                {
                    this.rebootOK = paramsList[2];
                    if (!ExpressionType.IsBoolean(this.rebootOK))
                        throw new NslArgumentException(name, 3, ExpressionType.Boolean);
                }
                else
                    this.rebootOK = null;
            }
            else
            {
                this.destFile = null;
                this.rebootOK = null;
            }
        }

        /// <summary>
        /// Assembles the source code.
        /// </summary>
        public override void Assemble()
        {
            Expression varOrSourceFile = AssembleExpression.GetRegisterOrExpression(this.sourceFile);
            Expression varOrDestFile = AssembleExpression.GetRegisterOrExpression(this.destFile);
            string write = name + " ";
            if (this.rebootOK != null)
            {
                AssembleExpression.AssembleIfRequired(this.rebootOK);
                if (this.rebootOK.GetBooleanValue() == true)
                    write += "/REBOOTOK ";
            }

            ScriptParser.WriteLine(write + varOrSourceFile + " " + varOrDestFile);
            varOrSourceFile.SetInUse(false);
            varOrDestFile.SetInUse(false);
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