/*
 * RegisterType.java
 */
namespace Nsl
{
    public enum RegisterType
    {
        /// <summary>
        /// Built in NSIS registers ($0-$9, $R0-$R9).
        /// </summary>
        Register,
        /// <summary>
        /// User defined variable (e.g. $MyVar).
        /// </summary>
        Variable,
        /// <summary>
        /// Other registers/variables (such as $INSTDIR, $OUTDIR).
        /// </summary>
        Other
    }
}