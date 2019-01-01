using System.Globalization;
using Assertion;
using Xunit;

namespace Lemonad.ErrorHandling.Unit.ParserTests.Result {
    public class Double {
        [Fact]
        public void With_Valid_String_With_NumberStyle()
            => Parsers.ResultParsers
                .Double("0.00000001", NumberStyles.Any, CultureInfo.InvariantCulture)
                .AssertValue(0.00000001);

        [Fact]
        public void With_Invalid_String_With_NumberStyle()
            => Parsers.ResultParsers
                .Double("foobar", NumberStyles.Any, CultureInfo.InvariantCulture)
                .AssertError(
                    AssertionUtilities.FormatStringParserMessage<double>(
                        "foobar"
                    )
                );

        [Fact]
        public void With_Null_String_With_NumberStyle()
            => Parsers.ResultParsers
                .Double(null, NumberStyles.Any, CultureInfo.InvariantCulture)
                .AssertError(
                    AssertionUtilities.FormatStringParserMessage<double>(
                        null
                    )
                );

        [Fact]
        public void With_Valid_String()
            => Parsers.ResultParsers
                .Double("0.00000001")
                .AssertValue(0.00000001);

        [Fact]
        public void With_Invalid_String()
            => Parsers.ResultParsers
                .Double("foobar")
                .AssertError(
                    AssertionUtilities.FormatStringParserMessage<double>(
                        "foobar"
                    )
                );

        [Fact]
        public void With_Null_String()
            => Parsers.ResultParsers
                .Double(null)
                .AssertError(
                    AssertionUtilities.FormatStringParserMessage<double>(
                        null
                    )
                );
    }
}