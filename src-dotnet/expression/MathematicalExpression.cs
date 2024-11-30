/*
 * MathematicalExpression.java
 */
using Java.Io;
using Nsl;

namespace Nsl.Expression
{
    public class MathematicalExpression : LogicalExpression
    {
        public MathematicalExpression(Expression leftOperand, string @operator, Expression rightOperand) : base(leftOperand, @operator, rightOperand)
        {
        }

        /*if (!(leftOperand instanceof MathematicalExpression) && leftOperand.type != ExpressionType.Register && leftOperand.type != ExpressionType.Integer)
      throw new NslException("The left operand must be an integer or a variable", true);
    if (!(rightOperand instanceof MathematicalExpression) && rightOperand.type != ExpressionType.Register && rightOperand.type != ExpressionType.Integer)
      throw new NslException("The right operand must be an integer or a variable", true);*/
        /// <summary>
        /// Assembles the source code.
        /// </summary>
        public override void Assemble()
        {
            throw new NotSupportedException("Not supported.");
        }

        /*if (!(leftOperand instanceof MathematicalExpression) && leftOperand.type != ExpressionType.Register && leftOperand.type != ExpressionType.Integer)
      throw new NslException("The left operand must be an integer or a variable", true);
    if (!(rightOperand instanceof MathematicalExpression) && rightOperand.type != ExpressionType.Register && rightOperand.type != ExpressionType.Integer)
      throw new NslException("The right operand must be an integer or a variable", true);*/
        /// <summary>
        /// Assembles the source code.
        /// </summary>
        public override void Assemble(Register var)
        {
            this.Assemble(var, 0);
        }

        /*if (!(leftOperand instanceof MathematicalExpression) && leftOperand.type != ExpressionType.Register && leftOperand.type != ExpressionType.Integer)
      throw new NslException("The left operand must be an integer or a variable", true);
    if (!(rightOperand instanceof MathematicalExpression) && rightOperand.type != ExpressionType.Register && rightOperand.type != ExpressionType.Integer)
      throw new NslException("The right operand must be an integer or a variable", true);*/
        /// <summary>
        /// Assembles the source code.
        /// </summary>
        protected virtual void Assemble(Register var, int counter)
        {
            counter++;
            Register varLeft;
            if (this.leftOperand is AssignmentExpression)
                varLeft = RegisterList.GetCurrent()[this.leftOperand.integerValue];
            else if (counter == 1)
                varLeft = RegisterList.GetCurrent().GetNext();
            else
                varLeft = var;
            if (this.leftOperand is MathematicalExpression && this.rightOperand is MathematicalExpression)
            {
                ((MathematicalExpression)this.leftOperand).Assemble(varLeft, counter);
                Register varRight = RegisterList.GetCurrent().GetNext();
                ((MathematicalExpression)this.rightOperand).Assemble(varRight, counter);
                ScriptParser.WriteLine(String.Format("IntOp %s %s %s %s", var, varLeft, this.@operator, varRight));
                varRight.SetInUse(false);
            }
            else if (this.leftOperand is MathematicalExpression)
            {
                ((MathematicalExpression)this.leftOperand).Assemble(varLeft, counter);
                if (this.@operator.ToString().Equals("~"))
                    ScriptParser.WriteLine(String.Format("IntOp %s %s %s", var, varLeft, this.@operator));
                else
                    ScriptParser.WriteLine(String.Format("IntOp %s %s %s %s", var, varLeft, this.@operator, this.rightOperand));
            }
            else if (this.rightOperand is MathematicalExpression)
            {
                ((MathematicalExpression)this.rightOperand).Assemble(varLeft, counter);
                ScriptParser.WriteLine(String.Format("IntOp %s %s %s %s", var, this.leftOperand, this.@operator, varLeft));
            }
            else if (this.leftOperand is AssembleExpression || this.rightOperand is AssembleExpression)
            {
                Expression varOrLeft = AssembleExpression.GetRegisterOrExpression(this.leftOperand);
                if (this.@operator.ToString().Equals("~"))
                {
                    ScriptParser.WriteLine(String.Format("IntOp %s %s %s", var, varOrLeft, this.@operator));
                }
                else
                {
                    Expression varOrRight = AssembleExpression.GetRegisterOrExpression(this.rightOperand);
                    ScriptParser.WriteLine(String.Format("IntOp %s %s %s %s", var, varOrLeft, this.@operator, varOrRight));
                    varOrRight.SetInUse(false);
                }

                varOrLeft.SetInUse(false);
            }
            else
            {
                if (this.@operator.ToString().Equals("~"))
                    ScriptParser.WriteLine(String.Format("IntOp %s %s %s", var, this.leftOperand, this.@operator));
                else
                    ScriptParser.WriteLine(String.Format("IntOp %s %s %s %s", var, this.leftOperand, this.@operator, this.rightOperand));
            }

            if (counter == 1)
                varLeft.SetInUse(false);
        }
    }
}