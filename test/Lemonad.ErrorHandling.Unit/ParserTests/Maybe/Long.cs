using Assertion;
using Xunit;

namespace Lemonad.ErrorHandling.Unit.ParserTests.Maybe {
    public class Long {
        [Fact]
        public void With_Valid_String()
            => Lemonad.ErrorHandling.Parsers.MaybeParsers
                .Long(long.MaxValue.ToString())
                .AssertValue(long.MaxValue);

        [Fact]
        public void With_Invalid_String()
            => Lemonad.ErrorHandling.Parsers.MaybeParsers
                .Long("foobar")
                .AssertNone();

        [Fact]
        public void With_Null_String()
            => Lemonad.ErrorHandling.Parsers.MaybeParsers
                .Long(null)
                .AssertNone();
    }
}