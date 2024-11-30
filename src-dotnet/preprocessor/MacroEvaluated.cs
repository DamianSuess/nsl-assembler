/*
 * MacroEvaluated.java
 */
using Java.Io;
using Java.Util;
using Nsl;

namespace Nsl.Preprocessor
{
    public class MacroEvaluated : MultipleReturnValueAssembleExpression
    {
        private readonly string name;
        private readonly DefineList defineList;
        private List<Expression> returnValues;
        private List<Register> returnRegisters;
        private StatementList statementList;
        private static MacroEvaluated current = null;
        public static MacroEvaluated GetCurrent()
        {
            return current;
        }

        public MacroEvaluated(string name, DefineList defineList)
        {
            this.name = name;
            this.defineList = defineList;
            this.returnValues = new List<Expression>();
            this.returnRegisters = new List<Register>();
            this.statementList = null;
        }

        public virtual string GetName()
        {
            return this.name;
        }

        /// <summary>
        /// Evaluates the macro's contents.
        /// </summary>
        public virtual void Evaluate()
        {
            MacroEvaluated parent = current;
            current = this;
            this.statementList = StatementList.Match();
            current = parent;
        }

        /// <summary>
        /// Evaluates the macro's contents.
        /// </summary>
        public virtual void SetReturnValues(List<Expression> returnValues)
        {
            if (returnValues != null && returnValues.Count == 1)
            {
                Expression returnValue = returnValues[0];
                this.type = returnValue.GetType();
                this.booleanValue = returnValue.GetBooleanValue();
                this.stringValue = returnValue.GetStringValue();
                this.integerValue = returnValue.GetIntegerValue();
            }

            this.returnValues = returnValues;
        }

        /// <summary>
        /// Evaluates the macro's contents.
        /// </summary>
        public virtual List<Expression> GetReturnValues()
        {
            return this.returnValues;
        }

        /// <summary>
        /// Evaluates the macro's contents.
        /// </summary>
        public virtual List<Register> GetReturnRegisters()
        {
            return this.returnRegisters;
        }

        /// <summary>
        /// Evaluates the macro's contents.
        /// </summary>
        public virtual DefineList GetDefineList()
        {
            return this.defineList;
        }

        /// <summary>
        /// Evaluates the macro's contents.
        /// </summary>
        /// <summary>
        /// Assembles the source code.
        /// </summary>
        public override void Assemble()
        {
            List<Register> parentReturnVars = ReturnVarExpression.SetRegisters(this.returnRegisters);
            this.statementList.Assemble();
            ReturnVarExpression.SetRegisters(parentReturnVars);
        }

        /// <summary>
        /// Evaluates the macro's contents.
        /// </summary>
        /// <summary>
        /// Assembles the source code.
        /// </summary>
        public override void Assemble(Register var)
        {
            this.returnRegisters.Add(var);
            List<Register> parentReturnVars = ReturnVarExpression.SetRegisters(this.returnRegisters);
            this.statementList.Assemble();
            ReturnVarExpression.SetRegisters(parentReturnVars);
        }

        /// <summary>
        /// Evaluates the macro's contents.
        /// </summary>
        /// <summary>
        /// Assembles the source code.
        /// </summary>
        public override void Assemble(List<Register> vars)
        {
            this.returnRegisters = vars;
            List<Register> parentReturnVars = ReturnVarExpression.SetRegisters(this.returnRegisters);
            this.statementList.Assemble();

            // this.returnValues is only empty if an #nsis directive was used in the
            // macro (which sets it to an empty array).
            int returnValuesCount = this.returnValues.Count;
            if (this.returnValues != null && returnValuesCount == vars.Count)
            {
                for (int i = 0; i < returnValuesCount; i++)
                {
                    Expression returnValue = this.returnValues[i];
                    Register register = vars[i];
                    if (returnValue is AssembleExpression)
                    {
                        string assign = register.ToString();
                        ((AssembleExpression)returnValue).Assemble(register);
                        string value = register.ToString(); // Returns the register or a substituted value, if any
                        if (!value.Equals(assign))
                            ScriptParser.WriteLine("StrCpy " + assign + " " + value);
                    }
                    else
                    {
                        string assign = register.ToString();
                        string value = returnValue.ToString();
                        if (!value.Equals(assign))
                            ScriptParser.WriteLine("StrCpy " + assign + " " + value);
                    }
                }
            }

            ReturnVarExpression.SetRegisters(parentReturnVars);
        }

        /// <summary>
        /// Evaluates the macro's contents.
        /// </summary>
        /// <summary>
        /// Assembles the source code.
        /// </summary>
        // this.returnValues is only empty if an #nsis directive was used in the
        // macro (which sets it to an empty array).
        // Returns the register or a substituted value, if any
        public override string ToString()
        {
            return this.ToString(false);
        }
    }
}