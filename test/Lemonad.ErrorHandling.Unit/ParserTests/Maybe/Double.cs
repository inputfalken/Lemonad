using Assertion;
using Xunit;

namespace Lemonad.ErrorHandling.Unit.ParserTests.Maybe {
    public class Double {
        [Fact]
        public void With_Valid_String()
            => Parsers.MaybeParsers
                .Double("0.00000001")
                .AssertValue(0.00000001);

        [Fact]
        public void With_Invalid_String()
            => Parsers.MaybeParsers
                .Double("foobar")
                .AssertNone();

        [Fact]
        public void With_Null_String()
            => Parsers.MaybeParsers
                .Double(null)
                .AssertNone();
    }
}