/*
 * SetBrandingImageInstruction.java
 */
using Java.Io;
using Java.Util;
using Nsl;

namespace Nsl.Instruction
{
    /// <remarks>@authorStuart</remarks>
    public class SetBrandingImageInstruction : AssembleExpression
    {
        public static readonly string name = "SetBrandingImage";
        private readonly Expression file;
        private readonly Expression resizeToFit;
        private readonly Expression imgId;
        public SetBrandingImageInstruction(int returns)
        {
            if (!SectionInfo.In() && !FunctionInfo.In())
                throw new NslContextException(EnumSet.Of(NslContext.Section, NslContext.Function), name);
            if (returns > 0)
                throw new NslReturnValueException(name);
            List<Expression> paramsList = Expression.MatchList();
            int paramsCount = paramsList.Count;
            if (paramsCount < 1 || paramsCount > 3)
                throw new NslArgumentException(name, 1, 3);
            this.file = paramsList[0];
            if (paramsCount > 1)
            {
                this.resizeToFit = paramsList[1];
                if (!ExpressionType.IsBoolean(this.resizeToFit))
                    throw new NslArgumentException(name, 2, ExpressionType.Boolean);
                if (paramsCount > 2)
                {
                    this.imgId = paramsList[2];
                    if (!ExpressionType.IsInteger(this.imgId))
                        throw new NslArgumentException(name, 3, ExpressionType.Integer);
                }
                else
                {
                    this.imgId = null;
                }
            }
            else
            {
                this.resizeToFit = null;
                this.imgId = null;
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
            Expression varOrFile = AssembleExpression.GetRegisterOrExpression(this.file);
            string write = name + " " + var + " " + varOrFile;
            if (this.resizeToFit != null)
            {
                AssembleExpression.AssembleIfRequired(this.resizeToFit);
                if (this.imgId != null)
                {
                    AssembleExpression.AssembleIfRequired(this.imgId);
                    if (this.imgId.GetBooleanValue() == true)
                        write += " /IMGID=" + this.imgId;
                }

                if (this.resizeToFit.GetBooleanValue() == true)
                    write += " /RESIZETOFIT";
            }

            ScriptParser.WriteLine(write);
            varOrFile.SetInUse(false);
        }
    }
}