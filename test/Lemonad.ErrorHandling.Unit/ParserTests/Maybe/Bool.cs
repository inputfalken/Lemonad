using Assertion;
using Lemonad.ErrorHandling.Parsers;
using Xunit;

namespace Lemonad.ErrorHandling.Unit.ParserTests.Maybe {
    public class Bool {
        [Fact]
        public void With_Invalid_String()
            => MaybeParsers
                .Bool("foobar")
                .AssertNone();

        [Fact]
        public void With_Null_String()
            => MaybeParsers
                .Bool(null)
                .AssertNone();

        [Fact]
        public void With_Valid_String_For_False()
            => MaybeParsers.Bool("false").AssertValue(false);

        [Fact]
        public void With_Valid_String_For_True()
            => MaybeParsers.Bool("true").AssertValue(true);
    }
}