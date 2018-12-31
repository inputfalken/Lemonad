using Assertion;
using Xunit;

namespace Lemonad.ErrorHandling.Unit.ParserTests.Result {
    public class Decimal {
        [Fact]
        public void With_Valid_String()
            => Lemonad.ErrorHandling.Parsers.ResultParsers
                .Decimal("0.00000000000001")
                .AssertValue((decimal) 0.00000000000001);

        [Fact]
        public void With_Invalid_String()
            => Lemonad.ErrorHandling.Parsers.ResultParsers
                .Decimal("foobar")
                .AssertError(
                    AssertionUtilities.FormatStringParserMessage<decimal>(
                        "foobar"
                    )
                );

        [Fact]
        public void With_Null_String()
            => Lemonad.ErrorHandling.Parsers.ResultParsers
                .Decimal(null)
                .AssertError(
                    AssertionUtilities.FormatStringParserMessage<decimal>(
                        null
                    )
                );
    }
}