/*
 * SwitchDefaultCaseStatement.java
 */
using Java.Io;
using Nsl;

namespace Nsl.Statement
{
    public class SwitchDefaultCaseStatement : Statement
    {
        private Label label;
        public SwitchDefaultCaseStatement()
        {
            ScriptParser.tokenizer.MatchOrDie(':');
        }

        public virtual void SetLabel(Label label)
        {
            this.label = label;
        }

        public virtual Label GetLabel()
        {
            return this.label;
        }

        public override void Assemble()
        {
            this.label.Write();
        }
    }
}