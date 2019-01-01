using Assertion;
using Xunit;

namespace Lemonad.ErrorHandling.Unit.ParserTests.Maybe {
    public class Decimal {
        [Fact]
        public void With_Valid_String()
            => Parsers.MaybeParsers
                .Decimal("0.00000000000001")
                .AssertValue((decimal) 0.00000000000001);

        [Fact]
        public void With_Invalid_String()
            => Parsers.MaybeParsers
                .Decimal("foobar")
                .AssertNone();

        [Fact]
        public void With_Null_String()
            => Parsers.MaybeParsers
                .Decimal(null)
                .AssertNone();
    }
}