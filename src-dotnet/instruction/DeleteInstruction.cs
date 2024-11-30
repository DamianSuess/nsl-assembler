/*
 * DeleteInstruction.java
 */
using Java.Io;
using Java.Util;
using Nsl;

namespace Nsl.Instruction
{
    /// <remarks>@authorStuart</remarks>
    public class DeleteInstruction : AssembleExpression
    {
        public static readonly string name = "Delete";
        private readonly Expression file;
        private readonly Expression rebootOK;
        public DeleteInstruction(int returns)
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
            if (paramsCount > 1)
            {
                this.rebootOK = paramsList[1];
                if (!ExpressionType.IsBoolean(this.rebootOK))
                    throw new NslArgumentException(name, 2, ExpressionType.Boolean);
            }
            else
                this.rebootOK = null;
        }

        /// <summary>
        /// Assembles the source code.
        /// </summary>
        public override void Assemble()
        {
            Expression varOrFile = AssembleExpression.GetRegisterOrExpression(this.file);
            string write = name + " ";
            if (this.rebootOK != null)
            {
                AssembleExpression.AssembleIfRequired(this.rebootOK);
                if (this.rebootOK.GetBooleanValue() == true)
                    write += "/REBOOTOK ";
            }

            ScriptParser.WriteLine(write + varOrFile);
            varOrFile.SetInUse(false);
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