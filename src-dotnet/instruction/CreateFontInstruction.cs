/*
 * CreateFontInstruction.java
 */
using Java.Io;
using Java.Util;
using Nsl;

namespace Nsl.Instruction
{
    /// <remarks>@authorStuart</remarks>
    public class CreateFontInstruction : AssembleExpression
    {
        public static readonly string name = "CreateFont";
        private readonly Expression fontFace;
        private readonly Expression height;
        private readonly Expression weight;
        private readonly Expression italic;
        private readonly Expression underline;
        private readonly Expression strike;
        public CreateFontInstruction(int returns)
        {
            if (PageExInfo.In())
                throw new NslContextException(EnumSet.Of(NslContext.Section, NslContext.Function, NslContext.Global), name);
            if (returns != 1)
                throw new NslReturnValueException(name);
            List<Expression> paramsList = Expression.MatchList();
            int paramsCount = paramsList.Count;
            if (paramsCount < 1 || paramsCount > 6)
                throw new NslArgumentException(name, 1, 6);
            this.fontFace = paramsList[0];
            if (!ExpressionType.IsString(this.fontFace))
                throw new NslArgumentException(name, 1, ExpressionType.String);
            if (paramsCount > 1)
            {
                this.height = paramsList[1];
                if (!ExpressionType.IsInteger(this.height))
                    throw new NslArgumentException(name, 2, ExpressionType.Integer);
                if (paramsCount > 2)
                {
                    this.weight = paramsList[2];
                    if (!ExpressionType.IsInteger(this.weight))
                        throw new NslArgumentException(name, 3, ExpressionType.Integer);
                    if (paramsCount > 3)
                    {
                        this.italic = paramsList[3];
                        if (!ExpressionType.IsBoolean(this.italic))
                            throw new NslArgumentException(name, 4, ExpressionType.Boolean);
                        if (paramsCount > 4)
                        {
                            this.underline = paramsList[4];
                            if (!ExpressionType.IsBoolean(this.underline))
                                throw new NslArgumentException(name, 5, ExpressionType.Boolean);
                            if (paramsCount > 5)
                            {
                                this.strike = paramsList[5];
                                if (!ExpressionType.IsBoolean(this.strike))
                                    throw new NslArgumentException(name, 6, ExpressionType.Boolean);
                            }
                            else
                            {
                                this.strike = null;
                            }
                        }
                        else
                        {
                            this.underline = null;
                            this.strike = null;
                        }
                    }
                    else
                    {
                        this.italic = null;
                        this.underline = null;
                        this.strike = null;
                    }
                }
                else
                {
                    this.weight = null;
                    this.italic = null;
                    this.underline = null;
                    this.strike = null;
                }
            }
            else
            {
                this.height = null;
                this.weight = null;
                this.italic = null;
                this.underline = null;
                this.strike = null;
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
            AssembleExpression.AssembleIfRequired(this.fontFace);
            string write = name + " " + var + " " + this.fontFace;
            if (this.height != null)
            {
                AssembleExpression.AssembleIfRequired(this.height);
                write += " " + this.height;
                if (this.weight != null)
                {
                    AssembleExpression.AssembleIfRequired(this.weight);
                    write += " " + this.weight;
                    if (this.italic != null)
                    {
                        AssembleExpression.AssembleIfRequired(this.italic);
                        if (this.italic.GetBooleanValue() == true)
                            write += " /ITALIC";
                        if (this.underline != null)
                        {
                            AssembleExpression.AssembleIfRequired(this.underline);
                            if (this.underline.GetBooleanValue() == true)
                                write += " /UNDERLINE";
                            if (this.strike != null)
                            {
                                AssembleExpression.AssembleIfRequired(this.strike);
                                if (this.strike.GetBooleanValue() == true)
                                    write += " /STRIKE";
                            }
                        }
                    }
                }
            }

            ScriptParser.WriteLine(write);
        }
    }
}