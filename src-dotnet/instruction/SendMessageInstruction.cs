/*
 * SendMessageInstruction.java
 */
using Java.Io;
using Java.Util;
using Nsl;

namespace Nsl.Instruction
{
    /// <remarks>@authorStuart</remarks>
    public class SendMessageInstruction : AssembleExpression
    {
        public static readonly string name = "SendMessage";
        private readonly Expression hWnd;
        private readonly Expression msg;
        private readonly Expression wParam;
        private readonly Expression lParam;
        private readonly Expression timeout;
        public SendMessageInstruction(int returns)
        {
            if (!SectionInfo.In() && !FunctionInfo.In())
                throw new NslContextException(EnumSet.Of(NslContext.Section, NslContext.Function), name);
            if (returns > 1)
                throw new NslReturnValueException(name, 0, 1);
            List<Expression> paramsList = Expression.MatchList();
            int paramsCount = paramsList.Count;
            if (paramsCount < 4 || paramsCount > 5)
                throw new NslArgumentException(name, 1);
            this.hWnd = paramsList[0];
            this.msg = paramsList[1];
            this.wParam = paramsList[2];
            this.lParam = paramsList[3];
            if (paramsCount > 4)
            {
                this.timeout = paramsList[4];
                if (!ExpressionType.IsInteger(this.timeout))
                    throw new NslArgumentException(name, 5, ExpressionType.Integer);
            }
            else
                this.timeout = null;
        }

        public override void Assemble()
        {
            Expression varOrHWnd = AssembleExpression.GetRegisterOrExpression(this.hWnd);
            Expression varOrMsg = AssembleExpression.GetRegisterOrExpression(this.msg);
            Expression varOrWParam = AssembleExpression.GetRegisterOrExpression(this.wParam);
            Expression varOrLParam = AssembleExpression.GetRegisterOrExpression(this.lParam);
            if (timeout != null)
            {
                AssembleExpression.AssembleIfRequired(this.timeout);
                ScriptParser.WriteLine(name + " " + varOrHWnd + " " + varOrMsg + " " + varOrWParam + " " + varOrLParam + " /TIMEOUT=" + this.timeout);
            }
            else
            {
                ScriptParser.WriteLine(name + " " + varOrHWnd + " " + varOrMsg + " " + varOrWParam + " " + varOrLParam);
            }

            varOrHWnd.SetInUse(false);
            varOrMsg.SetInUse(false);
            varOrWParam.SetInUse(false);
            varOrLParam.SetInUse(false);
        }

        public override void Assemble(Register var)
        {
            Expression varOrHWnd = AssembleExpression.GetRegisterOrExpression(this.hWnd);
            Expression varOrMsg = AssembleExpression.GetRegisterOrExpression(this.msg);
            Expression varOrWParam = AssembleExpression.GetRegisterOrExpression(this.wParam);
            Expression varOrLParam = AssembleExpression.GetRegisterOrExpression(this.lParam);
            if (timeout != null)
            {
                AssembleExpression.AssembleIfRequired(this.timeout);
                ScriptParser.WriteLine(name + " " + varOrHWnd + " " + varOrMsg + " " + varOrWParam + " " + varOrLParam + " " + var + " /TIMEOUT=" + this.timeout);
            }
            else
            {
                ScriptParser.WriteLine(name + " " + varOrHWnd + " " + varOrMsg + " " + varOrWParam + " " + varOrLParam + " " + var);
            }

            varOrHWnd.SetInUse(false);
            varOrMsg.SetInUse(false);
            varOrWParam.SetInUse(false);
            varOrLParam.SetInUse(false);
        }
    }
}