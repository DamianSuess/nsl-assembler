/*
 * IfStatement.java
 */
using Java.Io;
using Java.Util;
using Nsl;

namespace Nsl.Statement
{
    /// <summary>
    ///  * @author Stuart
    /// </summary>
    public class IfStatement : Statement
    {
        private readonly Expression booleanExpression;
        private readonly BlockStatement blockStatement;
        private IfStatement elseStatement;
        /// <summary>
        /// Class constructor.
        /// </summary>
        public IfStatement()
        {
            if (!SectionInfo.In() && !FunctionInfo.In())
                throw new NslContextException(EnumSet.Of(NslContext.Section, NslContext.Function), "if");
            ScriptParser.tokenizer.MatchOrDie('(');
            this.booleanExpression = Expression.MatchComplex();
            if (!this.booleanExpression.GetType().Equals(ExpressionType.Boolean))
                throw new NslException("An \"if\" statement requires a Boolean expression", true);
            ScriptParser.tokenizer.MatchOrDie(')');
            this.blockStatement = new BlockStatement();
            this.elseStatement = null;
            if (ScriptParser.tokenizer.TokenIs("else"))
            {
                ScriptParser.tokenizer.TokenNext();
                if (ScriptParser.tokenizer.Match("if"))
                    this.elseStatement = new IfStatement();
                else
                    this.elseStatement = new IfStatement(new BlockStatement());
            }
        }

        /// <summary>
        /// Class constructor.
        /// </summary>
        private IfStatement(BlockStatement blockStatement)
        {
            this.booleanExpression = null;
            this.blockStatement = blockStatement;
            this.elseStatement = null;
        }

        /// <summary>
        /// Class constructor.
        /// </summary>
        /// <summary>
        /// Assembles the source code.
        /// </summary>
        private void Assemble(Label gotoEnd)
        {
            if (this.booleanExpression != null)
            {
                if (this.booleanExpression.IsLiteral())
                {

                    // May still need assembling even though it is a literal.
                    AssembleExpression.AssembleIfRequired(this.booleanExpression);
                    if (this.booleanExpression.GetBooleanValue() == true)
                        this.blockStatement.Assemble();
                    else if (this.elseStatement != null)
                        this.elseStatement.Assemble();
                    return;
                }

                Label gotoA = LabelList.GetCurrent().GetNext();
                gotoA.SetNotUsed(true);
                Label gotoB;
                if (this.elseStatement != null || gotoEnd == null)
                {
                    gotoB = LabelList.GetCurrent().GetNext();
                    gotoB.SetNotUsed(true);
                }
                else
                    gotoB = gotoEnd;
                ((ConditionalExpression)this.booleanExpression).Assemble(gotoA, gotoB);
                gotoA.Write();
                this.blockStatement.Assemble();
                if (this.elseStatement != null || gotoEnd == null)
                {
                    if (gotoEnd != null)
                        ScriptParser.WriteLine("Goto " + gotoEnd);
                    gotoB.Write();
                }
            }
            else
                this.blockStatement.Assemble();
            if (this.elseStatement != null)
                this.elseStatement.Assemble(gotoEnd);
        }

        /// <summary>
        /// Class constructor.
        /// </summary>
        /// <summary>
        /// Assembles the source code.
        /// </summary>
        // May still need assembling even though it is a literal.
        /// <summary>
        /// Assembles the source code.
        /// </summary>
        public override void Assemble()
        {

            //if (!this.blockStatement.isEmpty())
            {
                if (this.booleanExpression != null && this.booleanExpression.IsLiteral())
                {

                    // May still need assembling even though it is a literal.
                    AssembleExpression.AssembleIfRequired(this.booleanExpression);

                    // Boolean expression evaluates to true.
                    if (this.booleanExpression.GetBooleanValue() == true)
                        this.blockStatement.Assemble();
                    else if (this.elseStatement != null)
                        this.elseStatement.Assemble();
                }
                else
                {
                    if (this.elseStatement != null)
                    {
                        Label gotoEnd = LabelList.GetCurrent().GetNext();
                        gotoEnd.SetNotUsed(true);
                        this.Assemble(gotoEnd);
                        gotoEnd.Write();
                    }
                    else
                        this.Assemble(null);
                }
            }
        }
    }
}