using System.Globalization;
using Assertion;
using Lemonad.ErrorHandling.Parsers;
using Xunit;

namespace Lemonad.ErrorHandling.Unit.ParserTests.Result {
    public class DateTime {
        [Fact]
        public void With_Invalid_String()
            => ResultParsers
                .DateTime("foobar")
                .AssertError(
                    AssertionUtilities.FormatStringParserMessage<System.DateTime>(
                        "foobar"
                    )
                );

        [Fact]
        public void With_Invalid_String_With_DateTimeStyle()
            => ResultParsers
                .DateTime("foobar", DateTimeStyles.None, CultureInfo.InvariantCulture)
                .AssertError(
                    AssertionUtilities.FormatStringParserMessage<System.DateTime>(
                        "foobar"
                    )
                );

        [Fact]
        public void With_Null_String()
            => ResultParsers
                .DateTime(null)
                .AssertError(
                    AssertionUtilities.FormatStringParserMessage<System.DateTime>(
                        null
                    )
                );

        [Fact]
        public void With_Null_String_With_DateTimeStyle()
            => ResultParsers
                .DateTime(null, DateTimeStyles.None, CultureInfo.InvariantCulture)
                .AssertError(
                    AssertionUtilities.FormatStringParserMessage<System.DateTime>(
                        null
                    )
                );

        [Fact]
        public void With_Valid_String()
            => ResultParsers
                .DateTime(System.DateTime.Today.ToString(CultureInfo.InvariantCulture))
                .AssertValue(System.DateTime.Today);

        [Fact]
        public void With_Valid_String_With_DateTimeStyle()
            => ResultParsers
                .DateTime(System.DateTime.Today.ToString(CultureInfo.InvariantCulture), DateTimeStyles.None,
                    CultureInfo.InvariantCulture)
                .AssertValue(System.DateTime.Today);
    }
}