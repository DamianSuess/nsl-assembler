/*
 * InstProgressFlagsInstruction.java
 */
using Java.Io;
using Java.Util;
using Nsl;

namespace Nsl.Instruction
{
    /// <remarks>@authorStuart</remarks>
    public class InstProgressFlagsInstruction : AssembleExpression
    {
        public static readonly string name = "InstProgressFlags";
        private readonly Expression value1;
        private readonly Expression value2;
        public InstProgressFlagsInstruction(int returns)
        {
            if (!ScriptParser.InGlobalContext())
                throw new NslContextException(EnumSet.Of(NslContext.Global), name);
            if (returns > 0)
                throw new NslReturnValueException(name);
            List<Expression> paramsList = Expression.MatchList();
            int paramsCount = paramsList.Count;
            if (paramsCount < 1 || paramsCount > 2)
                throw new NslArgumentException(name, 1, 2);
            this.value1 = paramsList[0];
            if (!ExpressionType.IsString(this.value1))
                throw new NslArgumentException(name, 1, ExpressionType.String);
            if (paramsCount > 1)
            {
                this.value2 = paramsList[1];
                if (!ExpressionType.IsString(this.value2))
                    throw new NslArgumentException(name, 2, ExpressionType.String);
            }
            else
                this.value2 = null;
        }

        /// <summary>
        /// Assembles the source code.
        /// </summary>
        public override void Assemble()
        {
            AssembleExpression.AssembleIfRequired(this.value1);
            if (this.value2 == null)
            {
                ScriptParser.WriteLine(name + " " + this.value1);
            }
            else
            {
                AssembleExpression.AssembleIfRequired(this.value2);
                ScriptParser.WriteLine(name + " " + this.value1 + " " + this.value2);
            }
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