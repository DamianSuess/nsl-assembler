/*
 * ChangeUIInstruction.java
 */
using Java.Io;
using Java.Util;
using Nsl;

namespace Nsl.Instruction
{
    /// <remarks>@authorStuart</remarks>
    public class ChangeUIInstruction : AssembleExpression
    {
        public static readonly string name = "ChangeUI";
        private readonly Expression dialog;
        private readonly Expression uiFile;
        public ChangeUIInstruction(int returns)
        {
            if (!ScriptParser.InGlobalContext())
                throw new NslContextException(EnumSet.Of(NslContext.Global), name);
            if (returns > 0)
                throw new NslReturnValueException(name);
            List<Expression> paramsList = Expression.MatchList();
            int paramsCount = paramsList.Count;
            if (paramsCount != 1)
                throw new NslArgumentException(name, 1);
            this.dialog = paramsList[0];
            if (!ExpressionType.IsString(this.dialog))
                throw new NslArgumentException(name, 1, ExpressionType.String);
            this.uiFile = paramsList[1];
            if (!ExpressionType.IsString(this.uiFile))
                throw new NslArgumentException(name, 2, ExpressionType.String);
        }

        /// <summary>
        /// Assembles the source code.
        /// </summary>
        public override void Assemble()
        {
            AssembleExpression.AssembleIfRequired(this.dialog);
            AssembleExpression.AssembleIfRequired(this.uiFile);
            ScriptParser.WriteLine(name + " " + this.dialog + " " + this.uiFile);
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