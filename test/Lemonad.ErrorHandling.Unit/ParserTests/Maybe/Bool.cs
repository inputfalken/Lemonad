using Assertion;
using Xunit;

namespace Lemonad.ErrorHandling.Unit.ParserTests.Maybe {
    public class Bool {
        [Fact]
        public void With_Valid_String_For_True()
            => Parsers.MaybeParsers.Bool("true").AssertValue(true);

        [Fact]
        public void With_Valid_String_For_False()
            => Parsers.MaybeParsers.Bool("false").AssertValue(false);

        [Fact]
        public void With_Invalid_String()
            => Parsers.MaybeParsers
                .Bool("foobar")
                .AssertNone();

        [Fact]
        public void With_Null_String()
            => Parsers.MaybeParsers
                .Bool(null)
                .AssertNone();
    }
}