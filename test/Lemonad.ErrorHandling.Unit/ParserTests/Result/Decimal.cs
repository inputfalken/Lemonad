using System.Globalization;
using Assertion;
using Lemonad.ErrorHandling.Parsers;
using Xunit;

namespace Lemonad.ErrorHandling.Unit.ParserTests.Result {
    public class Decimal {
        [Fact]
        public void With_Invalid_String()
            => ResultParsers
                .Decimal("foobar")
                .AssertError(
                    AssertionUtilities.FormatStringParserMessage<decimal>(
                        "foobar"
                    )
                );

        [Fact]
        public void With_Invalid_String_With_NumberStyle()
            => ResultParsers
                .Decimal("foobar", NumberStyles.Any, CultureInfo.InvariantCulture)
                .AssertError(
                    AssertionUtilities.FormatStringParserMessage<decimal>(
                        "foobar"
                    )
                );

        [Fact]
        public void With_Null_String()
            => ResultParsers
                .Decimal(null)
                .AssertError(
                    AssertionUtilities.FormatStringParserMessage<decimal>(
                        null
                    )
                );

        [Fact]
        public void With_Null_String_With_NumberStyle()
            => ResultParsers
                .Decimal(null, NumberStyles.Any, CultureInfo.InvariantCulture)
                .AssertError(
                    AssertionUtilities.FormatStringParserMessage<decimal>(
                        null
                    )
                );

        [Fact]
        public void With_Valid_String()
            => ResultParsers
                .Decimal("0.00000000000001", NumberStyles.Any, CultureInfo.InvariantCulture)
                .AssertValue((decimal) 0.00000000000001);

        [Fact]
        public void With_Valid_String_With_NumberStyle()
            => ResultParsers
                .Decimal("0.00000000000001")
                .AssertValue((decimal) 0.00000000000001);
    }
}