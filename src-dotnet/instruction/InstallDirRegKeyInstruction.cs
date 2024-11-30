/*
 * ReadRegStrInstruction.java
 */
using Java.Io;
using Java.Util;
using Nsl;

namespace Nsl.Instruction
{
    /// <remarks>@authorStuart</remarks>
    public class InstallDirRegKeyInstruction : AssembleExpression
    {
        public static readonly string name = "InstallDirRegKey";
        private readonly Expression rootKey;
        private readonly Expression subKey;
        private readonly Expression keyName;
        public InstallDirRegKeyInstruction(int returns)
        {
            if (!ScriptParser.InGlobalContext())
                throw new NslContextException(EnumSet.Of(NslContext.Global), name);
            if (returns > 0)
                throw new NslReturnValueException(name);
            List<Expression> paramsList = Expression.MatchList();
            if (paramsList.Count != 3)
                throw new NslArgumentException(name, 3);
            this.rootKey = paramsList[0];
            if (!ExpressionType.IsString(this.rootKey))
                throw new NslArgumentException(name, 1, ExpressionType.String);
            this.subKey = paramsList[1];
            if (!ExpressionType.IsString(this.subKey))
                throw new NslArgumentException(name, 2, ExpressionType.String);
            this.keyName = paramsList[2];
            if (!ExpressionType.IsString(this.keyName))
                throw new NslArgumentException(name, 3, ExpressionType.String);
        }

        /// <summary>
        /// Assembles the source code.
        /// </summary>
        public override void Assemble()
        {
            AssembleExpression.AssembleIfRequired(this.rootKey);
            Expression varOrSubKey = AssembleExpression.GetRegisterOrExpression(this.subKey);
            Expression varOrKeyName = AssembleExpression.GetRegisterOrExpression(this.keyName);
            ScriptParser.WriteLine(name + " " + this.rootKey + " " + varOrSubKey + " " + varOrKeyName);
            varOrSubKey.SetInUse(false);
            varOrKeyName.SetInUse(false);
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