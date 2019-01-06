using System.Globalization;
using Assertion;
using Lemonad.ErrorHandling.Parsers;
using Xunit;

namespace Lemonad.ErrorHandling.Unit.ParserTests.Result {
    public class Double {
        [Fact]
        public void With_Invalid_String()
            => ResultParsers
                .Double("foobar")
                .AssertError(
                    AssertionUtilities.FormatStringParserMessage<double>(
                        "foobar"
                    )
                );

        [Fact]
        public void With_Invalid_String_With_NumberStyle()
            => ResultParsers
                .Double("foobar", NumberStyles.Any, CultureInfo.InvariantCulture)
                .AssertError(
                    AssertionUtilities.FormatStringParserMessage<double>(
                        "foobar"
                    )
                );

        [Fact]
        public void With_Null_String()
            => ResultParsers
                .Double(null)
                .AssertError(
                    AssertionUtilities.FormatStringParserMessage<double>(
                        null
                    )
                );

        [Fact]
        public void With_Null_String_With_NumberStyle()
            => ResultParsers
                .Double(null, NumberStyles.Any, CultureInfo.InvariantCulture)
                .AssertError(
                    AssertionUtilities.FormatStringParserMessage<double>(
                        null
                    )
                );

        [Fact]
        public void With_Valid_String()
            => ResultParsers
                .Double("0.00000001")
                .AssertValue(0.00000001);

        [Fact]
        public void With_Valid_String_With_NumberStyle()
            => ResultParsers
                .Double("0.00000001", NumberStyles.Any, CultureInfo.InvariantCulture)
                .AssertValue(0.00000001);
    }
}