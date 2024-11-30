/*
 * ComparisonType.java
 */
namespace Nsl
{
    public enum ComparisonType
    {
        Integer,
        IntegerUnsigned,
        String,
        StringCaseSensitive 

        // --------------------
        // TODO enum body members
        // /**
        //  * Matches a {@code ComparisonType} value from the current script tokenizer.
        //  * @return a {@code ComparisonType} value
        //  */
        // public static ComparisonType match() {
        //     if (ScriptParser.tokenizer.match("u"))
        //         return ComparisonType.IntegerUnsigned;
        //     if (ScriptParser.tokenizer.match("s"))
        //         return ComparisonType.String;
        //     if (ScriptParser.tokenizer.match("S"))
        //         return ComparisonType.StringCaseSensitive;
        //     return ComparisonType.Integer;
        // }
        // --------------------
    }
}