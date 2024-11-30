/*
 * SetCtlColorsInstruction.java
 */
using Java.Io;
using Java.Util;
using Nsl;

namespace Nsl.Instruction
{
    /// <remarks>@authorStuart</remarks>
    public class SetCtlColorsInstruction : AssembleExpression
    {
        public static readonly string name = "SetCtlColors";
        private readonly Expression hWnd;
        private readonly Expression branding;
        private readonly Expression textColor;
        private readonly Expression bgColor;
        public SetCtlColorsInstruction(int returns)
        {
            if (!SectionInfo.In() && !FunctionInfo.In())
                throw new NslContextException(EnumSet.Of(NslContext.Section, NslContext.Function), name);
            if (returns > 0)
                throw new NslReturnValueException(name);
            List<Expression> paramsList = Expression.MatchList();
            int paramsCount = paramsList.Count;
            if (paramsCount < 2 || paramsCount > 4)
                throw new NslArgumentException(name, 2, 4);
            this.hWnd = paramsList[0];
            Expression textColorOrBranding = paramsList[1];
            if (ExpressionType.IsBoolean(textColorOrBranding))
            {
                this.branding = textColorOrBranding;
                if (paramsCount > 2)
                {
                    this.bgColor = paramsList[2];
                    if (!ExpressionType.IsString(this.bgColor))
                        throw new NslArgumentException(name, 3, ExpressionType.String);
                    if (paramsCount > 3)
                    {
                        this.textColor = paramsList[3];
                        if (!ExpressionType.IsString(this.textColor))
                            throw new NslArgumentException(name, 4, ExpressionType.String);
                    }
                    else
                    {
                        this.textColor = null;
                    }
                }
                else
                {
                    this.bgColor = null;
                    this.textColor = null;
                }
            }
            else
            {
                this.branding = null;
                this.textColor = textColorOrBranding;
                if (!ExpressionType.IsString(this.textColor))
                    throw new NslArgumentException(name, 2, ExpressionType.String);
                if (paramsCount > 2)
                {
                    this.bgColor = paramsList[2];
                    if (!ExpressionType.IsString(this.bgColor))
                        throw new NslArgumentException(name, 3, ExpressionType.String);
                }
                else
                {
                    this.bgColor = null;
                }
            }
        }

        /// <summary>
        /// Assembles the source code.
        /// </summary>
        public override void Assemble()
        {
            throw new NotSupportedException("Not supported.");
        }

        /// <summary>
        /// Assembles the source code.
        /// </summary>
        public override void Assemble(Register var)
        {
            Expression varOrFile = AssembleExpression.GetRegisterOrExpression(this.hWnd);
            string write = name + " " + var + " " + varOrFile;
            if (this.branding != null)
            {
                AssembleExpression.AssembleIfRequired(this.branding);
                if (this.branding.GetBooleanValue() == true)
                    write += " /BRANDING";
            }

            if (this.textColor != null)
            {
                AssembleExpression.AssembleIfRequired(this.textColor);
                write += " " + this.textColor;
                if (this.bgColor != null)
                {
                    AssembleExpression.AssembleIfRequired(this.bgColor);
                    write += " " + this.bgColor;
                }
            }

            ScriptParser.WriteLine(write);
            varOrFile.SetInUse(false);
        }
    }
}