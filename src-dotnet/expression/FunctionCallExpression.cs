/*
 * FunctionCallExpression.java
 */
using Java.Io;
using Java.Util;
using Nsl;

namespace Nsl.Expression
{
    public class FunctionCallExpression : MultipleReturnValueAssembleExpression
    {
        private readonly List<Expression> params;
        private readonly int lineNo;
        public FunctionCallExpression(string name)
        {
            this.stringValue = name;
            this.@params = Expression.MatchList();
            this.lineNo = ScriptParser.tokenizer.Lineno();
        }

        public FunctionCallExpression(string name, List<Expression> @params)
        {
            this.stringValue = name;
            this.@params = @params;
            this.lineNo = ScriptParser.tokenizer.Lineno();
        }

        public virtual List<Expression> GetParams()
        {
            return this.@params;
        }

        /// <summary>
        /// Assembles the source code.
        /// </summary>
        public virtual void Assemble()
        {
            this.Assemble(new List<Register>());
        }

        /// <summary>
        /// Assembles the source code.
        /// </summary>
        public virtual void Assemble(Register var)
        {
            List<Register> vars = new List<Register>();
            vars.Add(var);
            this.Assemble(vars);
        }

        /// <summary>
        /// Assembles the source code.
        /// </summary>
        public virtual void Assemble(List<Register> vars)
        {
            FunctionInfo functionInfo = FunctionInfo.Find(this.stringValue, this.@params.Count, vars.Count);
            if (functionInfo == null)
                throw new NslException("Function \"" + this.stringValue + "\" not found that expects " + this.@params.Count + " parameters and returns " + vars.Count + " values", this.lineNo);
            List<Register> parentReturnVars = ReturnVarExpression.SetRegisters(vars);

            // Any variables used in the function that aren't used to call it must be pushed onto the stack.
            List<Expression> usedVarsList = new List<Expression>();
            foreach (Register param in functionInfo.GetUsedVars().Values())
            {
                if (Expression.FindRegister(CodeInfo.GetCurrent().GetUsedVars(), param) != null && Expression.FindRegister(this.@params, param) == null && Expression.FindRegister(vars, param) == null)
                {
                    ScriptParser.WriteLine("Push " + param);
                    usedVarsList.Add(param);
                }
            }


            // Function parameters.
            for (int i = this.params.size() - 1; i >= 0; i--)
            {
                Expression param = this.@params[i];
                if (param is AssembleExpression)
                {
                    Register varLeft;
                    if (vars.IsEmpty())
                        varLeft = RegisterList.GetCurrent().GetNext();
                    else
                        varLeft = RegisterList.GetCurrent()[vars[0].integerValue];
                    ((AssembleExpression)param).Assemble(varLeft);
                    ScriptParser.WriteLine("Push " + varLeft.ToString(true));
                }
                else
                {
                    ScriptParser.WriteLine("Push " + param);
                }
            }


            // Actual function call.
            ScriptParser.WriteLine("Call " + functionInfo.GetName());

            // Popping return values.
            if (vars.IsEmpty())
            {

                // Got to clear the stack.
                if (functionInfo.GetUsedVars().IsEmpty())
                {
                    Register var = RegisterList.GetCurrent().GetNext();
                    for (int i = 0; i < functionInfo.GetReturns(); i++)
                        ScriptParser.WriteLine("Pop " + var);
                    var.SetInUse(false);
                }
                else
                {
                    for (int i = 0; i < functionInfo.GetReturns(); i++)
                        ScriptParser.WriteLine("Pop " + functionInfo.GetUsedVars()[0].ToString());
                }
            }
            else
            {
                foreach (Expression var in vars)
                {
                    ScriptParser.WriteLine("Pop " + var.ToString());
                }
            }


            // Any variables used in the function that aren't used to call it must be popped back off the stack.
            for (int i = usedVarsList.size() - 1; i >= 0; i--)
                ScriptParser.WriteLine("Pop " + usedVarsList[i]);
            ReturnVarExpression.SetRegisters(parentReturnVars);
        }

        /// <summary>
        /// Assembles the source code.
        /// </summary>
        // Any variables used in the function that aren't used to call it must be pushed onto the stack.
        // Function parameters.
        // Actual function call.
        // Popping return values.
        // Not explicitly getting any return values.
        // Got to clear the stack.
        // Any variables used in the function that aren't used to call it must be popped back off the stack.
        public override string ToString()
        {
            string args = "";
            int argsCount = this.@params.Count;
            for (int i = 0; i < argsCount; i++)
            {
                if (i == argsCount - 1)
                    args += this.@params[i];
                else
                    args += this.@params[i] + ", ";
            }

            return this.stringValue + "(" + args + ")";
        }
    }
}