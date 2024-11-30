/*
 * InstTypeInstruction.java
 */
using Java.Io;
using Java.Util;
using Nsl;

namespace Nsl.Instruction
{
    /// <remarks>@authorStuart</remarks>
    public class InstTypeInstruction : AssembleExpression
    {
        public static readonly string name = "InstType";
        private readonly Expression value1;
        private readonly Expression value2;
        public InstTypeInstruction(int returns)
        {
            if (!ScriptParser.InGlobalContext())
                throw new NslContextException(EnumSet.Of(NslContext.Global), name);
            if (returns > 0)
                throw new NslReturnValueException(name);
            List<Expression> paramsList = Expression.MatchList();
            int paramsCount = paramsList.Count;
            if (paramsCount < 1 || paramsCount > 2)
                throw new NslArgumentException(name, 1, 2);
            this.value1 = paramsList[0];
            if (!ExpressionType.IsBoolean(this.value1) && !ExpressionType.IsString(this.value1))
                throw new NslArgumentException(name, 1, ExpressionType.Boolean, ExpressionType.String);
            if (paramsCount > 1)
            {
                this.value2 = paramsList[1];
                if (!ExpressionType.IsString(this.value2))
                    throw new NslArgumentException(name, 2, ExpressionType.String);
            }
            else
                this.value2 = null;
        }

        /// <summary>
        /// Assembles the source code.
        /// </summary>
        public override void Assemble()
        {
            AssembleExpression.AssembleIfRequired(this.value1);
            if (this.value1.GetType().Equals(ExpressionType.Boolean))
            {
                /*
       * InstType(true)
       * InstType /NOCUSTOM
       */
                if (this.value1.GetBooleanValue() == true)
                {
                    if (this.value2 != null)
                        AssembleExpression.AssembleIfRequired(this.value2);
                    ScriptParser.WriteLine(name + " /NOCUSTOM");
                }
                else
                {
                    /*
         * InstType(false)
         * InstType /COMPONENTSONLYONCUSTOM
         */
                    if (this.value2 == null)
                    {
                        ScriptParser.WriteLine(name + " /COMPONENTSONLYONCUSTOM");
                    }
                    else
                    /*
         * InstType(false, "str")
         * InstType /CUSTOMSTRING=str
         */
                    {
                        Expression varOrValue = AssembleExpression.GetRegisterOrExpression(this.value2);
                        ScriptParser.WriteLine(name + " \"/CUSTOMSTRING=" + varOrValue.ToString(true) + "\"");
                        varOrValue.SetInUse(true);
                    }
                }
            }
            else
            /*
     * InstType("install_type_name")
     * InstType install_type_name
     */
            {
                if (this.value2 != null)
                    AssembleExpression.AssembleIfRequired(this.value2);
                ScriptParser.WriteLine(name + " " + this.value1);
            }
        }

        /// <summary>
        /// Assembles the source code.
        /// </summary>
        /*
       * InstType(true)
       * InstType /NOCUSTOM
       */
        /*
         * InstType(false)
         * InstType /COMPONENTSONLYONCUSTOM
         */
        /*
         * InstType(false, "str")
         * InstType /CUSTOMSTRING=str
         */
        /*
     * InstType("install_type_name")
     * InstType install_type_name
     */
        public override void Assemble(Register var)
        {
            throw new NotSupportedException("Not supported.");
        }
    }
}