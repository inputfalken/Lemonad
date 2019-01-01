using System.Globalization;
using Assertion;
using Xunit;

namespace Lemonad.ErrorHandling.Unit.ParserTests.Result {
    public class Decimal {
        [Fact]
        public void With_Valid_String_With_NumberStyle()
            => Parsers.ResultParsers
                .Decimal("0.00000000000001")
                .AssertValue((decimal) 0.00000000000001);

        [Fact]
        public void With_Invalid_String_With_NumberStyle()
            => Parsers.ResultParsers
                .Decimal("foobar", NumberStyles.Any, CultureInfo.InvariantCulture)
                .AssertError(
                    AssertionUtilities.FormatStringParserMessage<decimal>(
                        "foobar"
                    )
                );

        [Fact]
        public void With_Null_String_With_NumberStyle()
            => Parsers.ResultParsers
                .Decimal(null, NumberStyles.Any, CultureInfo.InvariantCulture)
                .AssertError(
                    AssertionUtilities.FormatStringParserMessage<decimal>(
                        null
                    )
                );

        [Fact]
        public void With_Valid_String()
            => Parsers.ResultParsers
                .Decimal("0.00000000000001", NumberStyles.Any, CultureInfo.InvariantCulture)
                .AssertValue((decimal) 0.00000000000001);

        [Fact]
        public void With_Invalid_String()
            => Parsers.ResultParsers
                .Decimal("foobar")
                .AssertError(
                    AssertionUtilities.FormatStringParserMessage<decimal>(
                        "foobar"
                    )
                );

        [Fact]
        public void With_Null_String()
            => Parsers.ResultParsers
                .Decimal(null)
                .AssertError(
                    AssertionUtilities.FormatStringParserMessage<decimal>(
                        null
                    )
                );
    }
}