/*
 * ConstantList.java
 */
using Java.Util;

namespace Nsl
{
    public class ConstantList
    {
        private readonly List<Constant> constantList;
        private readonly HashMap<string, Constant> constantMap;
        private static ConstantList current = new ConstantList();
        /// <summary>
        /// Class constructor.
        /// </summary>
        public ConstantList()
        {
            this.constantList = new List<Constant>();
            this.constantMap = new HashMap<string, Constant>();
            this.Add("$PROGRAMFILES", null);
            this.Add("$PROGRAMFILES32", null);
            this.Add("$PROGRAMFILES64", null);
            this.Add("$COMMONFILES", null);
            this.Add("$COMMONFILES32", null);
            this.Add("$COMMONFILES64", null);
            this.Add("$DESKTOP", null);
            this.Add("$EXEDIR", null);
            this.Add("$EXEFILE", null);
            this.Add("$EXEPATH", null);
            this.Add("$NSISDIR", "${NSISDIR}");
            this.Add("$WINDIR", null);
            this.Add("$SYSDIR", null);
            this.Add("$TEMP", null);
            this.Add("$STARTMENU", null);
            this.Add("$SMPROGRAMS", null);
            this.Add("$SMSTARTUP", null);
            this.Add("$QUICKLAUNCH", null);
            this.Add("$DOCUMENTS", null);
            this.Add("$SENDTO", null);
            this.Add("$RECENT", null);
            this.Add("$FAVORITES", null);
            this.Add("$MUSIC", null);
            this.Add("$PICTURES", null);
            this.Add("$VIDEOS", null);
            this.Add("$NETHOOD", null);
            this.Add("$FONTS", null);
            this.Add("$TEMPLATES", null);
            this.Add("$APPDATA", null);
            this.Add("$LOCALAPPDATA", null);
            this.Add("$PRINTHOOD", null);
            this.Add("$INTERNET_CACHE", null);
            this.Add("$COOKIES", null);
            this.Add("$HISTORY", null);
            this.Add("$PROFILE", null);
            this.Add("$ADMINTOOLS", null);
            this.Add("$RESOURCES", null);
            this.Add("$RESOURCES_LOCALIZED", null);
            this.Add("$CDBURN_AREA", null);
            this.Add("$HWNDPARENT", null);
            this.Add("$PLUGINSDIR", null);
        }

        private void Add(string name, string realName)
        {
            Constant constant = new Constant(name, realName, this.constantList.Count);
            this.constantList.Add(constant);
            this.constantMap.Put(name, constant);
        }

        public virtual int Lookup(string name)
        {
            Constant constant = this.constantMap[name];
            if (constant != null)
                return constant.GetIndex();
            return -1;
        }

        public static ConstantList GetCurrent()
        {
            return current;
        }

        public virtual Constant Get(int index)
        {
            return this.constantList[index];
        }
    }
}