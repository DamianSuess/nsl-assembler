/*
 * UninstallSubCaptionInstruction.java
 */
using Java.Io;
using Java.Util;
using Nsl;

namespace Nsl.Instruction
{
    /// <remarks>@authorStuart</remarks>
    public class UninstallSubCaptionInstruction : AssembleExpression
    {
        public static readonly string name = "UninstallSubCaption";
        private readonly Expression pageNumber;
        private readonly Expression text;
        public UninstallSubCaptionInstruction(int returns)
        {
            if (!ScriptParser.InGlobalContext() && !PageExInfo.In())
                throw new NslContextException(EnumSet.Of(NslContext.Global, NslContext.PageEx), name);
            if (returns > 0)
                throw new NslReturnValueException(name);
            List<Expression> paramsList = Expression.MatchList();
            int paramsCount = paramsList.Count;
            if (paramsCount != 2)
                throw new NslArgumentException(name, 2);
            this.pageNumber = paramsList[0];
            if (!ExpressionType.IsInteger(this.pageNumber))
                throw new NslArgumentException(name, 1, ExpressionType.Integer);
            this.text = paramsList[1];
        }

        /// <summary>
        /// Assembles the source code.
        /// </summary>
        public override void Assemble()
        {
            AssembleExpression.AssembleIfRequired(this.pageNumber);
            Expression varOrText = AssembleExpression.GetRegisterOrExpression(this.text);
            ScriptParser.WriteLine(name + " " + this.pageNumber + " " + varOrText);
            varOrText.SetInUse(false);
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