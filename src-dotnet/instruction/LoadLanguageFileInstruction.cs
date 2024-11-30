/*
 * LoadLanguageFileInstruction.java
 */
using Java.Io;
using Java.Util;
using Nsl;

namespace Nsl.Instruction
{
    /// <remarks>@authorStuart</remarks>
    public class LoadLanguageFileInstruction : AssembleExpression
    {
        public static readonly string name = "LoadLanguageFile";
        private readonly Expression file;
        public LoadLanguageFileInstruction(int returns)
        {
            if (!ScriptParser.InGlobalContext())
                throw new NslContextException(EnumSet.Of(NslContext.Global), name);
            if (returns > 0)
                throw new NslReturnValueException(name);
            List<Expression> paramsList = Expression.MatchList();
            if (paramsList.Count != 1)
                throw new NslArgumentException(name, 1);
            this.file = paramsList[0];
            if (!ExpressionType.IsString(this.file))
                throw new NslArgumentException(name, 1, ExpressionType.String);
        }

        /// <summary>
        /// Assembles the source code.
        /// </summary>
        public override void Assemble()
        {
            AssembleExpression.AssembleIfRequired(this.file);
            ScriptParser.WriteLine(name + " " + this.file);
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