/*
 * MacroList.java
 */
using Java.Util;

namespace Nsl.Preprocessor
{
    public class MacroList
    {
        private readonly List<Macro> macros;
        private static readonly MacroList current = new MacroList();
        public static MacroList GetCurrent()
        {
            return current;
        }

        /// <summary>
        /// Class constructor.
        /// </summary>
        public MacroList()
        {
            this.macros = new List<Macro>();
        }

        public virtual Macro Get(string name, int paramCount)
        {
            foreach (Macro macro in this.macros)
                if (macro.GetName().Equals(name) && macro.GetParamCount() == paramCount)
                    return macro;
            return null;
        }

        public virtual List<Macro> Get(string name)
        {
            List<Macro> macrosList = new List<Macro>();
            foreach (Macro macro in this.macros)
                if (macro.GetName().Equals(name))
                    macrosList.Add(macro);
            return macrosList;
        }

        public virtual bool Add(Macro macro)
        {
            Macro existing = this.Get(macro.GetName(), macro.GetParamCount());
            if (existing == null)
            {
                this.macros.Add(macro);
                return true;
            }

            return false;
        }
    }
}