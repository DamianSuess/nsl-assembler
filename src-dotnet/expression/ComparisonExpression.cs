/*
 * ComparisonExpression.java
 */
using Java.Io;
using Nsl;

namespace Nsl.Expression
{
    public class ComparisonExpression : ConditionalExpression
    {
        private readonly ComparisonType comparisonType;
        public ComparisonExpression(Expression leftOperand, string @operator, Expression rightOperand, ComparisonType comparisonType) : base(leftOperand, @operator, rightOperand)
        {
            this.comparisonType = comparisonType;
        }

        /// <summary>
        /// Class constructor.
        /// </summary>
        public ComparisonExpression() : base(null, null, null)
        {
            this.comparisonType = ComparisonType.Integer;
        }

        /// <summary>
        /// Class constructor.
        /// </summary>
        private static void AssembleCmp(string leftOperand, string @operator, string rightOperand, Label gotoA, Label gotoB, ComparisonType comparisonType)
        {
            if (@operator.Equals("=="))
            {
                if (comparisonType == ComparisonType.String)
                    ScriptParser.WriteLine(String.Format("StrCmp %s %s %s %s", leftOperand, rightOperand, gotoA, gotoB));
                else if (comparisonType == ComparisonType.StringCaseSensitive)
                    ScriptParser.WriteLine(String.Format("StrCmpS %s %s %s %s", leftOperand, rightOperand, gotoA, gotoB));
                else if (comparisonType == ComparisonType.IntegerUnsigned)
                    ScriptParser.WriteLine(String.Format("IntCmpU %s %s %s %s %s", leftOperand, rightOperand, gotoA, gotoB, gotoB));
                else
                    ScriptParser.WriteLine(String.Format("IntCmp %s %s %s %s %s", leftOperand, rightOperand, gotoA, gotoB, gotoB));
            }
            else if (@operator.Equals("!="))
            {
                if (comparisonType == ComparisonType.String)
                    ScriptParser.WriteLine(String.Format("StrCmp %s %s %s %s", leftOperand, rightOperand, gotoB, gotoA));
                else if (comparisonType == ComparisonType.StringCaseSensitive)
                    ScriptParser.WriteLine(String.Format("StrCmpS %s %s %s %s", leftOperand, rightOperand, gotoB, gotoA));
                else if (comparisonType == ComparisonType.IntegerUnsigned)
                    ScriptParser.WriteLine(String.Format("IntCmpU %s %s %s %s %s", leftOperand, rightOperand, gotoB, gotoA, gotoA));
                else
                    ScriptParser.WriteLine(String.Format("IntCmp %s %s %s %s %s", leftOperand, rightOperand, gotoB, gotoA, gotoA));
            }
            else if (@operator.Equals("<="))
            {
                if (comparisonType == ComparisonType.IntegerUnsigned)
                    ScriptParser.WriteLine(String.Format("IntCmpU %s %s %s %s %s", leftOperand, rightOperand, gotoA, gotoA, gotoB));
                else
                    ScriptParser.WriteLine(String.Format("IntCmp %s %s %s %s %s", leftOperand, rightOperand, gotoA, gotoA, gotoB));
            }
            else if (@operator.Equals(">="))
            {
                if (comparisonType == ComparisonType.IntegerUnsigned)
                    ScriptParser.WriteLine(String.Format("IntCmpU %s %s %s %s %s", leftOperand, rightOperand, gotoA, gotoB, gotoA));
                else
                    ScriptParser.WriteLine(String.Format("IntCmp %s %s %s %s %s", leftOperand, rightOperand, gotoA, gotoB, gotoA));
            }
            else if (@operator.Equals("<"))
            {
                if (comparisonType == ComparisonType.IntegerUnsigned)
                    ScriptParser.WriteLine(String.Format("IntCmpU %s %s %s %s %s", leftOperand, rightOperand, gotoB, gotoA, gotoB));
                else
                    ScriptParser.WriteLine(String.Format("IntCmp %s %s %s %s %s", leftOperand, rightOperand, gotoB, gotoA, gotoB));
            }
            else if (@operator.Equals(">"))
            {
                if (comparisonType == ComparisonType.IntegerUnsigned)
                    ScriptParser.WriteLine(String.Format("IntCmpU %s %s %s %s %s", leftOperand, rightOperand, gotoB, gotoB, gotoA));
                else
                    ScriptParser.WriteLine(String.Format("IntCmp %s %s %s %s %s", leftOperand, rightOperand, gotoB, gotoB, gotoA));
            }
            else
                throw new NslException("Unknown operator " + @operator);
        }

        /// <summary>
        /// Class constructor.
        /// </summary>
        public virtual void Assemble(Register var)
        {
            Label gotoA = LabelList.GetCurrent().GetNext();
            Label gotoB = LabelList.GetCurrent().GetNext();
            if (this.booleanValue)
                this.Assemble(gotoB, gotoA);
            else
                this.Assemble(gotoA, gotoB);
            gotoA.Write();
            ScriptParser.WriteLine("StrCpy " + var + " true");
            ScriptParser.WriteLine("Goto +2");
            gotoB.Write();
            ScriptParser.WriteLine("StrCpy " + var + " false");
        }

        /// <summary>
        /// Class constructor.
        /// </summary>
        public virtual void Assemble(Label gotoA, Label gotoB)
        {
            if (this.booleanValue)
            {
                Label gotoTemp = gotoA;
                gotoA = gotoB;
                gotoB = gotoTemp;
            }

            if (this.leftOperand is AssembleExpression && this.rightOperand is AssembleExpression)
            {
                Register varLeft = RegisterList.GetCurrent().GetNext();
                Register varRight = RegisterList.GetCurrent().GetNext();
                ((AssembleExpression)this.leftOperand).Assemble(varLeft);
                ((AssembleExpression)this.rightOperand).Assemble(varRight);
                AssembleCmp(varLeft.ToString(), this.@operator, varRight.ToString(), gotoA, gotoB, this.comparisonType);
                varRight.SetInUse(false);
                varLeft.SetInUse(false);
            }
            else if (this.leftOperand is AssembleExpression)
            {
                Register varLeft = RegisterList.GetCurrent().GetNext();
                ((AssembleExpression)this.leftOperand).Assemble(varLeft);
                AssembleCmp(varLeft.ToString(), this.@operator, this.rightOperand.ToString(), gotoA, gotoB, this.comparisonType);
                varLeft.SetInUse(false);
            }
            else if (this.rightOperand is AssembleExpression)
            {
                Register varRight = RegisterList.GetCurrent().GetNext();
                ((AssembleExpression)this.rightOperand).Assemble(varRight);
                AssembleCmp(this.leftOperand.ToString(), this.@operator, varRight.ToString(), gotoA, gotoB, this.comparisonType);
                varRight.SetInUse(false);
            }
            else
            {
                AssembleCmp(this.leftOperand.ToString(), this.@operator, this.rightOperand.ToString(), gotoA, gotoB, this.comparisonType);
            }
        }
    }
}