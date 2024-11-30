/*
 * AssignmentExpression.java
 */
using Java.Io;
using Java.Util;
using Nsl;

namespace Nsl.Expression
{
    public class AssignmentExpression : LogicalExpression
    {
        /// <summary>
        /// Class constructor.
        /// </summary>
        protected AssignmentExpression() : base(null, null, null)
        {
        }

        /// <summary>
        /// Class constructor.
        /// </summary>
        public AssignmentExpression(int registerIndex, Expression rightOperand) : base(null, null, rightOperand)
        {
            this.type = ExpressionType.Register;
            this.integerValue = registerIndex;
            if (rightOperand.type.Equals(ExpressionType.Register))
                Scope.GetCurrent().Check(rightOperand.integerValue);
        }

        /// <summary>
        /// Class constructor.
        /// </summary>
        public override string ToString()
        {
            if (this.integerValue == -1)
                return "(?? = " + this.rightOperand + ")";
            return "(" + RegisterList.GetCurrent()[this.integerValue] + " = " + this.rightOperand + ")";
        }

        /// <summary>
        /// Class constructor.
        /// </summary>
        /// <summary>
        /// Assembles the source code.
        /// </summary>
        public override void Assemble()
        {
            Register register;
            if (this.integerValue == -1)
                register = RegisterList.GetCurrent().GetNext();
            else
                register = RegisterList.GetCurrent()[this.integerValue];
            List<Register> parentReturnVars = ReturnVarExpression.SetRegisters(register);
            if (this.rightOperand is AssembleExpression)
            {
                string assign = register.ToString();
                ((AssembleExpression)this.rightOperand).Assemble(register);
                string value = register.ToString(); // Returns the register or a substituted value, if any
                if (!value.Equals(assign))
                    ScriptParser.WriteLine("StrCpy " + assign + " " + value);
            }
            else
            {
                string assign = register.ToString();
                string value = this.rightOperand.ToString();
                if (!value.Equals(assign))
                    ScriptParser.WriteLine("StrCpy " + assign + " " + value);
            }

            ReturnVarExpression.SetRegisters(parentReturnVars);
            if (this.integerValue == -1)
                register.SetInUse(false);
        }

        /// <summary>
        /// Class constructor.
        /// </summary>
        /// <summary>
        /// Assembles the source code.
        /// </summary>
        // Returns the register or a substituted value, if any
        public override void Assemble(Register var)
        {
            this.Assemble();
            if (var.GetIndex() != this.integerValue)
                var.Substitute(RegisterList.GetCurrent()[this.integerValue].ToString());
        }
    }
}