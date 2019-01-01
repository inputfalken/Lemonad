using Assertion;
using Xunit;

namespace Lemonad.ErrorHandling.Unit.ParserTests.Maybe {
    public class Int {
        [Fact]
        public void With_Valid_String()
            => Parsers.MaybeParsers
                .Int("20")
                .AssertValue(20);

        [Fact]
        public void With_Invalid_String()
            => Parsers.MaybeParsers
                .Int("foobar")
                .AssertNone();

        [Fact]
        public void With_Null_String()
            => Parsers.MaybeParsers
                .Int(null)
                .AssertNone();
    }
}