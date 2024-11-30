/*
 * NslExpectedException.java
 */
using Java.Io;

namespace Nsl
{
    public class NslExpectedException : NslException
    {
        public NslExpectedException(string expected) : base(String.Format("Expected %s, but found %s", expected, ScriptParser.tokenizer.ttype == StreamTokenizer.TT_EOF ? "the end of the file" : ScriptParser.tokenizer.TokenIsWord() ? "\"" + ScriptParser.tokenizer.sval + "\"" : ScriptParser.tokenizer.TokenIsNumber() ? "\"" + (int)ScriptParser.tokenizer.nval + "\"" : ScriptParser.tokenizer.TokenIsString() ? "\"" + ScriptParser.tokenizer.sval + "\"" : "\"" + (char)ScriptParser.tokenizer.ttype + "\""), true)
        {
        }

        public NslExpectedException(string expected, string found) : base(String.Format("Expected %s, but found %s", expected, found), true)
        {
        }
    }
}