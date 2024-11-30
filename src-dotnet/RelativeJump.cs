/*
 * RelativeJump.java
 */
namespace Nsl
{
    public class RelativeJump : Label
    {
        /// <summary>
        /// The zero relative jump.
        /// </summary>
        public static readonly RelativeJump Zero = new RelativeJump("0");
        /// <summary>
        /// The zero relative jump.
        /// </summary>
        public RelativeJump(string jump) : base(jump)
        {
        }

        /// <summary>
        /// The zero relative jump.
        /// </summary>
        /// <summary>
        /// Does nothing.
        /// </summary>
        public override void Write()
        {
        }
    }
}