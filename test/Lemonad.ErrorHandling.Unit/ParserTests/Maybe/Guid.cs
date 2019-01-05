using Assertion;
using Lemonad.ErrorHandling.Parsers;
using Xunit;

namespace Lemonad.ErrorHandling.Unit.ParserTests.Maybe {
    public class Guid {
        [Fact]
        public void With_Invalid_String()
            => MaybeParsers.Guid("foobar").AssertNone();

        [Fact]
        public void With_Null_String()
            => MaybeParsers.Guid(null).AssertNone();

        [Fact]
        public void With_Valid_String() {
            var guid = System.Guid.NewGuid();
            MaybeParsers.Guid(guid.ToString()).AssertValue(guid);
        }
    }
}