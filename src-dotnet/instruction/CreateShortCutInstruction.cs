/*
 * CreateShortCutInstruction.java
 */
using Java.Io;
using Java.Util;
using Nsl;

namespace Nsl.Instruction
{
    /// <remarks>@authorStuart</remarks>
    public class CreateShortCutInstruction : AssembleExpression
    {
        public static readonly string name = "CreateShortCut";
        private readonly Expression path;
        private readonly Expression target;
        private readonly Expression parameters;
        private readonly Expression iconFile;
        private readonly Expression iconIndex;
        private readonly Expression startOptions;
        private readonly Expression keyboardShortcut;
        private readonly Expression description;
        public CreateShortCutInstruction(int returns)
        {
            if (!SectionInfo.In() && !FunctionInfo.In())
                throw new NslContextException(EnumSet.Of(NslContext.Section, NslContext.Function), name);
            if (returns > 0)
                throw new NslReturnValueException(name);
            List<Expression> paramsList = Expression.MatchList();
            int paramsCount = paramsList.Count;
            if (paramsCount < 2 || paramsCount > 8)
                throw new NslArgumentException(name, 1);
            this.path = paramsList[0];
            this.target = paramsList[1];
            if (paramsCount > 2)
            {
                this.parameters = paramsList[2];
                if (paramsCount > 3)
                {
                    this.iconFile = paramsList[3];
                    if (paramsCount > 4)
                    {
                        this.iconIndex = paramsList[4];
                        if (paramsCount > 5)
                        {
                            this.startOptions = paramsList[5];
                            if (!ExpressionType.IsString(this.startOptions))
                                throw new NslArgumentException(name, 6, ExpressionType.String);
                            if (paramsCount > 6)
                            {
                                this.keyboardShortcut = paramsList[6];
                                if (!ExpressionType.IsString(this.keyboardShortcut))
                                    throw new NslArgumentException(name, 7, ExpressionType.String);
                                if (paramsCount > 7)
                                {
                                    this.description = paramsList[7];
                                }
                                else
                                    this.description = null;
                            }
                            else
                            {
                                this.keyboardShortcut = null;
                                this.description = null;
                            }
                        }
                        else
                        {
                            this.startOptions = null;
                            this.keyboardShortcut = null;
                            this.description = null;
                        }
                    }
                    else
                    {
                        this.iconIndex = null;
                        this.startOptions = null;
                        this.keyboardShortcut = null;
                        this.description = null;
                    }
                }
                else
                {
                    this.iconFile = null;
                    this.iconIndex = null;
                    this.startOptions = null;
                    this.keyboardShortcut = null;
                    this.description = null;
                }
            }
            else
            {
                this.parameters = null;
                this.iconFile = null;
                this.iconIndex = null;
                this.startOptions = null;
                this.keyboardShortcut = null;
                this.description = null;
            }
        }

        /// <summary>
        /// Assembles the source code.
        /// </summary>
        public override void Assemble()
        {
            Expression varOrPath = AssembleExpression.GetRegisterOrExpression(this.path);
            Expression varOrTarget = AssembleExpression.GetRegisterOrExpression(this.target);
            string write = "";
            if (this.parameters != null)
            {
                AssembleExpression.AssembleIfRequired(this.parameters);
                write += " " + this.parameters;
                if (this.iconFile != null)
                {
                    AssembleExpression.AssembleIfRequired(this.iconFile);
                    write += " " + this.iconFile;
                    if (this.iconIndex != null)
                    {
                        AssembleExpression.AssembleIfRequired(this.iconIndex);
                        write += " " + this.iconIndex;
                        if (this.startOptions != null)
                        {
                            AssembleExpression.AssembleIfRequired(this.startOptions);
                            write += " " + this.startOptions;
                            if (this.keyboardShortcut != null)
                            {
                                AssembleExpression.AssembleIfRequired(this.keyboardShortcut);
                                write += " " + this.keyboardShortcut;
                                if (this.description != null)
                                {
                                    AssembleExpression.AssembleIfRequired(this.description);
                                    write += " " + this.description;
                                }
                            }
                        }
                    }
                }
            }

            ScriptParser.WriteLine(name + " " + varOrPath + " " + varOrTarget + write);
            varOrPath.SetInUse(false);
            varOrTarget.SetInUse(false);
        }

        /// <summary>
        /// Assembles the source code.
        /// </summary>
        public override void Assemble(Register var)
        {
            throw new NotSupportedException("Not supported.");
        }
    }
}