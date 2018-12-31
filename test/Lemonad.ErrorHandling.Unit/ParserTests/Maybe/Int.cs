using Assertion;
using Xunit;

namespace Lemonad.ErrorHandling.Unit.ParserTests.Maybe {
    public class Int {
        [Fact]
        public void With_Valid_String()
            => Lemonad.ErrorHandling.Parsers.MaybeParsers
                .Int("20")
                .AssertValue(20);

        [Fact]
        public void With_Invalid_String()
            => Lemonad.ErrorHandling.Parsers.MaybeParsers
                .Int("foobar")
                .AssertNone();

        [Fact]
        public void With_Null_String()
            => Lemonad.ErrorHandling.Parsers.MaybeParsers
                .Int(null)
                .AssertNone();
    }
}