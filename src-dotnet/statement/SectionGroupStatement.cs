/*
 * SectionGroupStatement.java
 */
using Java.Io;
using Java.Util;
using Nsl;

namespace Nsl.Statement
{
    public class SectionGroupStatement : Statement
    {
        private readonly bool uninstall;
        private readonly string name;
        private readonly Expression description;
        private readonly Expression expanded;
        private readonly Expression bold;
        private readonly BlockStatement blockStatement;
        /// <summary>
        /// Class constructor.
        /// </summary>
        public SectionGroupStatement()
        {

            // We can't have a section within a function or section.
            if (!ScriptParser.InGlobalContext())
                throw new NslContextException(EnumSet.Of(NslContext.Global), "section group");

            // Section group name.
            this.name = ScriptParser.tokenizer.MatchAWord("a section group name");

            // Section arguments.
            List<Expression> paramsList = Expression.MatchList();
            int paramsCount = paramsList.Count;
            if (paramsCount > 3)
                throw new NslArgumentException(name, 0, 3);

            // Section group description.
            if (paramsCount > 0)
            {
                this.description = paramsList[0];
                if (!ExpressionType.IsString(this.description))
                    throw new NslArgumentException(name, 1, ExpressionType.String);

                // Expanded by default?
                if (paramsCount > 1)
                {
                    this.expanded = paramsList[1];
                    if (!ExpressionType.IsBoolean(this.expanded))
                        throw new NslArgumentException(name, 2, ExpressionType.Boolean);

                    // Bold?
                    if (paramsCount > 2)
                    {
                        this.bold = paramsList[2];
                        if (!ExpressionType.IsBoolean(this.bold))
                            throw new NslArgumentException(name, 3, ExpressionType.Boolean);
                    }
                    else
                    {
                        this.bold = null;
                    }
                }
                else
                {
                    this.expanded = null;
                    this.bold = null;
                }
            }
            else
            {
                this.description = null;
                this.expanded = null;
                this.bold = null;
            }

            this.blockStatement = new BlockStatement();
            this.uninstall = Scope.InUninstaller();
        }

        /// <summary>
        /// Class constructor.
        /// </summary>
        // We can't have a section within a function or section.
        // Section group name.
        // Section arguments.
        // Section group description.
        // Expanded by default?
        // Bold?
        /// <summary>
        /// Assembles the source code.
        /// </summary>
        public override void Assemble()
        {
            string prefix = "";
            if (this.uninstall)
                prefix += "un.";
            if (this.expanded != null)
            {
                AssembleExpression.AssembleIfRequired(this.expanded);
            }

            if (this.bold != null)
            {
                AssembleExpression.AssembleIfRequired(this.bold);
                if (this.bold.GetBooleanValue() == true)
                    prefix += "!";
            }

            if (this.description == null)
                ScriptParser.WriteLine("SectionGroup \"" + prefix + "\" " + this.name);
            else
                ScriptParser.WriteLine("SectionGroup " + (this.expanded != null && this.expanded.GetBooleanValue() == true ? "/e " : "") + "\"" + prefix + this.description.ToString(true) + "\" " + this.name);
            this.blockStatement.Assemble();
            ScriptParser.WriteLine("SectionGroupEnd");
        }
    }
}