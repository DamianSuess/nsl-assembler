using Java.Io;

namespace Nsl
{
    /// <summary>
    ///  * @author Stuart
    /// </summary>
    public class Main
    {
        /// <param name="args">the command line arguments</param>
        public static void Main(String[] args)
        {
            string scriptPath = null;
            bool noPauseOnError = false;
            bool noMakeNSIS = false;
            foreach (string arg in args)
            {
                if (arg.EqualsIgnoreCase("/nomake"))
                {
                    noMakeNSIS = true;
                }
                else if (arg.EqualsIgnoreCase("/nopause"))
                {
                    noPauseOnError = true;
                }
                else
                {
                    scriptPath = arg.Trim();
                }
            }

            if (scriptPath == null || scriptPath.IsEmpty())
                ShowUsage();
            System.Exit(ScriptParser.Parse(scriptPath, noPauseOnError, noMakeNSIS));
        }

        /// <summary>
        /// Shows command line usage for the nsL assembler before exiting.
        /// </summary>
        private static void ShowUsage()
        {
            Console.WriteLine("Usage:");
            Console.WriteLine("  java -jar nsL.jar [Options] script.nsl");
            Console.WriteLine();
            Console.WriteLine("Options:");
            Console.WriteLine("  -n");
            System.Exit(1);
        }
    }
}