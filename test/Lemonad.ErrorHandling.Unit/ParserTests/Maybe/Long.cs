using System.Globalization;
using Assertion;
using Xunit;

namespace Lemonad.ErrorHandling.Unit.ParserTests.Maybe {
    public class Long {
        [Fact]
        public void With_Valid_String_With_NumberStyle()
            => Parsers.MaybeParsers
                .Long(long.MaxValue.ToString())
                .AssertValue(long.MaxValue);

        [Fact]
        public void With_Invalid_String_With_NumberStyle()
            => Parsers.MaybeParsers
                .Long("foobar", NumberStyles.Any, CultureInfo.InvariantCulture)
                .AssertNone();

        [Fact]
        public void With_Null_String_With_NumberStyle()
            => Parsers.MaybeParsers
                .Long(null, NumberStyles.Any, CultureInfo.InvariantCulture)
                .AssertNone();

        [Fact]
        public void With_Valid_String()
            => Parsers.MaybeParsers
                .Long(long.MaxValue.ToString())
                .AssertValue(long.MaxValue);

        [Fact]
        public void With_Invalid_String()
            => Parsers.MaybeParsers
                .Long("foobar")
                .AssertNone();

        [Fact]
        public void With_Null_String()
            => Parsers.MaybeParsers
                .Long(null)
                .AssertNone();
    }
}