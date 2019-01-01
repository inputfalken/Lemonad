using System.Globalization;
using Assertion;
using Xunit;

namespace Lemonad.ErrorHandling.Unit.ParserTests.Result {
    public class Int {
        [Fact]
        public void With_Valid_String_With_NumberStyle()
            => Parsers.ResultParsers
                .Int("20", NumberStyles.Any, CultureInfo.InvariantCulture)
                .AssertValue(20);

        [Fact]
        public void With_Invalid_String_With_NumberStyle()
            => Parsers.ResultParsers
                .Int("foobar", NumberStyles.Any, CultureInfo.InvariantCulture)
                .AssertError(
                    AssertionUtilities.FormatStringParserMessage<int>(
                        "foobar"
                    )
                );

        [Fact]
        public void With_Null_String_With_NumberStyle()
            => Parsers.ResultParsers
                .Int(null, NumberStyles.Any, CultureInfo.InvariantCulture)
                .AssertError(
                    AssertionUtilities.FormatStringParserMessage<int>(
                        null
                    )
                );

        [Fact]
        public void With_Valid_String()
            => Parsers.ResultParsers
                .Int("20")
                .AssertValue(20);

        [Fact]
        public void With_Invalid_String()
            => Parsers.ResultParsers
                .Int("foobar")
                .AssertError(
                    AssertionUtilities.FormatStringParserMessage<int>(
                        "foobar"
                    )
                );

        [Fact]
        public void With_Null_String()
            => Parsers.ResultParsers
                .Int(null)
                .AssertError(
                    AssertionUtilities.FormatStringParserMessage<int>(
                        null
                    )
                );
    }
}