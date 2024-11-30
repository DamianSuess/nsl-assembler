/*
 * ForStatement.java
 */
using Java.Io;
using Java.Util;
using Nsl;

namespace Nsl.Statement
{
    /// <summary>
    ///  * @author Stuart
    /// </summary>
    public class ForStatement : Statement
    {
        private readonly List<Expression> assignmentExpressions;
        private readonly Expression booleanExpression;
        private readonly List<Expression> loopExpressions;
        private readonly BlockStatement blockStatement;
        /// <summary>
        /// Class constructor.
        /// </summary>
        public ForStatement()
        {
            if (!SectionInfo.In() && !FunctionInfo.In())
                throw new NslContextException(EnumSet.Of(NslContext.Section, NslContext.Function), "for");
            ScriptParser.tokenizer.MatchOrDie('(');
            this.assignmentExpressions = new List<Expression>();
            if (!ScriptParser.tokenizer.Match(';'))
            {
                do
                {
                    Expression left = Expression.MatchComplex();
                    if (!(left is AssignmentExpression))
                        throw new NslException("A \"for\" statement requires an assignment expression for initialization", true);
                    this.assignmentExpressions.Add(left);
                }
                while (ScriptParser.tokenizer.Match(','));
                ScriptParser.tokenizer.MatchOrDie(';');
            }

            if (!ScriptParser.tokenizer.Match(';'))
            {
                this.booleanExpression = Expression.MatchComplex();
                if (!this.booleanExpression.GetType().Equals(ExpressionType.Boolean))
                    throw new NslException("A \"for\" statement requires a Boolean expression for its condition", true);
                ScriptParser.tokenizer.MatchOrDie(';');
            }
            else
                this.booleanExpression = null;
            this.loopExpressions = new List<Expression>();
            if (!ScriptParser.tokenizer.Match(')'))
            {
                do
                {
                    Expression left = Expression.MatchComplex();
                    if (!(left is AssignmentExpression))
                        throw new NslException("A \"for\" statement requires an assignment expression for each iteration", true);
                    this.loopExpressions.Add(left);
                }
                while (ScriptParser.tokenizer.Match(','));
                ScriptParser.tokenizer.MatchOrDie(')');
            }


            // Set non-null values so that the block statement can contain break or continue statements.
            CodeInfo.GetCurrent().SetBreakLabel(RelativeJump.Zero);
            CodeInfo.GetCurrent().SetContinueLabel(RelativeJump.Zero);
            this.blockStatement = new BlockStatement();
            CodeInfo.GetCurrent().SetBreakLabel(null);
            CodeInfo.GetCurrent().SetContinueLabel(null);
        }

        /// <summary>
        /// Class constructor.
        /// </summary>
        // Set non-null values so that the block statement can contain break or continue statements.
        public override void Assemble()
        {

            // Do not assemble anything if the for loop will never loop!
            if (this.booleanExpression != null && this.booleanExpression.IsLiteral() && this.booleanExpression.GetBooleanValue() == false)
            {

                // May still need assembling even though it is a literal.
                AssembleExpression.AssembleIfRequired(this.booleanExpression);
                return;
            }

            Label gotoLoop = LabelList.GetCurrent().GetNext();
            Label gotoContinue = LabelList.GetCurrent().GetNext();
            Label gotoEnd = LabelList.GetCurrent().GetNext();

            // We don't use the continue label unless a continue statement has been used.
            gotoContinue.SetNotUsed(true);
            foreach (Expression assignmentExpression in assignmentExpressions)
                ((AssembleExpression)assignmentExpression).Assemble();
            gotoLoop.Write();
            if (this.booleanExpression != null && this.booleanExpression is ConditionalExpression)
            {
                Label gotoEnter = LabelList.GetCurrent().GetNext();
                ((ConditionalExpression)this.booleanExpression).Assemble(gotoEnter, gotoEnd);
                gotoEnter.Write();
            }

            Label parentBreak = CodeInfo.GetCurrent().SetBreakLabel(gotoEnd);
            Label parentContinue = CodeInfo.GetCurrent().SetContinueLabel(gotoContinue);
            this.blockStatement.Assemble();
            CodeInfo.GetCurrent().SetBreakLabel(parentBreak);
            CodeInfo.GetCurrent().SetContinueLabel(parentContinue);
            gotoContinue.Write();
            foreach (Expression loopExpression in loopExpressions)
                ((AssembleExpression)loopExpression).Assemble();
            ScriptParser.WriteLine("Goto " + gotoLoop);
            gotoEnd.Write();
        }
    }
}