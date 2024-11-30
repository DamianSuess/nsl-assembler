/*
 * InstallDirInstruction.java
 */
using Java.Io;
using Java.Util;
using Nsl;

namespace Nsl.Instruction
{
    /// <remarks>@authorStuart</remarks>
    public class InstallDirInstruction : AssembleExpression
    {
        public static readonly string name = "InstallDir";
        private readonly Expression installDir;
        public InstallDirInstruction(int returns)
        {
            if (!ScriptParser.InGlobalContext())
                throw new NslContextException(EnumSet.Of(NslContext.Global), name);
            if (returns > 0)
                throw new NslReturnValueException(name);
            List<Expression> paramsList = Expression.MatchList();
            if (paramsList.Count != 1)
                throw new NslArgumentException(name, 1);
            this.installDir = paramsList[0];
        }

        /// <summary>
        /// Assembles the source code.
        /// </summary>
        public override void Assemble()
        {
            Expression varOrInstallDir = AssembleExpression.GetRegisterOrExpression(this.installDir);
            ScriptParser.WriteLine(name + " " + varOrInstallDir);
            varOrInstallDir.SetInUse(false);
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