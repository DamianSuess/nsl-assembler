/*
 * VIAddVersionKeyInstruction.java
 */
using Java.Io;
using Java.Util;
using Nsl;

namespace Nsl.Instruction
{
    /// <remarks>@authorStuart</remarks>
    public class VIAddVersionKeyInstruction : AssembleExpression
    {
        public static readonly string name = "VIAddVersionKey";
        private readonly Expression keyName;
        private readonly Expression value;
        private readonly Expression langId;
        public VIAddVersionKeyInstruction(int returns)
        {
            if (!ScriptParser.InGlobalContext())
                throw new NslContextException(EnumSet.Of(NslContext.Global), name);
            if (returns > 0)
                throw new NslReturnValueException(name);
            List<Expression> paramsList = Expression.MatchList();
            int paramsCount = paramsList.Count;
            if (paramsCount < 2 || paramsCount > 3)
                throw new NslArgumentException(name, 1);
            this.keyName = paramsList[0];
            if (!ExpressionType.IsString(this.keyName))
                throw new NslArgumentException(name, 1, ExpressionType.String);
            this.value = paramsList[1];
            if (!ExpressionType.IsString(this.value))
                throw new NslArgumentException(name, 2, ExpressionType.String);
            if (paramsCount > 1)
            {
                this.langId = paramsList[2];
                if (!ExpressionType.IsInteger(this.langId))
                    throw new NslArgumentException(name, 3, ExpressionType.Integer);
            }
            else
            {
                this.langId = null;
            }
        }

        /// <summary>
        /// Assembles the source code.
        /// </summary>
        public override void Assemble()
        {
            AssembleExpression.AssembleIfRequired(this.keyName);
            AssembleExpression.AssembleIfRequired(this.value);
            string write = name + " " + this.keyName + " " + this.value;
            if (this.langId != null)
            {
                AssembleExpression.AssembleIfRequired(this.langId);
                write += " " + this.langId;
            }

            ScriptParser.WriteLine(write);
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