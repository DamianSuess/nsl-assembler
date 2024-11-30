/*
 * SectionStatement.java
 */
using Java.Io;
using Java.Util;
using Nsl;

namespace Nsl.Statement
{
    public class SectionStatement : Statement
    {
        private readonly bool uninstall;
        private readonly SectionInfo current;
        private readonly string name;
        private readonly Expression description;
        private readonly Expression readOnly;
        private readonly Expression optional;
        private readonly Expression bold;
        private readonly List<Expression> sectionInList;
        private readonly BlockStatement blockStatement;
        /// <summary>
        /// Class constructor.
        /// </summary>
        public SectionStatement()
        {

            // We can't have a section within a function or section.
            if (!ScriptParser.InGlobalContext())
                throw new NslContextException(EnumSet.Of(NslContext.Global), "section");

            // Section name.
            this.name = ScriptParser.tokenizer.MatchAWord("a section name");

            // Section arguments.
            List<Expression> paramsList = Expression.MatchList();
            int paramsCount = paramsList.Count;
            this.sectionInList = new List<Expression>();
            int sectionInListStarts = 4;

            // Section description.
            if (paramsCount > 0)
            {
                this.description = paramsList[0];
                if (!ExpressionType.IsString(this.description))
                    throw new NslArgumentException(name, 1, ExpressionType.String);

                // Read only?
                if (paramsCount > 1)
                {
                    Expression readOnlyOrSectionIn = paramsList[1];
                    if (ExpressionType.IsBoolean(readOnlyOrSectionIn))
                    {
                        this.readOnly = readOnlyOrSectionIn;

                        // Optional?
                        if (paramsCount > 2)
                        {
                            Expression optionalOrSectionIn = paramsList[2];
                            if (ExpressionType.IsBoolean(optionalOrSectionIn))
                            {
                                this.optional = optionalOrSectionIn;

                                // Bold?
                                if (paramsCount > 3)
                                {
                                    Expression boldOrSectionIn = paramsList[3];
                                    if (ExpressionType.IsBoolean(boldOrSectionIn))
                                    {
                                        this.bold = boldOrSectionIn;
                                    }
                                    else
                                    {
                                        if (!ExpressionType.IsInteger(boldOrSectionIn))
                                            throw new NslArgumentException(name, 4, ExpressionType.Integer);
                                        this.sectionInList.Add(boldOrSectionIn);
                                        sectionInListStarts = 4;
                                        this.bold = null;
                                    }
                                }
                                else
                                {
                                    this.bold = null;
                                }
                            }
                            else
                            {
                                if (!ExpressionType.IsInteger(optionalOrSectionIn))
                                    throw new NslArgumentException(name, 3, ExpressionType.Integer);
                                this.sectionInList.Add(optionalOrSectionIn);
                                sectionInListStarts = 3;
                                this.optional = null;
                                this.bold = null;
                            }
                        }
                        else
                        {
                            this.optional = null;
                            this.bold = null;
                        }
                    }
                    else
                    {
                        if (!ExpressionType.IsInteger(readOnlyOrSectionIn))
                            throw new NslArgumentException(name, 2, ExpressionType.Integer);
                        this.sectionInList.Add(readOnlyOrSectionIn);
                        sectionInListStarts = 2;
                        this.readOnly = null;
                        this.optional = null;
                        this.bold = null;
                    }

                    for (int i = sectionInListStarts; i < paramsCount; i++)
                    {
                        Expression sectionIn = paramsList[i];
                        if (!ExpressionType.IsInteger(sectionIn))
                            throw new NslArgumentException(name, i + 1, ExpressionType.Integer);
                        this.sectionInList.Add(sectionIn);
                    }
                }
                else
                {
                    this.readOnly = null;
                    this.optional = null;
                    this.bold = null;
                }
            }
            else
            {
                this.description = null;
                this.readOnly = null;
                this.optional = null;
                this.bold = null;
            }

            this.current = new SectionInfo();
            SectionInfo.SetCurrent(this.current);
            this.blockStatement = new BlockStatement();
            SectionInfo.SetCurrent(null);
            RegisterList.GetCurrent().SetAllInUse(false);
            this.uninstall = Scope.InUninstaller();
        }

        /// <summary>
        /// Class constructor.
        /// </summary>
        // We can't have a section within a function or section.
        // Section name.
        // Section arguments.
        // Section description.
        // Read only?
        // Optional?
        // Bold?
        /// <summary>
        /// Assembles the source code.
        /// </summary>
        public override void Assemble()
        {
            string prefix = "";
            if (this.uninstall)
                prefix += "un.";
            if (this.readOnly != null)
            {
                AssembleExpression.AssembleIfRequired(this.readOnly);
            }

            if (this.optional != null)
            {
                AssembleExpression.AssembleIfRequired(this.optional);
            }

            if (this.bold != null)
            {
                AssembleExpression.AssembleIfRequired(this.bold);
                if (this.bold.GetBooleanValue() == true)
                    prefix += "!";
            }

            if (this.description == null)
                ScriptParser.WriteLine("Section \"" + prefix + "\" " + this.name);
            else
                ScriptParser.WriteLine("Section " + (this.optional != null && this.optional.GetBooleanValue() == true ? "/o " : "") + "\"" + prefix + this.description.ToString(true) + "\" " + this.name);
            SectionInfo.SetCurrent(this.current);
            if (this.readOnly != null && this.readOnly.GetBooleanValue() == true)
            {
                ScriptParser.WriteLine("SectionIn RO");
            }

            this.blockStatement.Assemble();
            SectionInfo.SetCurrent(null);
            LabelList.GetCurrent().Reset();
            ScriptParser.WriteLine("SectionEnd");
        }
    }
}