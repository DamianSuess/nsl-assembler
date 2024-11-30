/*
 * NslException.java
 */
namespace Nsl
{
    public class NslException : Exception
    {
        private Exception inner;
        public NslException(Exception inner)
        {
            this.inner = inner;
        }

        public NslException(string message) : base(message + ".")
        {
        }

        public NslException(string message, bool includeLineNo) : base(GetParseStack(ScriptParser.tokenizer.Lineno(), true) + message + ".")
        {
        }

        public NslException(string message, int lineNo) : base(GetParseStack(lineNo, true) + message + ".")
        {
        }

        public virtual Exception GetInner()
        {
            return this.inner;
        }

        public static void PrintWarning(string message)
        {
            Console.WriteLine(GetParseStack(ScriptParser.tokenizer.Lineno(), false) + message + ".");
            Console.Flush();
        }

        private static string GetParseStack(int lineNo, bool isError)
        {
            string errorStack = "";
            foreach (Tokenizer tokenizer in ScriptParser.tokenizers)
                if (tokenizer.GetSource() != null)
                    errorStack += (isError ? "Error" : "Warning") + " in " + tokenizer.GetSource() + " on line " + tokenizer.Lineno() + ":\r\n  ";
            if (ScriptParser.tokenizer.GetSource() != null)
                errorStack += (isError ? "Error" : "Warning") + " in " + ScriptParser.tokenizer.GetSource() + " on line " + lineNo + ":\r\n  ";
            return errorStack;
        }
    }
}