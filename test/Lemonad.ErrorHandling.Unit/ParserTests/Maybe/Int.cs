using System.Globalization;
using Assertion;
using Lemonad.ErrorHandling.Parsers;
using Xunit;

namespace Lemonad.ErrorHandling.Unit.ParserTests.Maybe {
    public class Int {
        [Fact]
        public void With_Invalid_String()
            => MaybeParsers
                .Int("foobar")
                .AssertNone();

        [Fact]
        public void With_Invalid_String_With_NumberStyle()
            => MaybeParsers
                .Int("foobar", NumberStyles.Any, CultureInfo.InvariantCulture)
                .AssertNone();

        [Fact]
        public void With_Null_String()
            => MaybeParsers
                .Int(null)
                .AssertNone();

        [Fact]
        public void With_Null_String_With_NumberStyle()
            => MaybeParsers
                .Int(null, NumberStyles.Any, CultureInfo.InvariantCulture)
                .AssertNone();

        [Fact]
        public void With_Valid_String()
            => MaybeParsers
                .Int("20")
                .AssertValue(20);

        [Fact]
        public void With_Valid_String_With_NumberStyle()
            => MaybeParsers
                .Int("20", NumberStyles.Any, CultureInfo.InvariantCulture)
                .AssertValue(20);
    }
}