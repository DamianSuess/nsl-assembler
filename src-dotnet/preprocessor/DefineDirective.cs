/*
 * DefineDirective.java
 */
using Java.Io;
using Nsl;
using Nsl.Statement;

namespace Nsl.Preprocessor
{
    public class DefineDirective : Statement
    {
        /// <summary>
        /// Class constructor.
        /// </summary>
        public DefineDirective()
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

            if (!value.IsLiteral() && value is AssembleExpression)
                throw new NslException("\"#define\" directive (for constant \"" + name + "\") requires an expression that can be evaluated", line);
            if (!DefineList.GetCurrent().Add(name, value))
                throw new NslException("Constant \"" + name + "\" already defined", line);
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