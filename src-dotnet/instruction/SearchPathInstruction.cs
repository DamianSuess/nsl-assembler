/*
 * SearchPathInstruction.java
 */
using Java.Io;
using Java.Util;
using Nsl;

namespace Nsl.Instruction
{
    /// <remarks>@authorStuart</remarks>
    public class SearchPathInstruction : AssembleExpression
    {
        public static readonly string name = "SearchPath";
        private readonly Expression file;
        public SearchPathInstruction(int returns)
        {
            if (PageExInfo.In())
                throw new NslContextException(EnumSet.Of(NslContext.Section, NslContext.Function, NslContext.Global), name);
            if (returns != 1)
                throw new NslReturnValueException(name, 1);
            List<Expression> paramsList = Expression.MatchList();
            if (paramsList.Count != 1)
                throw new NslArgumentException(name, 1);
            this.file = paramsList[0];
        }

        public override void Assemble()
        {
            throw new NotSupportedException("Not supported.");
        }

        public override void Assemble(Register var)
        {
            Expression varOrFile = AssembleExpression.GetRegisterOrExpression(this.file);
            ScriptParser.WriteLine(name + " " + var + " " + varOrFile);
            varOrFile.SetInUse(false);
        }
    }
}