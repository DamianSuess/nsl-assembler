/*
 * UndefDirective.java
 */
using Java.Io;
using Nsl;
using Nsl.Statement;

namespace Nsl.Preprocessor
{
    public class UndefDirective : Statement
    {
        /// <summary>
        /// Class constructor.
        /// </summary>
        public UndefDirective()
        {
            int line = ScriptParser.tokenizer.Lineno();
            string name = ScriptParser.tokenizer.MatchAWord("a constant name");
            if (!DefineList.GetCurrent().Remove(name))
                throw new NslException("Constant \"" + name + "\" is not defined", line);
        }

        /// <summary>
        /// Class constructor.
        /// </summary>
        public override void Assemble()
        {
        }
    }
}