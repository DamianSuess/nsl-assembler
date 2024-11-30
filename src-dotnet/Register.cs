/*
 * Register.java
 */
using Nsl.Expression;

namespace Nsl
{
    public class Register : Expression
    {
        private RegisterType registerType;
        private string substitute;
        public Register(string name, int index, RegisterType registerType, bool inUse)
        {
            this.type = ExpressionType.Register;
            this.stringValue = name;
            this.integerValue = index;
            this.registerType = registerType;
            this.booleanValue = inUse;
            this.substitute = null;
        }

        public virtual int GetIndex()
        {
            return this.integerValue;
        }

        public virtual bool GetInUse()
        {
            return this.booleanValue;
        }

        public override void SetInUse(bool inUse)
        {
            this.booleanValue = inUse;
        }

        public virtual RegisterType GetRegisterType()
        {
            return this.registerType;
        }

        public virtual void Substitute(string value)
        {
            this.substitute = value;
            this.booleanValue = false;
        }

        public override string ToString()
        {
            return this.ToString(false);
        }

        public override string ToString(bool noQuote)
        {
            if (this.substitute != null)
            {
                string value = this.substitute;
                this.substitute = null;
                if (noQuote)
                    return value;
                return "\"" + value + "\"";
            }

            if (CodeInfo.GetCurrent() != null)
                CodeInfo.GetCurrent().AddUsedVar(this);
            return this.stringValue;
        }
    }
}