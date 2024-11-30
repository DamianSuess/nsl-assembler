/*
 * DoStatement.java
 */
using Java.Io;
using Java.Util;
using Nsl;

namespace Nsl.Statement
{
    /// <summary>
    ///  * @author Stuart
    /// </summary>
    public class DoStatement : Statement
    {
        private readonly Expression booleanExpression;
        private readonly BlockStatement blockStatement;
        /// <summary>
        /// Class constructor.
        /// </summary>
        public DoStatement()
        {
            if (!SectionInfo.In() && !FunctionInfo.In())
                throw new NslContextException(EnumSet.Of(NslContext.Section, NslContext.Function), "do");

            // Set non-null values so that the block statement can contain break or continue statements.
            CodeInfo.GetCurrent().SetBreakLabel(RelativeJump.Zero);
            CodeInfo.GetCurrent().SetContinueLabel(RelativeJump.Zero);
            this.blockStatement = new BlockStatement();
            CodeInfo.GetCurrent().SetBreakLabel(null);
            CodeInfo.GetCurrent().SetContinueLabel(null);
            ScriptParser.tokenizer.MatchOrDie("while");
            ScriptParser.tokenizer.MatchOrDie('(');
            this.booleanExpression = Expression.MatchComplex();
            if (!this.booleanExpression.GetType().Equals(ExpressionType.Boolean))
                throw new NslException("A \"do\" statement requires a Boolean expression for its \"while\"", true);
            ScriptParser.tokenizer.MatchOrDie(')');
            ScriptParser.tokenizer.MatchEolOrDie();
        }

        /// <summary>
        /// Class constructor.
        /// </summary>
        // Set non-null values so that the block statement can contain break or continue statements.
        public override void Assemble()
        {
            Label gotoLoop = LabelList.GetCurrent().GetNext();
            Label gotoEnd = LabelList.GetCurrent().GetNext();
            gotoLoop.Write();
            gotoLoop.SetNotUsed(true);
            gotoEnd.SetNotUsed(true);
            Label parentBreak = CodeInfo.GetCurrent().SetBreakLabel(gotoEnd);
            Label parentContinue = CodeInfo.GetCurrent().SetContinueLabel(gotoLoop);
            this.blockStatement.Assemble();
            CodeInfo.GetCurrent().SetBreakLabel(parentBreak);
            CodeInfo.GetCurrent().SetContinueLabel(parentContinue);
            if (this.booleanExpression.IsLiteral() && this.booleanExpression.GetBooleanValue() == true)
            {
                ScriptParser.WriteLine("Goto " + gotoLoop);
            }
            else if (this.booleanExpression is ConditionalExpression)
            {
                ((ConditionalExpression)this.booleanExpression).Assemble(gotoLoop, gotoEnd);
            }
            else if (gotoLoop.IsNotUsed())
            {

                // Prevent makensis warning if loop label not used.
                ScriptParser.WriteLine("StrCmp \"\" \"\" 0 " + gotoLoop);
            }

            gotoEnd.Write();
        }
    }
}