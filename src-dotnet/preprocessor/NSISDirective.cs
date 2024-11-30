/*
 * NSISDirective.java
 */
using Java.Io;
using Java.Util;
using Nsl;
using Nsl.Expression;
using Nsl.Statement;

namespace Nsl.Preprocessor
{
    public class NSISDirective : Statement
    {
        private readonly string nsis;
        private readonly MacroEvaluated macroEvaluated;
        /// <summary>
        /// Class constructor.
        /// </summary>
        public NSISDirective()
        {
            this.nsis = ScriptParser.tokenizer.ReadUntil("#nsisend");
            ScriptParser.tokenizer.TokenNext();
            this.macroEvaluated = MacroEvaluated.GetCurrent();
            if (this.macroEvaluated != null)
                this.macroEvaluated.SetReturnValues(new List<Expression>());
        }

        /// <summary>
        /// Class constructor.
        /// </summary>
        public override void Assemble()
        {
            if (this.macroEvaluated != null)
            {
                foreach (string name in this.macroEvaluated.GetDefineList().GetNames())
                    ScriptParser.WriteLine("!define " + name + " " + this.macroEvaluated.GetDefineList()[name]);
                if (this.macroEvaluated.GetReturnRegisters() != null)
                {
                    int returnRegisterCount = macroEvaluated.GetReturnRegisters().Count;
                    for (int i = 0; i < returnRegisterCount; i++)
                        ScriptParser.WriteLine("!define ReturnVar" + (i + 1) + " " + this.macroEvaluated.GetReturnRegisters()[i]);
                }
            }

            ScriptParser.WriteLine(this.nsis);
            if (this.macroEvaluated != null)
            {
                foreach (string name in this.macroEvaluated.GetDefineList().GetNames())
                    ScriptParser.WriteLine("!undef " + name);
                if (this.macroEvaluated.GetReturnRegisters() != null)
                {
                    int returnRegisterCount = this.macroEvaluated.GetReturnRegisters().Count;
                    for (int i = 0; i < returnRegisterCount; i++)
                        ScriptParser.WriteLine("!undef ReturnVar" + (i + 1));
                }
            }
        }
    }
}