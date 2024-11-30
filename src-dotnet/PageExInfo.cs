/*
 * PageExInfo.java
 */
namespace Nsl
{
    public class PageExInfo
    {
        private static PageExInfo current = null;
        /// <summary>
        /// Class constructor.
        /// </summary>
        public PageExInfo()
        {
        }

        public static bool In()
        {
            return current != null;
        }

        public static PageExInfo GetCurrent()
        {
            return current;
        }

        public static void SetCurrent(PageExInfo pageExInfo)
        {
            current = pageExInfo;
        }
    }
}