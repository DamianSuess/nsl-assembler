/*
 * LabelList.java
 */
using Java.Io;

namespace Nsl
{
    public class LabelList
    {
        private static LabelList current = new LabelList();
        private int counter;
        /// <summary>
        /// Class constructor.
        /// </summary>
        public LabelList()
        {
            this.counter = 0;
        }

        /// <summary>
        /// Resets the label list counter.
        /// </summary>
        public virtual void Reset()
        {
            this.counter = 0;
        }

        public static LabelList GetCurrent()
        {
            return current;
        }

        public virtual Label GetNext()
        {
            Label label = new Label("_lbl_" + this.counter);
            this.counter++;
            return label;
        }
    }
}