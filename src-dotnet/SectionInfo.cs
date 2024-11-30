/*
 * SectionInfo.java
 */
namespace Nsl
{
    public class SectionInfo : CodeInfo
    {
        /// <summary>
        /// Class constructor.
        /// </summary>
        public SectionInfo()
        {
        }

        /// <summary>
        /// Class constructor.
        /// </summary>
        public static bool In()
        {
            return current != null && current is SectionInfo;
        }

        /// <summary>
        /// Class constructor.
        /// </summary>
        public static SectionInfo GetCurrent()
        {
            return (SectionInfo)current;
        }
    }
}