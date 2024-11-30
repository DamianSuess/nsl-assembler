/*
 * IncludeDirective.java
 */
using Java.Io;
using Nsl;
using Nsl.Statement;

namespace Nsl.Preprocessor
{
    public class IncludeDirective : Statement
    {
        private readonly StatementList statementList;
        /// <summary>
        /// Class constructor.
        /// </summary>
        public IncludeDirective()
        {
            if (!ScriptParser.tokenizer.TokenIsString())
                throw new NslExpectedException("a file path to include");
            string path = ScriptParser.tokenizer.sval;
            Reader reader;
            try
            {
                reader = new FileReader(path);
            }
            catch (IOException ex)
            {
                throw new NslException(ex.GetMessage(), true);
            }

            ScriptParser.PushTokenizer(new Tokenizer(reader, "included script \"" + path + "\""));
            ScriptParser.tokenizer.SetAutoPop(false);
            this.statementList = StatementList.Match();
            ScriptParser.PopTokenizer();
            ScriptParser.tokenizer.TokenNext();
        }

        /// <summary>
        /// Class constructor.
        /// </summary>
        public override void Assemble()
        {
            this.statementList.Assemble();
        }
    }
}