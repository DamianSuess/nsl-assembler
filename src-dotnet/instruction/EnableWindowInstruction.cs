/*
 * EnableWindowInstruction.java
 */
using Java.Io;
using Java.Util;
using Nsl;

namespace Nsl.Instruction
{
    /// <remarks>@authorStuart</remarks>
    public class EnableWindowInstruction : AssembleExpression
    {
        public static readonly string name = "EnableWindow";
        private readonly Expression hWnd;
        private readonly Expression enabled;
        public EnableWindowInstruction(int returns)
        {
            if (!SectionInfo.In() && !FunctionInfo.In())
                throw new NslContextException(EnumSet.Of(NslContext.Section, NslContext.Function), name);
            if (returns > 0)
                throw new NslReturnValueException(name);
            List<Expression> paramsList = Expression.MatchList();
            if (paramsList.Count != 2)
                throw new NslArgumentException(name, 2);
            this.hWnd = paramsList[0];
            this.enabled = paramsList[1];
            if (!ExpressionType.IsInteger(this.enabled))
                throw new NslArgumentException(name, 2, ExpressionType.Integer);
        }

        /// <summary>
        /// Assembles the source code.
        /// </summary>
        public override void Assemble()
        {
            Expression varOrHWnd = AssembleExpression.GetRegisterOrExpression(this.hWnd);
            AssembleExpression.AssembleIfRequired(this.enabled);
            ScriptParser.WriteLine(name + " " + varOrHWnd + " " + this.enabled);
            varOrHWnd.SetInUse(false);
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