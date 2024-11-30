/*
 * PluginCallExpression.java
 */
using Java.Io;
using Java.Util;
using Nsl;

namespace Nsl.Expression
{
    public class PluginCallExpression : MultipleReturnValueAssembleExpression
    {
        private readonly List<Expression> params;
        private bool noUnload;
        public PluginCallExpression(string name) : this(name, Expression.MatchList())
        {
        }

        public PluginCallExpression(string name, List<Expression> @params)
        {
            this.stringValue = name;
            this.@params = @params;
            if (this.@params.Count > 0)
            {
                Expression param = this.@params[0];
                if (ExpressionType.IsBoolean(param) && param.booleanValue == true)
                    this.noUnload = true;
            }
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
            List<Register> parentReturnVars = ReturnVarExpression.SetRegisters(vars);

            // Plug-in function parameters.
            for (int i = this.params.size() - 1; i >= 0; i--)
            {
                Expression param = this.@params[i];
                if (param is AssembleExpression)
                {
                    Register varLeft = RegisterList.GetCurrent()[vars[0].integerValue];
                    ((AssembleExpression)param).Assemble(varLeft);
                    ScriptParser.WriteLine("Push " + varLeft);
                }
                else
                {
                    ScriptParser.WriteLine("Push " + param);
                }
            }


            // Actual plug-in function call.
            if (this.noUnload)
                ScriptParser.WriteLine(this.stringValue + " /NOUNLOAD");
            else
                ScriptParser.WriteLine(this.stringValue);

            // Popping return values.
            foreach (Expression var in vars)
            {
                ScriptParser.WriteLine("Pop " + var.ToString(true));
            }

            ReturnVarExpression.SetRegisters(parentReturnVars);
        }

        /// <summary>
        /// Assembles the source code.
        /// </summary>
        // Plug-in function parameters.
        // Actual plug-in function call.
        // Popping return values.
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