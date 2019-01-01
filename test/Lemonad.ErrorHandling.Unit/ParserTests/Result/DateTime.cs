using System.Globalization;
using Assertion;
using Xunit;

namespace Lemonad.ErrorHandling.Unit.ParserTests.Result {
    public class DateTime {
        [Fact]
        public void With_Valid_String()
            => Parsers.ResultParsers
                .DateTime(System.DateTime.Today.ToString(CultureInfo.InvariantCulture))
                .AssertValue(System.DateTime.Today);

        [Fact]
        public void With_Invalid_String()
            => Parsers.ResultParsers
                .DateTime("foobar")
                .AssertError(
                    AssertionUtilities.FormatStringParserMessage<System.DateTime>(
                        "foobar"
                    )
                );

        [Fact]
        public void With_Null_String()
            => Parsers.ResultParsers
                .DateTime(null)
                .AssertError(
                    AssertionUtilities.FormatStringParserMessage<System.DateTime>(
                        null
                    )
                );

        [Fact]
        public void With_Valid_String_With_DateTimeStyle()
            => Parsers.ResultParsers
                .DateTime(System.DateTime.Today.ToString(CultureInfo.InvariantCulture), DateTimeStyles.None,
                    CultureInfo.InvariantCulture)
                .AssertValue(System.DateTime.Today);

        [Fact]
        public void With_Invalid_String_With_DateTimeStyle()
            => Parsers.ResultParsers
                .DateTime("foobar", DateTimeStyles.None, CultureInfo.InvariantCulture)
                .AssertError(
                    AssertionUtilities.FormatStringParserMessage<System.DateTime>(
                        "foobar"
                    )
                );

        [Fact]
        public void With_Null_String_With_DateTimeStyle()
            => Parsers.ResultParsers
                .DateTime(null, DateTimeStyles.None, CultureInfo.InvariantCulture)
                .AssertError(
                    AssertionUtilities.FormatStringParserMessage<System.DateTime>(
                        null
                    )
                );
    }
}