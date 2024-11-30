/*
 * NullAssembleExpression.java
 */
using Java.Io;
using Nsl;

namespace Nsl.Expression
{
    public class NullAssembleExpression : AssembleExpression
    {
        /// <summary>
        /// Assembles nothing.
        /// </summary>
        public override void Assemble()
        {
        }

        /// <summary>
        /// Assembles nothing.
        /// </summary>
        /// <summary>
        /// Assembles nothing.
        /// </summary>
        public override void Assemble(Register var)
        {
        }
    }
}