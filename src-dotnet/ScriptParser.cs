/*
 * ScriptParser.java
 */
using Java.Io;
using Nsl;
using Java.Util;

namespace Nsl
{
    public class ScriptParser
    {
        private ScriptParser()
        {
        }

        public static Tokenizer tokenizer;
        public static Stack<Tokenizer> tokenizers = new Stack<Tokenizer>();
        private static Writer writer;
        private static string scriptPath;
        public static int Parse(string path, bool noPauseOnError, bool noMakeNSIS)
        {
            int exitCode = 0;
            scriptPath = path;
            Statement statement = null;
            PrintWriter stdout = new PrintWriter(Console, true);
            PrintWriter stderr = new PrintWriter(System.err, true);
            try
            {
                PushTokenizer(new Tokenizer(new FileReader(path), "script \"" + path + "\""));
                try
                {
                    statement = StatementList.Match();
                }
                catch (NslException ex)
                {
                    if (ex.GetInner() != null)
                        stderr.Println(ex.GetInner().ToString());
                    else
                        stderr.Println(ex.GetMessage());
                    if (!noPauseOnError)
                        System.@in.Read();
                }

                tokenizer.GetReader().Dispose();
            }
            catch (IOException ex)
            {
                stderr.Println(ex);
                if (!noPauseOnError)
                    System.@in.Read();
            }

            if (statement != null)
            {
                File outputFile = new File(GetOutputPath(path));
                try
                {
                    writer = new OutputStreamWriter(new FileOutputStream(outputFile));

                    // Insert any Var instructions at the top.
                    RegisterList.GetCurrent().DefineVars();

                    // Write the NSIS script.
                    statement.Assemble();

                    // Insert .onInit/un.onInit with global assignments if required.
                    bool anyGlobalAssignments = !Statement.GetGlobal().IsEmpty();
                    bool anyGlobalUninstallerAssignments = !Statement.GetGlobalUninstaller().IsEmpty();
                    if (anyGlobalAssignments || anyGlobalUninstallerAssignments)
                    {
                        int onInitDefined = FunctionInfo.IsOnInitDefined();
                        if (anyGlobalAssignments && (onInitDefined & 1) == 0)
                        {
                            WriteLine("Function .onInit");
                            foreach (Statement globalStatement in Statement.GetGlobal())
                                globalStatement.Assemble();
                            WriteLine("FunctionEnd");
                        }

                        if (anyGlobalUninstallerAssignments && (onInitDefined & 2) == 0)
                        {
                            WriteLine("Function un.onInit");
                            foreach (Statement globalStatement in Statement.GetGlobalUninstaller())
                                globalStatement.Assemble();
                            WriteLine("FunctionEnd");
                        }
                    }

                    writer.Dispose();
                    writer = null;
                }
                catch (Exception ex)
                {
                    if (writer != null)
                    {
                        try
                        {
                            writer.Dispose();
                        }
                        finally
                        {
                        }

                        outputFile.Delete();
                    }

                    exitCode = 2;
                    if (ex is NslException)
                        stderr.Println(ex.GetMessage());
                    else
                        ex.PrintStackTrace(stderr);
                    if (!noPauseOnError)
                        System.@in.Read();
                }


                // Successfully written NSIS script file.
                if (exitCode == 0)
                {
                    stdout.Println();
                    stdout.Println("Wrote \"" + outputFile.GetCanonicalPath() + "\".");
                    stdout.Println("Assembled successfully.");
                    stdout.Println();

                    // Build the NSIS script.
                    if (!noMakeNSIS)
                    {
                        File makensisw = new File("..\\makensisw.exe");
                        if (makensisw.Exists())
                        {
                            Runtime.GetRuntime().Exec("\"" + makensisw.GetAbsolutePath() + "\" \"" + outputFile.GetCanonicalPath() + "\"");
                        }
                        else
                        {
                            stderr.Println("Unable to compile \"" + outputFile.GetCanonicalPath() + "\":");
                            stderr.Println("  \"makensisw.exe\" not found in \"" + (new File(makensisw.GetParent())).GetCanonicalPath() + "\".");
                            if (!noPauseOnError)
                                System.@in.Read();
                        }
                    }
                }
            }

            return exitCode;
        }

        public static void PushTokenizer(Tokenizer push)
        {
            if (tokenizer != null)
                tokenizers.Push(tokenizer);
            tokenizer = push;
            tokenizer.TokenNext("a token");
        }

        public static Tokenizer PopTokenizer()
        {
            if (tokenizers.IsEmpty())
                return null;
            try
            {
                tokenizer.GetReader().Dispose();
            }
            catch (IOException ex)
            {
                throw new NslException(ex.GetMessage(), true);
            }

            tokenizer = tokenizers.Pop();
            return tokenizer;
        }

        public static bool InGlobalContext()
        {
            return !FunctionInfo.In() && !SectionInfo.In() && !PageExInfo.In();
        }

        public static void Write(string text)
        {
            writer.Write(text);
        }

        public static void WriteLine(string line)
        {
            writer.Write(line + "\r\n");
        }

        public static void WriteLine()
        {
            writer.Write("\r\n");
        }

        public static string GetScriptPath()
        {
            return scriptPath;
        }

        private static string GetOutputPath(string scriptPath)
        {
            int i = scriptPath.LastIndexOf('.');
            if (i == -1)
                return scriptPath + ".nsi";
            return scriptPath.Substring(0, i) + ".nsi";
        }
    }
}