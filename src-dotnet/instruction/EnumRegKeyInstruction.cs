/*
 * EnumRegKeyInstruction.java
 */
using Java.Io;
using Java.Util;
using Nsl;

namespace Nsl.Instruction
{
    /// <remarks>@authorStuart</remarks>
    public class EnumRegKeyInstruction : AssembleExpression
    {
        public static readonly string name = "EnumRegKey";
        private readonly Expression rootKey;
        private readonly Expression subKey;
        private readonly Expression index;
        public EnumRegKeyInstruction(int returns)
        {
            if (!SectionInfo.In() && !FunctionInfo.In())
                throw new NslContextException(EnumSet.Of(NslContext.Section, NslContext.Function), name);
            if (returns != 1)
                throw new NslReturnValueException(name, 1);
            List<Expression> paramsList = Expression.MatchList();
            int paramsCount = paramsList.Count;
            if (paramsCount != 3)
                throw new NslArgumentException(name, 3);
            this.rootKey = paramsList[0];
            if (!ExpressionType.IsString(this.rootKey))
                throw new NslArgumentException(name, 1, ExpressionType.String);
            this.subKey = paramsList[1];
            this.index = paramsList[2];
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
            AssembleExpression.AssembleIfRequired(this.rootKey);
            Expression varOrSubKey = AssembleExpression.GetRegisterOrExpression(this.subKey);
            Expression varOrIndex = AssembleExpression.GetRegisterOrExpression(this.index);
            ScriptParser.WriteLine(name + " " + var + " " + this.rootKey + " " + varOrSubKey + " " + varOrIndex);
            varOrSubKey.SetInUse(false);
            varOrIndex.SetInUse(false);
        }
    }
}