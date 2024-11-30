/*
 * NslReturnValueException.java
 */
namespace Nsl
{
    public class NslReturnValueException : NslException
    {
        public NslReturnValueException(string function) : base("The \"" + function + "\" instruction does not return a value", true)
        {
        }

        public NslReturnValueException(string function, int returns) : base("The \"" + function + "\" instruction only returns " + returns + (returns == 1 ? " value" : " values"), true)
        {
        }

        public NslReturnValueException(string function, int returns, int orReturns) : base("The \"" + function + "\" instruction can only return " + returns + " or " + orReturns + (orReturns == 1 ? " value" : " values"), true)
        {
        }
    }
}