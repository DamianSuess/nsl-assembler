/*
 * LicenseBkColorInstruction.java
 */
using Java.Io;
using Java.Util;
using Nsl;

namespace Nsl.Instruction
{
    /// <remarks>@authorStuart</remarks>
    public class LicenseBkColorInstruction : AssembleExpression
    {
        public static readonly string name = "LicenseBkColor";
        private readonly Expression value;
        public LicenseBkColorInstruction(int returns)
        {
            if (!ScriptParser.InGlobalContext() && !PageExInfo.In())
                throw new NslContextException(EnumSet.Of(NslContext.Global, NslContext.PageEx), name);
            if (returns > 0)
                throw new NslReturnValueException(name);
            List<Expression> paramsList = Expression.MatchList();
            if (paramsList.Count != 1)
                throw new NslArgumentException(name, 1);
            this.value = paramsList[0];
            if (!ExpressionType.IsBoolean(this.value) && !ExpressionType.IsString(this.value))
                throw new NslArgumentException(name, 1, ExpressionType.Boolean, ExpressionType.String);
        }

        /// <summary>
        /// Assembles the source code.
        /// </summary>
        public override void Assemble()
        {
            AssembleExpression.AssembleIfRequired(this.value);
            if (this.value.GetType().Equals(ExpressionType.Boolean))
            {
                if (this.value.GetBooleanValue() == true)
                    ScriptParser.WriteLine(name + " /gray");
                else
                    ScriptParser.WriteLine(name + " /windows");
            }
            else
            {
                ScriptParser.WriteLine(name + " " + this.value);
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