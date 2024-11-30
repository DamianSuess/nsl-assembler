/*
 * MacroDirective.java
 */
using Java.Io;
using Java.Util;
using Nsl;
using Nsl.Expression;
using Nsl.Statement;

namespace Nsl.Preprocessor
{
    public class MacroDirective : Statement
    {
        /// <summary>
        /// Class constructor.
        /// </summary>
        public MacroDirective()
        {
            int macroLine = ScriptParser.tokenizer.Lineno();
            string name = ScriptParser.tokenizer.MatchAWord("a macro name");
            ScriptParser.tokenizer.MatchOrDie('(');
            HashSet<string> paramsSet = new HashSet<string>();
            List<string> paramsList = new List<string>();
            if (!ScriptParser.tokenizer.TokenIs(')'))
            {
                while (true)
                {
                    string word = ScriptParser.tokenizer.MatchAWord("a constant name");
                    if (!paramsSet.Add(word))
                        throw new NslException("Macro \"" + name + "\" has parameter names with the same name", macroLine);
                    paramsList.Add(word);
                    if (ScriptParser.tokenizer.TokenIs(')'))
                        break;
                    ScriptParser.tokenizer.MatchOrDie(',');
                }
            }

            string contents = ScriptParser.tokenizer.ReadUntil("#macroend");
            ScriptParser.tokenizer.TokenNext();
            if (!MacroList.GetCurrent().Add(new Macro(name, paramsList.ToArray(new string[0]), macroLine, contents)))
                throw new NslException("Macro \"" + name + "\" already defined with " + paramsList.Count + " parameters", macroLine);
        }

        /// <summary>
        /// Class constructor.
        /// </summary>
        /// <summary>
        /// Matches the #return directive which can only be used inside a #macro.
        /// </summary>
        public static void MatchReturnDirective()
        {
            MacroEvaluated macro = MacroEvaluated.GetCurrent();
            if (macro == null)
                throw new NslException("\"#return\" directive can only be used within a \"#macro\"", true);
            bool specialStringNoEscapePrevious = Expression.SetSpecialStringEscape(false);
            if (ScriptParser.tokenizer.TokenIs('('))
            {
                macro.SetReturnValues(Expression.MatchList());
            }
            else
            {
                List<Expression> returnValues = new List<Expression>();
                returnValues.Add(Expression.MatchComplex());
                macro.SetReturnValues(returnValues);
            }

            Expression.SetSpecialStringEscape(specialStringNoEscapePrevious);
        }

        /// <summary>
        /// Class constructor.
        /// </summary>
        /// <summary>
        /// Matches the #return directive which can only be used inside a #macro.
        /// </summary>
        /// <summary>
        /// Assembles the source code.
        /// </summary>
        public override void Assemble()
        {
        }
    }
}