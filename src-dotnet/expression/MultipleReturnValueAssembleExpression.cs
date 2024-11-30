/*
 * MultipleReturnValueExpression.java
 */
using Java.Io;
using Java.Util;
using Nsl;

namespace Nsl.Expression
{
    public abstract class MultipleReturnValueAssembleExpression : AssembleExpression
    {
        /// <summary>
        /// Assembles the source code.
        /// </summary>
        public abstract override void Assemble();
        /// <summary>
        /// Assembles the source code.
        /// </summary>
        public abstract override void Assemble(Register var);
        /// <summary>
        /// Assembles the source code.
        /// </summary>
        public abstract void Assemble(List<Register> vars);
    }
}