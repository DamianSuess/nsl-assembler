/*
 * BreakStatement.java
 */
using Java.Io;
using Java.Util;
using Nsl;

namespace Nsl.Statement
{
    /// <summary>
    ///  * @author Stuart
    /// </summary>
    public class BreakStatement : Statement
    {
        /// <summary>
        /// Class constructor.
        /// </summary>
        public BreakStatement()
        {
            if (!SectionInfo.In() && !FunctionInfo.In())
                throw new NslContextException(EnumSet.Of(NslContext.Section, NslContext.Function), "break");
            ScriptParser.tokenizer.MatchEolOrDie();
            if (CodeInfo.GetCurrent().GetBreakLabel() == null)
                throw new NslException("The \"break\" statement cannot be used here", true);
        }

        /// <summary>
        /// Class constructor.
        /// </summary>
        public override void Assemble()
        {
            CodeInfo.GetCurrent().GetBreakLabel().SetNotUsed(false);
            ScriptParser.WriteLine("Goto " + CodeInfo.GetCurrent().GetBreakLabel());
        }
    }
}