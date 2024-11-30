/*
 * GetDlgItemInstruction.java
 */
using Java.Io;
using Java.Util;
using Nsl;

namespace Nsl.Instruction
{
    /// <remarks>@authorStuart</remarks>
    public class GetDlgItemInstruction : AssembleExpression
    {
        public static readonly string name = "GetDlgItem";
        private readonly Expression hWnd;
        private readonly Expression itemId;
        public GetDlgItemInstruction(int returns)
        {
            if (PageExInfo.In())
                throw new NslContextException(EnumSet.Of(NslContext.Section, NslContext.Function, NslContext.Global), name);
            if (returns != 1)
                throw new NslReturnValueException(name, 1);
            List<Expression> paramsList = Expression.MatchList();
            if (paramsList.Count != 2)
                throw new NslArgumentException(name, 2);
            this.hWnd = paramsList[0];
            this.itemId = paramsList[1];
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
            Expression varOrHWnd = AssembleExpression.GetRegisterOrExpression(this.hWnd);
            Expression varOrItemId = AssembleExpression.GetRegisterOrExpression(this.itemId);
            ScriptParser.WriteLine(name + " " + var + " " + varOrHWnd + " " + varOrItemId);
            varOrHWnd.SetInUse(false);
        }
    }
}