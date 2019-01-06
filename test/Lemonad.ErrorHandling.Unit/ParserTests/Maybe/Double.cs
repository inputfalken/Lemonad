using System.Globalization;
using Assertion;
using Lemonad.ErrorHandling.Parsers;
using Xunit;

namespace Lemonad.ErrorHandling.Unit.ParserTests.Maybe {
    public class Double {
        [Fact]
        public void With_Invalid_String()
            => MaybeParsers
                .Double("foobar")
                .AssertNone();

        [Fact]
        public void With_Invalid_String_With_NumberStyle()
            => MaybeParsers
                .Double("foobar", NumberStyles.Any, CultureInfo.InvariantCulture)
                .AssertNone();

        [Fact]
        public void With_Null_String()
            => MaybeParsers
                .Double(null)
                .AssertNone();

        [Fact]
        public void With_Null_String_With_NumberStyle()
            => MaybeParsers
                .Double(null, NumberStyles.Any, CultureInfo.InvariantCulture)
                .AssertNone();

        [Fact]
        public void With_Valid_String()
            => MaybeParsers
                .Double("0.00000001")
                .AssertValue(0.00000001);

        [Fact]
        public void With_Valid_String_With_NumberStyle()
            => MaybeParsers
                .Double("0.00000001", NumberStyles.Any, CultureInfo.InvariantCulture)
                .AssertValue(0.00000001);
    }
}