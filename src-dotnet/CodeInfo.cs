/*
 * CodeInfo.java
 */
using Java.Util;

namespace Nsl
{
    /// <summary>
    ///  * @author Stuart
    /// </summary>
    public abstract class CodeInfo
    {
        protected static CodeInfo current = null;
        protected readonly HashMap<int, Register> usedVars;
        private Label labelBreak;
        private Label labelContinue;
        protected CodeInfo()
        {
            this.usedVars = new HashMap<int, Register>();
            this.labelBreak = null;
            this.labelContinue = null;
        }

        public virtual HashMap<int, Register> GetUsedVars()
        {
            return this.usedVars;
        }

        public virtual Label GetBreakLabel()
        {
            return this.labelBreak;
        }

        public virtual Label SetBreakLabel(Label labelBreak)
        {
            Label old = this.labelBreak;
            this.labelBreak = labelBreak;
            return old;
        }

        public virtual Label GetContinueLabel()
        {
            return this.labelContinue;
        }

        public virtual Label SetContinueLabel(Label labelContinue)
        {
            Label old = this.labelContinue;
            this.labelContinue = labelContinue;
            return old;
        }

        public virtual void AddUsedVar(Register var)
        {
            int key = Integer.ValueOf(var.GetIntegerValue());
            if (this.usedVars[key] == null)
                this.usedVars.Put(key, var);
        }

        public static CodeInfo GetCurrent()
        {
            return current;
        }

        public static void SetCurrent(CodeInfo codeInfo)
        {
            current = codeInfo;
        }
    }
}