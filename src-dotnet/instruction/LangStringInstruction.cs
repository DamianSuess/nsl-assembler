/*
 * LangStringInstruction.java
 */
using Java.Io;
using Java.Util;
using Nsl;

namespace Nsl.Instruction
{
    /// <remarks>@authorStuart</remarks>
    public class LangStringInstruction : AssembleExpression
    {
        public static readonly string name = "LangString";
        private readonly Expression langString;
        private readonly Expression langId;
        private readonly Expression text;
        public LangStringInstruction(int returns)
        {
            if (!ScriptParser.InGlobalContext())
                throw new NslContextException(EnumSet.Of(NslContext.Global), name);
            if (returns > 0)
                throw new NslReturnValueException(name);
            List<Expression> paramsList = Expression.MatchList();
            if (paramsList.Count != 3)
                throw new NslArgumentException(name, 3);
            this.langString = paramsList[0];
            if (!ExpressionType.IsString(this.langString))
                throw new NslArgumentException(name, 1, ExpressionType.String);
            this.langId = paramsList[1];
            if (!ExpressionType.IsInteger(this.langId))
                throw new NslArgumentException(name, 2, ExpressionType.Integer);
            this.text = paramsList[2];
            if (!ExpressionType.IsString(this.text))
                throw new NslArgumentException(name, 3, ExpressionType.String);
        }

        /// <summary>
        /// Assembles the source code.
        /// </summary>
        public override void Assemble()
        {
            AssembleExpression.AssembleIfRequired(this.langString);
            AssembleExpression.AssembleIfRequired(this.langId);
            AssembleExpression.AssembleIfRequired(this.text);
            ScriptParser.WriteLine(name + " " + this.langString + " " + this.langId + " " + this.text);
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