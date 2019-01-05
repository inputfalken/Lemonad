using System.Globalization;
using Assertion;
using Lemonad.ErrorHandling.Parsers;
using Xunit;

namespace Lemonad.ErrorHandling.Unit.ParserTests.Maybe {
    public class Long {
        [Fact]
        public void With_Invalid_String()
            => MaybeParsers
                .Long("foobar")
                .AssertNone();

        [Fact]
        public void With_Invalid_String_With_NumberStyle()
            => MaybeParsers
                .Long("foobar", NumberStyles.Any, CultureInfo.InvariantCulture)
                .AssertNone();

        [Fact]
        public void With_Null_String()
            => MaybeParsers
                .Long(null)
                .AssertNone();

        [Fact]
        public void With_Null_String_With_NumberStyle()
            => MaybeParsers
                .Long(null, NumberStyles.Any, CultureInfo.InvariantCulture)
                .AssertNone();

        [Fact]
        public void With_Valid_String()
            => MaybeParsers
                .Long(long.MaxValue.ToString())
                .AssertValue(long.MaxValue);

        [Fact]
        public void With_Valid_String_With_NumberStyle()
            => MaybeParsers
                .Long(long.MaxValue.ToString())
                .AssertValue(long.MaxValue);
    }
}