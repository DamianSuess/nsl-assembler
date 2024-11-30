/*
 * SwitchCaseStatement.java
 */
using Java.Io;
using Nsl;

namespace Nsl.Statement
{
    public class SwitchCaseStatement : Statement
    {
        private readonly Expression match;
        private readonly int lineNo;
        private Label label;
        public SwitchCaseStatement()
        {
            this.lineNo = ScriptParser.tokenizer.Lineno();
            this.match = Expression.Match();
            ScriptParser.tokenizer.MatchOrDie(':');
            if (!ExpressionType.IsBoolean(this.match) && !ExpressionType.IsInteger(this.match) && !ExpressionType.IsString(this.match))
                throw new NslException("\"case\" in a \"switch\" statement requires a literal string, Boolean or integer value", true);
        }

        public virtual void SetLabel(Label label)
        {
            this.label = label;
        }

        public virtual Label GetLabel()
        {
            return this.label;
        }

        public virtual int GetLineNo()
        {
            return this.lineNo;
        }

        public virtual Expression GetMatch()
        {
            return this.match;
        }

        public override void Assemble()
        {
            this.label.Write();
        }
    }
}