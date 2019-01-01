using System.Globalization;
using Assertion;
using Xunit;

namespace Lemonad.ErrorHandling.Unit.ParserTests.Maybe {
    public class DateTime {
        [Fact]
        public void With_Valid_String()
            => Parsers.MaybeParsers
                .DateTime(System.DateTime.Today.ToString(CultureInfo.InvariantCulture))
                .AssertValue(System.DateTime.Today);

        [Fact]
        public void With_Invalid_String()
            => Parsers.MaybeParsers
                .DateTime("foobar")
                .AssertNone();

        [Fact]
        public void With_Null_String()
            => Parsers.MaybeParsers
                .DateTime(null)
                .AssertNone();
    }
}