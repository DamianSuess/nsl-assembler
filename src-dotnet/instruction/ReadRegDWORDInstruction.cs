/*
 * ReadRegDWORDInstruction.java
 */
using Java.Io;
using Java.Util;
using Nsl;

namespace Nsl.Instruction
{
    /// <remarks>@authorStuart</remarks>
    public class ReadRegDWORDInstruction : AssembleExpression
    {
        public static readonly string name = "ReadRegDWORD";
        private readonly Expression rootKey;
        private readonly Expression subKey;
        private readonly Expression keyName;
        public ReadRegDWORDInstruction(int returns)
        {
            if (PageExInfo.In())
                throw new NslContextException(EnumSet.Of(NslContext.Section, NslContext.Function, NslContext.Global), name);
            if (returns != 1)
                throw new NslReturnValueException(name, 1);
            List<Expression> paramsList = Expression.MatchList();
            if (paramsList.Count != 3)
                throw new NslArgumentException(name, 3);
            this.rootKey = paramsList[0];
            if (!ExpressionType.IsString(this.rootKey))
                throw new NslArgumentException(name, 1, ExpressionType.String);
            this.subKey = paramsList[1];
            this.keyName = paramsList[2];
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
            Expression varOrKeyName = AssembleExpression.GetRegisterOrExpression(this.keyName);
            ScriptParser.WriteLine(name + " " + var + " " + this.rootKey + " " + varOrSubKey + " " + varOrKeyName);
            varOrSubKey.SetInUse(false);
            varOrKeyName.SetInUse(false);
        }
    }
}