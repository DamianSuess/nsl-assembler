/*
 * GetTempFileNameInstruction.java
 */
using Java.Io;
using Java.Util;
using Nsl;

namespace Nsl.Instruction
{
    /// <remarks>@authorStuart</remarks>
    public class GetTempFileNameInstruction : AssembleExpression
    {
        public static readonly string name = "GetTempFileName";
        private readonly Expression baseDir;
        public GetTempFileNameInstruction(int returns)
        {
            if (PageExInfo.In())
                throw new NslContextException(EnumSet.Of(NslContext.Section, NslContext.Function, NslContext.Global), name);
            if (returns != 1)
                throw new NslReturnValueException(name, 1);
            List<Expression> paramsList = Expression.MatchList();
            int paramsCount = paramsList.Count;
            if (paramsCount > 1)
                throw new NslArgumentException(name, 0, 1);
            if (paramsCount > 1)
                this.baseDir = paramsList[0];
            else
                this.baseDir = null;
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
            if (this.baseDir == null)
            {
                ScriptParser.WriteLine(name + " " + var);
            }
            else
            {
                Expression varOrBaseDir = AssembleExpression.GetRegisterOrExpression(this.baseDir);
                ScriptParser.WriteLine(name + " " + var + " " + varOrBaseDir);
                varOrBaseDir.SetInUse(false);
            }
        }
    }
}