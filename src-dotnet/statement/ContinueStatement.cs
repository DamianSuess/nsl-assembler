/*
 * ContinueStatement.java
 */
using Java.Io;
using Java.Util;
using Nsl;

namespace Nsl.Statement
{
    /// <summary>
    ///  * @author Stuart
    /// </summary>
    public class ContinueStatement : Statement
    {
        /// <summary>
        /// Class constructor.
        /// </summary>
        public ContinueStatement()
        {
            if (!SectionInfo.In() && !FunctionInfo.In())
                throw new NslContextException(EnumSet.Of(NslContext.Section, NslContext.Function), "continue");
            ScriptParser.tokenizer.MatchEolOrDie();
            if (CodeInfo.GetCurrent().GetContinueLabel() == null)
                throw new NslException("The \"continue\" statement cannot be used here", true);
        }

        /// <summary>
        /// Class constructor.
        /// </summary>
        public override void Assemble()
        {
            CodeInfo.GetCurrent().GetContinueLabel().SetNotUsed(false);
            ScriptParser.WriteLine("Goto " + CodeInfo.GetCurrent().GetContinueLabel());
        }
    }
}