using System.Globalization;
using Assertion;
using Lemonad.ErrorHandling.Parsers;
using Xunit;

namespace Lemonad.ErrorHandling.Unit.ParserTests.Maybe {
    public class Decimal {
        [Fact]
        public void With_Invalid_String()
            => MaybeParsers
                .Decimal("foobar")
                .AssertNone();

        [Fact]
        public void With_Invalid_String_With_NumberStyle()
            => MaybeParsers
                .Decimal("foobar", NumberStyles.Any, CultureInfo.InvariantCulture)
                .AssertNone();

        [Fact]
        public void With_Null_String()
            => MaybeParsers
                .Decimal(null)
                .AssertNone();

        [Fact]
        public void With_Null_String_With_NumberStyle()
            => MaybeParsers
                .Decimal(null, NumberStyles.Any, CultureInfo.InvariantCulture)
                .AssertNone();

        [Fact]
        public void With_Valid_String()
            => MaybeParsers
                .Decimal("0.00000000000001")
                .AssertValue((decimal) 0.00000000000001);

        [Fact]
        public void With_Valid_String_With_NumberStyle()
            => MaybeParsers
                .Decimal("0.00000000000001", NumberStyles.Any, CultureInfo.InvariantCulture)
                .AssertValue((decimal) 0.00000000000001);
    }
}