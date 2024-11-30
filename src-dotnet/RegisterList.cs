/*
 * RegisterList.java
 */
using Java.Io;
using Java.Util;

namespace Nsl
{
    public class RegisterList
    {
        private readonly List<Register> registerList;
        private readonly HashMap<string, Register> registerMap;
        private static RegisterList current = new RegisterList();
        /// <summary>
        /// Class constructor.
        /// </summary>
        public RegisterList()
        {
            this.registerList = new List<Register>();
            this.registerMap = new HashMap<string, Register>();
            this.Add("$0", RegisterType.Register, false);
            this.Add("$1", RegisterType.Register, false);
            this.Add("$2", RegisterType.Register, false);
            this.Add("$3", RegisterType.Register, false);
            this.Add("$4", RegisterType.Register, false);
            this.Add("$5", RegisterType.Register, false);
            this.Add("$6", RegisterType.Register, false);
            this.Add("$7", RegisterType.Register, false);
            this.Add("$8", RegisterType.Register, false);
            this.Add("$9", RegisterType.Register, false);
            this.Add("$R0", RegisterType.Register, false);
            this.Add("$R1", RegisterType.Register, false);
            this.Add("$R2", RegisterType.Register, false);
            this.Add("$R3", RegisterType.Register, false);
            this.Add("$R4", RegisterType.Register, false);
            this.Add("$R5", RegisterType.Register, false);
            this.Add("$R6", RegisterType.Register, false);
            this.Add("$R7", RegisterType.Register, false);
            this.Add("$R8", RegisterType.Register, false);
            this.Add("$R9", RegisterType.Register, false);
            this.Add("$INSTDIR", RegisterType.Other, true);
            this.Add("$OUTDIR", RegisterType.Other, true);
        }

        private void Add(string name, RegisterType registerType, bool inUse)
        {
            Register register = new Register(name, this.registerList.Count, registerType, inUse);
            this.registerList.Add(register);
            this.registerMap.Put(register.ToString(), register);
            if (registerType == RegisterType.Other)
            {
                Scope.GetGlobal().AddVar(register.GetIndex());
                Scope.GetGlobalUninstaller().AddVar(register.GetIndex());
            }
        }

        public virtual int Add(string name)
        {
            Register register = this.registerMap[name];
            if (register != null)
            {
                register.SetInUse(true);
                return register.GetIndex();
            }

            int index = this.registerList.Count;
            register = new Register(name, index, RegisterType.Variable, false);
            register.SetInUse(true);
            this.registerList.Add(register);
            this.registerMap.Put(register.ToString(), register);
            return index;
        }

        public virtual Register Get(int index)
        {
            return this.registerList[index];
        }

        public virtual void SetAllInUse(bool inUse)
        {
            foreach (Register register in this.registerList)
            {
                if (register.GetRegisterType() == RegisterType.Register)
                    register.SetInUse(inUse);
                else
                    break;
            }
        }

        public virtual Register GetNext()
        {
            foreach (Register register in this.registerList)
            {
                if (register.GetRegisterType() != RegisterType.Other && !register.GetInUse())
                {
                    register.SetInUse(true);
                    return register;
                }
            }

            throw new NslException("Out of registers and variables (used all " + this.registerList.Count + ")!");
        }

        public static RegisterList GetCurrent()
        {
            return current;
        }

        public static void SetCurrent(RegisterList current)
        {
            RegisterList.current = current;
        }

        /// <summary>
        /// Writes the variable declarations to the output stream.
        /// </summary>
        public virtual void DefineVars()
        {
            foreach (Register register in this.registerList)
                if (register.GetRegisterType() == RegisterType.Variable)
                    ScriptParser.WriteLine("Var " + register.ToString().Substring(1));
        }
    }
}