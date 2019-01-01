namespace Lemonad.ErrorHandling.Parsers {
    /// <summary>
    ///  Format options for <see cref="System.Guid"/>.
    /// </summary>
    public enum GuidFormat {
        /// <summary>
        /// 32 digits:
        /// </summary>
        DigitsOnly = 'N',

        /// <summary>
        /// 32 digits separated by hyphens:
        /// </summary>
        DigitsWithHyphens = 'D',

        /// <summary>
        /// 32 digits separated by hyphens, enclosed in braces:
        /// </summary>
        DigitsWithHyphensWrappedInBrackets = 'B',

        /// <summary>
        /// 32 digits separated by hyphens, enclosed in parentheses:
        /// </summary>
        DigitsWithHyphensWrappedInParentheses = 'P',

        /// <summary>
        /// Four hexadecimal values enclosed in braces, where the fourth value is a subset of eight hexadecimal values that is also enclosed in braces:
        /// </summary>
        FourHexadecimalWrappedInBrackets = 'X'
    }
}