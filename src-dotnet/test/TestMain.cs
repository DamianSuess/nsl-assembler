using Java.Io;
using Nsl;

namespace Nsl.Test
{
    /// <summary>
    ///  * @author Stuart
    /// </summary>
    public class TestMain
    {
        /// <param name="args">the command line arguments</param>
        public static void Main(String[] args)
        {
            if (args.length == 0)
                return;
            try
            {
                FileReader fileReader = new FileReader(args[0]);
                Tokenizer tokenizer = new Tokenizer(fileReader, args[0]);
                while (tokenizer.NextToken() != StreamTokenizer.TT_EOF)
                {
                    if (tokenizer.ttype == StreamTokenizer.TT_WORD)
                        Console.WriteLine("word = " + tokenizer.sval);
                    else if (tokenizer.ttype == StreamTokenizer.TT_NUMBER)
                        Console.WriteLine("number = " + tokenizer.nval);
                    else if (tokenizer.ttype == '"')
                        Console.WriteLine("string = " + tokenizer.sval);
                    else
                        Console.WriteLine("char = " + (char)tokenizer.ttype);
                }

                fileReader.Dispose();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
            }
        }
    }
}