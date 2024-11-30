/*
 * Expression.java
 */
using Nsl;

namespace Nsl.Expression
{
    public class ReferenceExpression : Expression
    {
        /// <summary>
        /// Class constructor.
        /// </summary>
        public ReferenceExpression()
        {
            ScriptParser.tokenizer.MatchOrDie('(');
            this.type = ExpressionType.String;
            this.stringValue = ScriptParser.tokenizer.MatchAWord("a name of reference");
            ScriptParser.tokenizer.MatchOrDie(')');
        }
    }
}