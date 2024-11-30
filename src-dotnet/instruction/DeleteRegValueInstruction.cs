/*
 * DeleteRegValueInstruction.java
 */
using Java.Io;
using Java.Util;
using Nsl;

namespace Nsl.Instruction
{
    /// <remarks>@authorStuart</remarks>
    public class DeleteRegValueInstruction : AssembleExpression
    {
        public static readonly string name = "DeleteRegValue";
        private readonly Expression rootKey;
        private readonly Expression subKey;
        private readonly Expression valueName;
        public DeleteRegValueInstruction(int returns)
        {
            if (!SectionInfo.In() && !FunctionInfo.In())
                throw new NslContextException(EnumSet.Of(NslContext.Section, NslContext.Function), name);
            if (returns > 0)
                throw new NslReturnValueException(name);
            List<Expression> paramsList = Expression.MatchList();
            int paramsCount = paramsList.Count;
            if (paramsCount != 3)
                throw new NslArgumentException(name, 3);
            this.rootKey = paramsList[0];
            if (!ExpressionType.IsString(this.rootKey))
                throw new NslArgumentException(name, 1, ExpressionType.String);
            this.subKey = paramsList[1];
            this.valueName = paramsList[2];
        }

        /// <summary>
        /// Assembles the source code.
        /// </summary>
        public override void Assemble()
        {
            AssembleExpression.AssembleIfRequired(this.rootKey);
            Expression varOrSubKey = AssembleExpression.GetRegisterOrExpression(this.subKey);
            Expression varOrValueName = AssembleExpression.GetRegisterOrExpression(this.valueName);
            ScriptParser.WriteLine(name + " " + this.rootKey + " " + varOrSubKey + " " + varOrValueName);
            varOrSubKey.SetInUse(false);
            varOrValueName.SetInUse(false);
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