/*
 * DefineList.java
 */
using Java.Util;
using Nsl.Expression;

namespace Nsl.Preprocessor
{
    public class DefineList
    {
        private readonly HashMap<string, Expression> constants;
        private int count;
        private static DefineList current = new DefineList();
        public static DefineList GetCurrent()
        {
            return current;
        }

        /// <summary>
        /// Class constructor.
        /// </summary>
        public DefineList()
        {
            this.constants = new HashMap<string, Expression>();
            this.count = 0;
        }

        public virtual int GetCount()
        {
            return this.count;
        }

        public virtual Expression Get(string name)
        {
            return this.constants[name];
        }

        public virtual HashSet<string> GetNames()
        {
            return this.constants.KeySet();
        }

        public virtual bool Add(string name, Expression value)
        {
            Expression oldValue = this.constants.Put(name, value);
            if (oldValue == null)
            {
                this.count++;
                return true;
            }

            return false;
        }

        public virtual bool Remove(string name)
        {
            return this.constants.Remove(name) != null;
        }

        public static Expression Lookup(string name)
        {
            Expression value = null;
            if (MacroEvaluated.GetCurrent() != null)
                value = MacroEvaluated.GetCurrent().GetDefineList()[name];
            if (value == null)
                value = DefineList.GetCurrent()[name];
            return value;
        }
    }
}