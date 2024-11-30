/*
 * SectionSetFlagsInstruction.java
 */
using Java.Io;
using Java.Util;
using Nsl;

namespace Nsl.Instruction
{
    /// <remarks>@authorStuart</remarks>
    public class SectionSetInstTypesInstruction : AssembleExpression
    {
        public static readonly string name = "SectionSetFlags";
        private readonly Expression index;
        private readonly Expression instTypes;
        public SectionSetInstTypesInstruction(int returns)
        {
            if (!SectionInfo.In() && !FunctionInfo.In())
                throw new NslContextException(EnumSet.Of(NslContext.Section, NslContext.Function), name);
            if (returns > 0)
                throw new NslReturnValueException(name);
            List<Expression> paramsList = Expression.MatchList();
            if (paramsList.Count != 2)
                throw new NslArgumentException(name, 2);
            this.index = paramsList[0];
            this.instTypes = paramsList[1];
            if (!ExpressionType.IsInteger(this.instTypes))
                throw new NslArgumentException(name, 2, ExpressionType.Integer);
        }

        /// <summary>
        /// Assembles the source code.
        /// </summary>
        public override void Assemble()
        {
            Expression varOrIndex = AssembleExpression.GetRegisterOrExpression(this.index);
            AssembleExpression.AssembleIfRequired(this.instTypes);
            ScriptParser.WriteLine(name + " " + varOrIndex + " " + this.instTypes);
            varOrIndex.SetInUse(false);
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