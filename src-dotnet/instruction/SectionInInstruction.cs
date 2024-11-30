/*
 * SectionInInstruction.java
 */
using Java.Io;
using Java.Util;
using Nsl;

namespace Nsl.Instruction
{
    /// <remarks>@authorStuart</remarks>
    public class SectionInInstruction : AssembleExpression
    {
        public static readonly string name = "SectionIn";
        private readonly List<Expression> paramsList;
        public SectionInInstruction(int returns)
        {
            if (!SectionInfo.In())
                throw new NslContextException(EnumSet.Of(NslContext.Section), name);
            if (returns > 0)
                throw new NslReturnValueException(name);
            this.paramsList = Expression.MatchList();
            int paramsCount = this.paramsList.Count;
            if (paramsCount == 0)
                throw new NslArgumentException(name, 1, 999);
            for (int i = 0; i < paramsCount; i++)
            {
                Expression param = this.paramsList[i];
                if (!ExpressionType.IsInteger(param) && !ExpressionType.IsBoolean(param))
                    throw new NslArgumentException(name, i + 1, ExpressionType.Integer, ExpressionType.Boolean);
            }
        }

        /// <summary>
        /// Assembles the source code.
        /// </summary>
        public override void Assemble()
        {
            string write = name;
            foreach (Expression param in this.paramsList)
            {
                AssembleExpression.AssembleIfRequired(param);
                if (param.GetType().Equals(ExpressionType.Boolean) && param.GetBooleanValue() == true)
                    write += " RO";
                else
                    write += " " + param;
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