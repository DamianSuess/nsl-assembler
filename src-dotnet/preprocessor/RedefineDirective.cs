/*
 * RedefineDirective.java
 */
using Java.Io;
using Nsl;
using Nsl.Statement;

namespace Nsl.Preprocessor
{
    public class RedefineDirective : Statement
    {
        /// <summary>
        /// Class constructor.
        /// </summary>
        public RedefineDirective()
        {
            int line = ScriptParser.tokenizer.Lineno();
            string name = ScriptParser.tokenizer.MatchAWord("a constant name");
            int valueLine = ScriptParser.tokenizer.Lineno();

            // If the next token is on a new line then we can assume the constant has no
            // value associated with it.
            Expression value;
            if (line != valueLine)
            {
                value = Expression.Empty;
            }
            else
            {
                bool specialStringNoEscapePrevious = Expression.SetSpecialStringEscape(false);
                value = Expression.MatchComplex();
                Expression.SetSpecialStringEscape(specialStringNoEscapePrevious);
            }

            if (value is AssembleExpression)
                throw new NslException("\"#redefine\" directive (for constant \"" + name + "\") requires an expression that can be evaluated", true);
            DefineList.GetCurrent().Add(name, value);
        }

        /// <summary>
        /// Class constructor.
        /// </summary>
        // If the next token is on a new line then we can assume the constant has no
        // value associated with it.
        public override void Assemble()
        {
        }
    }
}