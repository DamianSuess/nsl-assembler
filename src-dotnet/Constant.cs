/*
 * Constant.java
 */
namespace Nsl
{
    public class Constant
    {
        private readonly string name;
        private readonly string realName;
        private readonly int index;
        public Constant(string name, string realName, int index)
        {
            this.name = name;
            this.realName = null;
            this.index = index;
        }

        public virtual int GetIndex()
        {
            return this.index;
        }

        public virtual string ToString()
        {
            if (this.realName != null)
                return this.realName;
            return this.name;
        }
    }
}