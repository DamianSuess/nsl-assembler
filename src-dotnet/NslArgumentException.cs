/*
 * NslArgumentException.java
 */
using Nsl.Expression;

namespace Nsl
{
    public class NslArgumentException : NslException
    {
        public NslArgumentException(string function, int paramNumber, string valueGiven) : base(valueGiven + " is not a valid value for the " + TranslateParamNumber(paramNumber) + " parameter of \"" + function + "\"", true)
        {
        }

        public NslArgumentException(string function, int paramNumber, ExpressionType expectedType) : base("\"" + function + "\" expects " + expectedType + " for its " + TranslateParamNumber(paramNumber) + " parameter", true)
        {
        }

        public NslArgumentException(string function, int paramNumber, ExpressionType expectedType, ExpressionType orExpectedType) : base("\"" + function + "\" expects " + expectedType + " or " + orExpectedType + " for its " + TranslateParamNumber(paramNumber) + " parameter", true)
        {
        }

        public NslArgumentException(string function, int paramNumber, bool mustBeLiteral) : base("\"" + function + "\" expects " + (mustBeLiteral ? "a literal expression" : "a non-literal expression") + " for its " + TranslateParamNumber(paramNumber) + " parameter", true)
        {
        }

        public NslArgumentException(string function, int @params) : base("\"" + function + "\" expects " + @params + " parameter(s)", true)
        {
        }

        public NslArgumentException(string function, int paramCountFrom, int paramCountTo) : base("\"" + function + "\" expects " + paramCountFrom + (paramCountTo == 999 ? " or more" : " to " + paramCountTo) + " parameters", true)
        {
        }

        public NslArgumentException(string function, int paramCountFrom, int paramCountTo, int paramsGiven) : base("\"" + function + "\" expects " + paramCountFrom + (paramCountTo == 999 ? " or more" : " to " + paramCountTo) + " parameters, but " + paramsGiven + " parameter(s) were given", true)
        {
        }

        private static string TranslateParamNumber(int paramNumber)
        {
            if (paramNumber < 4 || paramNumber > 20)
            {
                string s = Integer.ToString(paramNumber);
                char last = s.CharAt(s.Length() - 1);
                switch (last)
                {
                    case '1':
                        return paramNumber + "st";
                    case '2':
                        return paramNumber + "nd";
                    case '3':
                        return paramNumber + "rd";
                }
            }

            return paramNumber + "th";
        }
    }
}