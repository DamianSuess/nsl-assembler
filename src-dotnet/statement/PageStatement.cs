/*
 * PageStatement.java
 */
using Java.Io;
using Java.Util;
using Nsl;

namespace Nsl.Statement
{
    public class PageStatement : Statement
    {
        private readonly string pageName;
        private readonly Expression function1, function2, function3OrCaption, enableCancel;
        private readonly PageExInfo pageExInfo;
        private readonly BlockStatement pageEx;
        private readonly bool uninstall;
        /// <summary>
        /// Class constructor.
        /// </summary>
        public PageStatement()
        {

            // Must be outside a section or function.
            if (!ScriptParser.InGlobalContext())
                throw new NslContextException(EnumSet.Of(NslContext.Global), "page");

            // Page name.
            this.pageName = ScriptParser.tokenizer.MatchAWord("a page name");

            // Validate the page name.
            if (!this.pageName.Equals("Custom") && !this.pageName.Equals("UninstConfirm") && !this.pageName.Equals("License") && !this.pageName.Equals("Components") && !this.pageName.Equals("Directory") && !this.pageName.Equals("InstFiles"))
                throw new NslException("\"" + this.pageName + "\" is not a valid page name", true);

            // Additional options.
            List<Expression> paramsList = Expression.MatchList();
            int paramsCount = paramsList.Count;
            if (paramsCount > 4)
                throw new NslArgumentException("page", 0, 4);
            if (paramsCount > 0)
            {
                this.function1 = paramsList[0];
                if (!ExpressionType.IsString(this.function1))
                    throw new NslArgumentException("page", 1, ExpressionType.String);
            }
            else
                this.function1 = null;
            if (paramsCount > 1)
            {
                this.function2 = paramsList[1];
                if (!ExpressionType.IsString(this.function2))
                    throw new NslArgumentException("page", 2, ExpressionType.String);
            }
            else
                this.function2 = null;
            if (paramsCount > 2)
            {
                this.function3OrCaption = paramsList[2];
                if (!ExpressionType.IsString(this.function3OrCaption))
                    throw new NslArgumentException("page", 3, ExpressionType.String);
            }
            else
                this.function3OrCaption = null;
            if (paramsCount > 3)
            {
                this.enableCancel = paramsList[3];
                if (!ExpressionType.IsBoolean(this.enableCancel))
                    throw new NslArgumentException("page", 4, ExpressionType.Boolean);
            }
            else
                this.enableCancel = null;

            // PageEx block.
            if (ScriptParser.tokenizer.TokenIs('{'))
            {
                this.pageExInfo = new PageExInfo();
                PageExInfo.SetCurrent(this.pageExInfo);
                this.pageEx = new BlockStatement();
                PageExInfo.SetCurrent(null);
            }
            else
            {
                ScriptParser.tokenizer.MatchEolOrDie();
                this.pageExInfo = null;
                this.pageEx = null;
            }

            this.uninstall = Scope.InUninstaller();
        }

        /// <summary>
        /// Class constructor.
        /// </summary>
        // Must be outside a section or function.
        // Page name.
        // Validate the page name.
        // Additional options.
        // PageEx block.
        /// <summary>
        /// Assembles the source code.
        /// </summary>
        public override void Assemble()
        {
            if (this.pageEx == null)
            {
                string write;
                if (this.uninstall)
                    write = "UninstPage " + this.pageName;
                else
                    write = "Page " + this.pageName;
                if (this.function1 != null)
                {
                    write += " " + this.function1;
                    if (this.function2 != null)
                    {
                        write += " " + this.function2;
                        if (this.function3OrCaption != null)
                        {
                            write += " " + this.function3OrCaption;
                            if (this.enableCancel != null && this.enableCancel.GetBooleanValue() == true)
                            {
                                write += " /ENABLECANCEL";
                            }
                        }
                    }
                }

                ScriptParser.WriteLine(write);
            }
            else
            {
                if (this.uninstall)
                    ScriptParser.WriteLine("PageEx un." + this.pageName);
                else
                    ScriptParser.WriteLine("PageEx " + this.pageName);
                if (this.function1 != null)
                {
                    string write = "PageCallbacks " + this.function1;
                    if (this.function2 != null)
                    {
                        write += " " + this.function2;
                        if (!this.pageName.Equals("Custom") && this.function3OrCaption != null)
                        {
                            write += " " + this.function3OrCaption;
                        }
                    }

                    ScriptParser.WriteLine(write);
                }

                PageExInfo.SetCurrent(this.pageExInfo);
                this.pageEx.Assemble();
                PageExInfo.SetCurrent(null);
                ScriptParser.WriteLine("PageExEnd");
            }
        }
    }
}