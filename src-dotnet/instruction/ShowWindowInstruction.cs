/*
 * ShowWindowInstruction.java
 */
using Java.Io;
using Java.Util;
using Nsl;

namespace Nsl.Instruction
{
    /// <remarks>@authorStuart</remarks>
    public class ShowWindowInstruction : AssembleExpression
    {
        public static readonly string name = "ShowWindow";
        private readonly Expression hWnd;
        private readonly Expression value;
        public ShowWindowInstruction(int returns)
        {
            if (!SectionInfo.In() && !FunctionInfo.In())
                throw new NslContextException(EnumSet.Of(NslContext.Section, NslContext.Function), name);
            if (returns > 0)
                throw new NslReturnValueException(name);
            List<Expression> paramsList = Expression.MatchList();
            if (paramsList.Count != 2)
                throw new NslArgumentException(name, 2);
            this.hWnd = paramsList[0];
            this.value = paramsList[1];
            if (!ExpressionType.IsInteger(this.value))
                throw new NslArgumentException(name, 2, ExpressionType.Integer);
        }

        /// <summary>
        /// Assembles the source code.
        /// </summary>
        public override void Assemble()
        {
            Expression varOrHWnd = AssembleExpression.GetRegisterOrExpression(this.hWnd);
            AssembleExpression.AssembleIfRequired(this.value);
            ScriptParser.WriteLine(name + " " + varOrHWnd + " " + this.value);
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