/*
 * SwitchStatement.java
 */
using Java.Io;
using Java.Util;
using Nsl;

namespace Nsl.Statement
{
    public class SwitchStatement : Statement
    {
        private readonly Expression switchExpression;
        private readonly List<Statement> statementList;
        private readonly List<SwitchCaseStatement> casesList;
        private SwitchDefaultCaseStatement defaultCase;
        /// <summary>
        /// Class constructor.
        /// </summary>
        public SwitchStatement()
        {
            if (!SectionInfo.In() && !FunctionInfo.In())
                throw new NslContextException(EnumSet.Of(NslContext.Section, NslContext.Function), "switch");
            int lineNo = ScriptParser.tokenizer.Lineno();
            ScriptParser.tokenizer.MatchOrDie('(');
            this.switchExpression = Expression.MatchComplex();
            ScriptParser.tokenizer.MatchOrDie(')');
            ScriptParser.tokenizer.MatchOrDie('{');

            // Set non-null values so that the block statement can contain break statements.
            CodeInfo.GetCurrent().SetBreakLabel(RelativeJump.Zero);
            this.statementList = new List<Statement>();
            this.casesList = new List<SwitchCaseStatement>();
            this.defaultCase = null;

            // Get the statements including case statements.
            while (true)
            {
                if (ScriptParser.tokenizer.Match("case"))
                {
                    Statement statement = new SwitchCaseStatement();
                    if (this.defaultCase != null)
                        throw new NslException("The \"default\" case in a \"switch\" statement must be the last case", true);
                    this.casesList.Add((SwitchCaseStatement)statement);
                    this.statementList.Add(statement);
                }
                else if (ScriptParser.tokenizer.Match("default"))
                {
                    this.defaultCase = new SwitchDefaultCaseStatement();
                    this.statementList.Add(this.defaultCase);
                }
                else
                {
                    Statement statement = Statement.Match();
                    if (statement == null)
                        break;
                    this.statementList.Add(statement);
                }
            }


            // No cases?
            if (this.casesList.IsEmpty())
                throw new NslException("A \"switch\" statement must have at least one \"case\" statement", true);

            // Validate switch cases for jump instructions.
            if (this.switchExpression is JumpExpression)
                ((JumpExpression)this.switchExpression).CheckSwitchCases(this.casesList, lineNo);

            // Check the last statement is a break statement.
            bool noBreak = true;
            if (!this.statementList.IsEmpty())
            {
                Statement last = this.statementList[this.statementList.Count - 1];
                if (last is BlockStatement)
                    last = ((BlockStatement)last).GetLast();
                if (last is BreakStatement)
                    noBreak = false;
            }

            if (noBreak)
                throw new NslException("A \"switch\" statement must end with a \"break\" statement", true);
            CodeInfo.GetCurrent().SetBreakLabel(null);
            ScriptParser.tokenizer.MatchOrDie('}');
        }

        /// <summary>
        /// Class constructor.
        /// </summary>
        // Set non-null values so that the block statement can contain break statements.
        // Get the statements including case statements.
        // No cases?
        // Validate switch cases for jump instructions.
        // Check the last statement is a break statement.
        public override void Assemble()
        {

            // Do not assemble anything if there are no cases!
            if (this.casesList.IsEmpty())
                return;

            // Give each case a label.
            foreach (SwitchCaseStatement statement in this.casesList)
                statement.SetLabel(LabelList.GetCurrent().GetNext());
            if (this.defaultCase != null)
                this.defaultCase.SetLabel(LabelList.GetCurrent().GetNext());
            Label gotoEnd = LabelList.GetCurrent().GetNext();
            Label gotoStart = LabelList.GetCurrent().GetNext();

            // Go to the jump table which is assembled after the switch case labels and
            // statements.
            ScriptParser.WriteLine("Goto " + gotoStart);

            // Using "break;" inside a switch jumps to the end.
            Label parentBreak = CodeInfo.GetCurrent().SetBreakLabel(gotoEnd);

            // Assemble all the statements inside the switch { }. This includes the
            // case labels.
            foreach (Statement statement in this.statementList)
                statement.Assemble();

            // Restore the parent break label.
            CodeInfo.GetCurrent().SetBreakLabel(parentBreak);

            // Label at the top of the jump table.
            gotoStart.Write();

            // Jump instructions can jump directly to the case labels.
            if (this.switchExpression is JumpExpression)
            {
                ((JumpExpression)this.switchExpression).Assemble(this.casesList);
            }
            else

            // Other expressions we just assemble them if required and compare their
            // result.
            {
                Expression varOrSwitchExpression = AssembleExpression.GetRegisterOrExpression(this.switchExpression);
                foreach (SwitchCaseStatement caseStatement in this.casesList)
                {

                    // Type is an integer; use IntCmp.
                    if (caseStatement.GetMatch().GetType().Equals(ExpressionType.Integer))
                    {
                        ScriptParser.WriteLine(String.Format("IntCmp %s %s %s", varOrSwitchExpression, caseStatement.GetMatch(), caseStatement.GetLabel()));
                    }
                    else

                    // Type is a string; use StrCmp or StrCmpS (if special `` quotes were
                    // used).
                    {
                        ScriptParser.WriteLine(String.Format("StrCmp%s %s %s %s", caseStatement.GetMatch().GetType().Equals(ExpressionType.StringSpecial) ? "S" : "", varOrSwitchExpression, caseStatement.GetMatch(), caseStatement.GetLabel()));
                    }
                }

                varOrSwitchExpression.SetInUse(false);
            }


            // Default case jump.
            if (this.defaultCase != null)
                ScriptParser.WriteLine("Goto " + this.defaultCase.GetLabel());
            gotoEnd.Write();
        }
    }
}