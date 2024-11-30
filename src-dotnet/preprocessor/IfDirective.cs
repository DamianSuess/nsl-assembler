/*
 * IfDirective.java
 */
using Java.Io;
using Nsl;

namespace Nsl.Preprocessor
{
    /// <summary>
    ///  * @author Stuart
    /// </summary>
    public class IfDirective : Statement
    {
        private static bool inIfDirective = false;
        private StatementList statementList;
        /// <summary>
        /// Class constructor.
        /// </summary>
        public IfDirective()
        {
            this.statementList = null;
            while (true)
            {
                int line = ScriptParser.tokenizer.Lineno();
                Expression booleanExpression = Expression.MatchComplex();
                if (booleanExpression is AssembleExpression)
                    throw new NslException("\"#if\" directive requires a Boolean expression that can be evaluated", true);
                if (booleanExpression.GetBooleanValue() == true && this.statementList == null)
                {
                    bool inIfDirectiveOld = inIfDirective;
                    inIfDirective = true;
                    this.statementList = StatementList.Match();
                    inIfDirective = inIfDirectiveOld;
                }
                else
                {
                    do
                    {
                        if (ScriptParser.tokenizer.TokenIs("#endif") || ScriptParser.tokenizer.TokenIs("#else") || ScriptParser.tokenizer.TokenIs("#elseif"))
                            break;
                    }
                    while (ScriptParser.tokenizer.TokenNext());
                }

                if (ScriptParser.tokenizer.Match("#elseif"))
                    continue;
                if (ScriptParser.tokenizer.Match("#else"))
                {
                    if (this.statementList == null)
                    {
                        bool inIfDirectiveOld = inIfDirective;
                        inIfDirective = true;
                        this.statementList = StatementList.Match();
                        inIfDirective = inIfDirectiveOld;
                    }
                    else
                    {
                        do
                        {
                            if (ScriptParser.tokenizer.TokenIs("#endif"))
                                break;
                        }
                        while (ScriptParser.tokenizer.TokenNext());
                    }
                }

                if (ScriptParser.tokenizer.Match("#endif"))
                    break;
                throw new NslException("\"#if\" missing matching \"#endif\"", line);
            }
        }

        /// <summary>
        /// Class constructor.
        /// </summary>
        public static bool In()
        {
            return inIfDirective;
        }

        /// <summary>
        /// Class constructor.
        /// </summary>
        /// <summary>
        /// Assembles the source code.
        /// </summary>
        public override void Assemble()
        {
            if (this.statementList != null)
                this.statementList.Assemble();
        }
    }
}