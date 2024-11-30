/*
 * FunctionStatement.java
 */
using Java.Io;
using Java.Util;
using Nsl;

namespace Nsl.Statement
{
    public class FunctionStatement : Statement
    {
        private readonly FunctionInfo current;
        private readonly List<Register> params;
        private readonly BlockStatement blockStatement;
        /// <summary>
        /// Class constructor.
        /// </summary>
        public FunctionStatement()
        {

            // We can't have a function within a function or section.
            if (!ScriptParser.InGlobalContext())
                throw new NslContextException(EnumSet.Of(NslContext.Global), "function");

            // Function name.
            string name;
            if (ScriptParser.tokenizer.Match('.'))
            {
                name = '.' + ScriptParser.tokenizer.MatchAWord("a function name");
                if (Scope.InUninstaller())
                    name = "un" + name;
            }
            else
            {
                name = ScriptParser.tokenizer.MatchAWord("a function name");
                if (Scope.InUninstaller())
                    name = "un." + name;
            }


            // Function call parameters.
            this.@params = new List<Register>();
            List<Expression> paramsList = Expression.MatchRegisterList();
            foreach (Expression ret in paramsList)
            {
                if (!ExpressionType.IsRegister(ret))
                    throw new NslException("Parameters in a function definition must be variables", true);
                Scope.GetCurrent().AddVar(ret.GetIntegerValue());
                this.@params.Add(RegisterList.GetCurrent()[ret.GetIntegerValue()]);
            }

            this.current = FunctionInfo.Create(name, this.@params);
            FunctionInfo.SetCurrent(this.current);
            this.blockStatement = new BlockStatement();
            FunctionInfo.SetCurrent(null);
            RegisterList.GetCurrent().SetAllInUse(false);

            // If the last statement is a return statement, tell it.
            Statement last = this.blockStatement.GetLast();
            if (last is ReturnStatement)
                ((ReturnStatement)last).SetLast(true);
            if (this.current.IsCallback())
            {
                if (name.EqualsIgnoreCase(".onMouseOverSection"))
                {
                    if (this.@params.Count != 1)
                        throw new NslException("Callback function \"" + name + "\" requires 1 parameter, $0", true);
                    Register var = RegisterList.GetCurrent()[this.@params[0].GetIntegerValue()];
                    if (var == null || !var.ToString().Equals("$0"))
                        throw new NslException("Callback function \"" + name + "\" requires 1 parameter, $0", true);
                }
                else
                {
                    if (!this.@params.IsEmpty())
                        throw new NslException("Callback function \"" + name + "\" requires 0 parameters", true);
                }

                if (this.current.GetReturns() > 0)
                    throw new NslException("Callback function \"" + name + "\" requires 0 return values", true);
            }
        }

        /// <summary>
        /// Class constructor.
        /// </summary>
        // We can't have a function within a function or section.
        // Function name.
        // Function call parameters.
        // If the last statement is a return statement, tell it.
        /// <summary>
        /// Assembles the source code.
        /// </summary>
        public override void Assemble()
        {
            ScriptParser.WriteLine("Function " + this.current.GetName());
            FunctionInfo.SetCurrent(this.current);

            // Assemble global assignments if we're in .onInit.
            if (this.current.GetName().EqualsIgnoreCase(".onInit"))
            {
                foreach (Statement statement in Statement.globalAssignmentStatements)
                    statement.Assemble();
            }


            // Assemble global uninstaller assignments if we're in un.onInit.
            if (this.current.GetName().EqualsIgnoreCase("un.onInit"))
            {
                foreach (Statement statement in Statement.globalUninstallerAssignmentStatements)
                    statement.Assemble();
            }

            foreach (Expression param in this.@params)
                ScriptParser.WriteLine("Pop " + param);
            this.blockStatement.Assemble();
            FunctionInfo.SetCurrent(null);
            LabelList.GetCurrent().Reset();
            ScriptParser.WriteLine("FunctionEnd");
        }
    }
}