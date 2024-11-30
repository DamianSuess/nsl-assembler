/*
 * SetCompressorInstruction.java
 */
using Java.Io;
using Java.Util;
using Nsl;

namespace Nsl.Instruction
{
    /// <remarks>@authorStuart</remarks>
    public class SetCompressorInstruction : AssembleExpression
    {
        public static readonly string name = "SetCompressor";
        private readonly Expression value;
        private readonly Expression solidFlag;
        private readonly Expression finalFlag;
        public SetCompressorInstruction(int returns)
        {
            if (!ScriptParser.InGlobalContext())
                throw new NslContextException(EnumSet.Of(NslContext.Global), name);
            if (returns > 0)
                throw new NslReturnValueException(name);
            List<Expression> paramsList = Expression.MatchList();
            int paramsCount = paramsList.Count;
            if (paramsCount < 1 || paramsCount > 3)
                throw new NslArgumentException(name, 1, 3);
            this.value = paramsList[0];
            if (!ExpressionType.IsString(this.value))
                throw new NslArgumentException(name, 1, ExpressionType.String);
            if (paramsCount > 1)
            {
                this.solidFlag = paramsList[1];
                if (!ExpressionType.IsBoolean(this.solidFlag))
                    throw new NslArgumentException(name, 2, ExpressionType.Boolean);
            }
            else
                this.solidFlag = null;
            if (paramsCount > 2)
            {
                this.finalFlag = paramsList[2];
                if (!ExpressionType.IsBoolean(this.finalFlag))
                    throw new NslArgumentException(name, 3, ExpressionType.Boolean);
            }
            else
                this.finalFlag = null;
        }

        /// <summary>
        /// Assembles the source code.
        /// </summary>
        public override void Assemble()
        {
            AssembleExpression.AssembleIfRequired(this.value);
            string write = name;
            if (this.solidFlag != null)
            {
                AssembleExpression.AssembleIfRequired(this.solidFlag);
                if (this.solidFlag.GetBooleanValue() == true)
                    write += " /SOLID";
            }

            if (this.finalFlag != null)
            {
                AssembleExpression.AssembleIfRequired(this.finalFlag);
                if (this.finalFlag.GetBooleanValue() == true)
                    write += " /FINAL";
            }

            ScriptParser.WriteLine(write + " " + this.value);
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