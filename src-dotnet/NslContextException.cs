/*
 * NslContextException.java
 */
using Java.Util;

namespace Nsl
{
    public class NslContextException : NslException
    {
        public NslContextException(EnumSet<NslContext> context, string function) : base(String.Format("\"%s\" can only be used in a %s context", function, Translate(context)), true)
        {
        }

        private static string Translate(EnumSet<NslContext> context)
        {
            List<string> partsList = new List<string>();
            if (context.Contains(NslContext.Global))
                partsList.Add("global");
            if (context.Contains(NslContext.Function))
                partsList.Add("function");
            if (context.Contains(NslContext.Section))
                partsList.Add("section");
            if (context.Contains(NslContext.PageEx))
                partsList.Add("page block (PageEx)");
            int count = partsList.Count;
            if (count == 0)
                return "??";
            string value = partsList[0];
            for (int i = 1; i < count; i++)
            {
                if (i == count - 1)
                    value += " or " + partsList[i];
                else
                    value += ", " + partsList[i];
            }

            return value;
        }
    }
}