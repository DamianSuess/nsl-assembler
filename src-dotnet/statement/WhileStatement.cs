/*
 * WhileStatement.java
 */
using Java.Io;
using Java.Util;
using Nsl;

namespace Nsl.Statement
{
    /// <summary>
    ///  * @author Stuart
    /// </summary>
    public class WhileStatement : Statement
    {
        private readonly Expression booleanExpression;
        private readonly BlockStatement blockStatement;
        /// <summary>
        /// Class constructor.
        /// </summary>
        public WhileStatement()
        {
            if (!SectionInfo.In() && !FunctionInfo.In())
                throw new NslContextException(EnumSet.Of(NslContext.Section, NslContext.Function), "while");
            ScriptParser.tokenizer.MatchOrDie('(');
            this.booleanExpression = Expression.MatchComplex();
            if (!this.booleanExpression.GetType().Equals(ExpressionType.Boolean))
                throw new NslException("A \"while\" statement requires a Boolean expression", true);
            ScriptParser.tokenizer.MatchOrDie(')');

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

            // Do not assemble anything if the while loop will never loop!
            if (this.booleanExpression != null && this.booleanExpression.IsLiteral() && this.booleanExpression.GetBooleanValue() == false)
            {

                // May still need assembling even though it is a literal.
                AssembleExpression.AssembleIfRequired(this.booleanExpression);
                return;
            }

            Label gotoLoop = LabelList.GetCurrent().GetNext();
            Label gotoEnd = LabelList.GetCurrent().GetNext();
            gotoLoop.Write();
            gotoEnd.SetNotUsed(true);
            if (this.booleanExpression != null && this.booleanExpression is ConditionalExpression)
            {
                Label gotoEnter = LabelList.GetCurrent().GetNext();
                gotoEnter.SetNotUsed(true);
                ((ConditionalExpression)this.booleanExpression).Assemble(gotoEnter, gotoEnd);
                gotoEnter.Write();
            }

            Label parentBreak = CodeInfo.GetCurrent().SetBreakLabel(gotoEnd);
            Label parentContinue = CodeInfo.GetCurrent().SetContinueLabel(gotoLoop);
            this.blockStatement.Assemble();
            CodeInfo.GetCurrent().SetBreakLabel(parentBreak);
            CodeInfo.GetCurrent().SetContinueLabel(parentContinue);
            ScriptParser.WriteLine("Goto " + gotoLoop);
            gotoEnd.Write();
        }
    }
}