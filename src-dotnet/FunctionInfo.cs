/*
 * FunctionInfo.java
 */
using Java.Util;

namespace Nsl
{
    public class FunctionInfo : CodeInfo
    {
        private static readonly List<FunctionInfo> list = new List<FunctionInfo>();
        private readonly string name;
        private readonly int params;
        private int returns;
        private readonly bool isCallback;
        private FunctionInfo(string name, List<Register> @params)
        {
            this.name = name;
            this.@params = @params.Count;
            this.returns = -1;
            foreach (Register param in @params)
                this.usedVars.Put(Integer.ValueOf(param.GetIntegerValue()), param);
            this.isCallback = IsCallbackFunction(name);
        }

        public static FunctionInfo Create(string name, List<Register> @params)
        {
            FunctionInfo functionInfo = new FunctionInfo(name, @params);
            list.Add(functionInfo);
            return functionInfo;
        }

        public static bool In()
        {
            return current != null && current is FunctionInfo;
        }

        public static FunctionInfo GetCurrent()
        {
            return (FunctionInfo)current;
        }

        private static bool IsCallbackFunction(string name)
        {
            name = name.ToLowerCase();
            return name.Equals(".onguiinit") || name.Equals("un.onguiinit") || name.Equals(".oninit") || name.Equals("un.oninit") || name.Equals(".oninstfailed") || name.Equals("un.oninstfailed") || name.Equals(".oninstsuccess") || name.Equals("un.oninstsuccess") || name.Equals(".onguiend") || name.Equals("un.onguiend") || name.Equals(".onmouseoversection") || name.Equals(".onrebootfailed") || name.Equals("un.onrebootfailed") || name.Equals(".onselchange") || name.Equals("un.onselchange") || name.Equals(".onuserabort") || name.Equals("un.onuserabort") || name.Equals(".onverifyinstdir");
        }

        public virtual string GetName()
        {
            if (this.isCallback || this.returns == -1 || (this.@params == 0 && this.returns == 0))
                return this.name;
            return this.name + '_' + this.@params + '_' + this.returns;
        }

        public virtual int GetParams()
        {
            return this.@params;
        }

        public virtual int GetReturns()
        {
            return this.returns;
        }

        public virtual void SetReturns(int returns)
        {
            this.returns = returns;
        }

        public static List<FunctionInfo> GetList()
        {
            return list;
        }

        public static int IsOnInitDefined()
        {
            int ret = 0;
            foreach (FunctionInfo functionInfo in list)
            {
                if (functionInfo.name.EqualsIgnoreCase(".onInit"))
                    ret |= 1;
                else if (functionInfo.name.EqualsIgnoreCase("un.onInit"))
                    ret |= 2;
            }

            return ret;
        }

        public virtual bool IsCallback()
        {
            return this.isCallback;
        }

        public static FunctionInfo Find(string name, int @params, int returns)
        {
            foreach (FunctionInfo functionInfo in list)
                if (functionInfo.Matches(name, @params, returns))
                    return functionInfo;
            return null;
        }

        public virtual bool Matches(string name, int @params, int returns)
        {

            // Matching name?
            if (!this.name.EqualsIgnoreCase(name))
                return false;

            // Check call parameters count.
            if (this.@params != @params)
                return false;

            // Check return parameters count.
            if (returns > 0 && this.returns != returns)
                return false;
            /*// Check call parameters types.
    for (int i = 0; i < paramCount; i++)
      if (this.params.get(i).getType() != call.params.get(i).getType())
        return false;

    // Check return parameters types.
    for (int i = 0; i < returnCount; i++)
      if (this.returns.get(i).getType() != call.returns.get(i).getType())
        return false;*/
            return true;
        }
    }
}