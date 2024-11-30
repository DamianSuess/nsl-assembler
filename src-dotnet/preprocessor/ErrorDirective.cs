/*
 * ErrorDirective.java
 */
using Java.Io;
using Nsl;
using Nsl.Statement;

namespace Nsl.Preprocessor
{
    public class ErrorDirective : Statement
    {
        /// <summary>
        /// Class constructor.
        /// </summary>
        public ErrorDirective()
        {
            int line = ScriptParser.tokenizer.Lineno();
            string error = ScriptParser.tokenizer.MatchAString();
            if (error == null || (error = error.Trim()).IsEmpty())
                throw new NslException("An error occurred (no error message specified)", line);
            throw new NslException(error, line);
        }

        /// <summary>
        /// Class constructor.
        /// </summary>
        public override void Assemble()
        {
        }
    }
}