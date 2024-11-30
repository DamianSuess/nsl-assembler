/*
 * Label.java
 */
using Java.Io;

namespace Nsl
{
    public class Label
    {
        private readonly string name;
        private bool notUsed;
        public Label(string name)
        {
            this.name = name;
            this.notUsed = false;
        }

        public virtual bool IsNotUsed()
        {
            return this.notUsed;
        }

        public virtual void SetNotUsed(bool notUsed)
        {
            this.notUsed = notUsed;
        }

        /// <summary>
        /// Writes the go-to label.
        /// </summary>
        public virtual void Write()
        {
            if (!this.notUsed)
                ScriptParser.WriteLine(this.name + ":");
        }

        public virtual string ToString()
        {
            this.notUsed = false;
            return this.name;
        }
    }
}