/*
 * Scope.java
 */
using Java.Util;

namespace Nsl
{
    public class Scope
    {
        private static readonly Scope global = new Scope();
        private static readonly Scope globalUninstaller = new Scope();
        private static Scope current = global;
        private static bool inUninstaller;
        private readonly Scope parent;
        private readonly List<int> registersList;
        /// <summary>
        /// Class constructor.
        /// </summary>
        private Scope()
        {
            this.parent = current;
            this.registersList = new List<int>();
        }

        public static Scope Create()
        {
            return (current = new Scope());
        }

        public static Scope GetCurrent()
        {
            return current;
        }

        public static Scope GetGlobal()
        {
            return global;
        }

        public static Scope GetGlobalUninstaller()
        {
            return globalUninstaller;
        }

        public static void SetInUninstaller(bool @in)
        {
            inUninstaller = @in;
            if (@in)
                current = globalUninstaller;
            else
                current = global;
        }

        public static bool InUninstaller()
        {
            return inUninstaller;
        }

        /// <summary>
        /// Must be called when the scope ends.
        /// </summary>
        public virtual void End()
        {
            current = this.parent;
        }

        public virtual void AddVar(int registerIndex)
        {
            if (!this.RegisterExists(registerIndex))
                this.registersList.Add(registerIndex);
        }

        private bool RegisterExists(int registerIndex)
        {
            return this.registersList.Contains(registerIndex) || (this.parent != null && this.parent.RegisterExists(registerIndex));
        }

        public virtual void Check(int registerIndex)
        {
            if ((FunctionInfo.In() || SectionInfo.In()) && !this.RegisterExists(registerIndex))
                throw new NslException("Variable " + RegisterList.GetCurrent()[registerIndex] + " may not have been initialised", true);
        }
    }
}