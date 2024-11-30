/*
 * DeleteRegKeyInstruction.java
 */
using Java.Io;
using Java.Util;
using Nsl;

namespace Nsl.Instruction
{
    /// <remarks>@authorStuart</remarks>
    public class DeleteRegKeyInstruction : AssembleExpression
    {
        public static readonly string name = "DeleteRegKey";
        private readonly Expression rootKey;
        private readonly Expression subKey;
        private readonly Expression ifEmpty;
        public DeleteRegKeyInstruction(int returns)
        {
            if (!SectionInfo.In() && !FunctionInfo.In())
                throw new NslContextException(EnumSet.Of(NslContext.Section, NslContext.Function), name);
            if (returns > 0)
                throw new NslReturnValueException(name);
            List<Expression> paramsList = Expression.MatchList();
            int paramsCount = paramsList.Count;
            if (paramsCount < 2 || paramsCount > 3)
                throw new NslArgumentException(name, 2, 3);
            this.rootKey = paramsList[0];
            if (!ExpressionType.IsString(this.rootKey))
                throw new NslArgumentException(name, 1, ExpressionType.String);
            this.subKey = paramsList[1];
            if (paramsCount > 2)
            {
                this.ifEmpty = paramsList[2];
                if (!ExpressionType.IsBoolean(this.ifEmpty))
                    throw new NslArgumentException(name, 3, ExpressionType.Boolean);
            }
            else
                this.ifEmpty = null;
        }

        /// <summary>
        /// Assembles the source code.
        /// </summary>
        public override void Assemble()
        {
            AssembleExpression.AssembleIfRequired(this.rootKey);
            Expression varOrSubKey = AssembleExpression.GetRegisterOrExpression(this.subKey);
            AssembleExpression.AssembleIfRequired(this.ifEmpty);
            ScriptParser.WriteLine(name + " " + (this.ifEmpty != null && this.ifEmpty.GetBooleanValue() == true ? "/ifempty " : "") + this.rootKey + " " + varOrSubKey);
            varOrSubKey.SetInUse(false);
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