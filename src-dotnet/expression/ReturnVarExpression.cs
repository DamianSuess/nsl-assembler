/*
 * MacroReturnVarExpression.java
 */
using Java.Util;
using Nsl;

namespace Nsl.Expression
{
    public class ReturnVarExpression : Expression
    {
        private static List<Register> current = null;
        public static List<Register> SetRegisters(List<Register> registers)
        {
            List<Register> parent = current;
            current = registers;
            return parent;
        }

        public static List<Register> SetRegisters(Register registers)
        {
            List<Register> parent = current;
            current = new List<Register>();
            current.Add(registers);
            return parent;
        }

        public static List<Register> GetRegisters()
        {
            return current;
        }

        public ReturnVarExpression(int registerNumber)
        {
            this.integerValue = registerNumber;
        }

        public override string ToString()
        {
            return this.ToString(false);
        }

        public override string ToString(bool noQuote)
        {
            if (current == null || current.IsEmpty())
                throw new NslException("Use of \"returnvar()\" where no return registers are being used", false);
            int registerCount = current.Count;
            if (this.integerValue < 1 || this.integerValue > registerCount)
                throw new NslException("A value of " + this.integerValue + " is out of range for \"returnvar()\" where " + registerCount + (registerCount == 1 ? " register is" : " registers are") + " in use", false);
            return current[this.integerValue - 1].ToString(noQuote);
        }
    }
}