/*
 * Macro.java
 */
using Java.Io;
using Java.Util;
using Nsl;

namespace Nsl.Preprocessor
{
    public class Macro
    {
        private readonly string name;
        private readonly String[] params;
        private readonly int paramCount;
        private readonly int lineNo;
        private readonly string contents;
        public Macro(string name, String[] @params, int lineNo, string contents)
        {
            this.name = name;
            this.@params = @params;
            this.paramCount = @params.length;
            this.lineNo = lineNo;
            this.contents = contents;
        }

        public virtual string GetName()
        {
            return this.name;
        }

        public virtual String[] GetParams()
        {
            return this.@params;
        }

        public virtual int GetParamCount()
        {
            return this.paramCount;
        }

        public virtual int GetLineNo()
        {
            return this.lineNo;
        }

        public virtual string GetContents()
        {
            return this.contents;
        }

        public virtual Expression Evaluate(List<Expression> paramValues, int returns)
        {
            int currentLineNo = ScriptParser.tokenizer.Lineno();
            ScriptParser.PushTokenizer(new Tokenizer(new StringReader(this.contents), "macro \"" + this.name + "\""));
            ScriptParser.tokenizer.SetAutoPop(false);

            // A local constants list for the arguments for this macro.
            DefineList constantList = new DefineList();
            if (paramValues != null)
                for (int i = 0; i < this.paramCount; i++)
                    constantList.Add(this.@params[i], paramValues[i]);

            // Add the number of return values to the constants list.
            constantList.Add("Returns", Expression.FromInteger(returns));
            MacroEvaluated macroEvaluated = new MacroEvaluated(this.name, constantList);
            macroEvaluated.Evaluate();
            if (returns > 0)
            {
                if (macroEvaluated.GetReturnValues() == null)
                {
                    throw new NslException("Insertion of macro \"" + this.name + "\" requires " + returns + (returns == 1 ? " return value" : " return values") + ", but no #return directive used", true);
                }
                else if (!macroEvaluated.GetReturnValues().IsEmpty())
                {
                    int returnValues = macroEvaluated.GetReturnValues().Count;
                    if (returnValues != returns)
                        throw new NslException("Insertion of macro \"" + this.name + "\" requires " + returns + (returns == 1 ? " return value" : " return values") + ", but " + returnValues + (returnValues == 1 ? " return value was" : " return values were") + " returned", currentLineNo);
                }
            }

            ScriptParser.PopTokenizer();

            // We add the contents after the current statement. This is so the return
            // registers list is known throughout the macro (i.e. for #nsis).
            //StatementList.getCurrent().addQueued(macroContents);
            // #nsis directive was used in the macro. Return the evaluated macro so that
            // it can tell the #nsis directive what the return registers are.
            if (macroEvaluated.GetReturnValues().IsEmpty())
                return macroEvaluated;

            // Caller requires a single return value. Return that value.
            if (returns == 1)
            {
                StatementList.GetCurrent().Add(new AssignmentStatement(new AssignmentExpression(-1, macroEvaluated)));
                return macroEvaluated.GetReturnValues()[0];
            }


            // Return the evaluated macro which handles multiple return values.
            if (returns > 1)
                return macroEvaluated;
            return new NullAssembleExpression();
        }

        public virtual string ToString()
        {
            if (this.paramCount < 0)
                return this.name;
            string call = this.name + '(';
            for (int i = 0; i < this.paramCount; i++)
            {
                call += this.@params[i];
                if (i != this.paramCount - 1)
                    call += ", ";
            }

            return call + ')';
        }
    }
}