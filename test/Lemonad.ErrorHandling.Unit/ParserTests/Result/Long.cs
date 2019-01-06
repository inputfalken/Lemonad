using System.Globalization;
using Assertion;
using Lemonad.ErrorHandling.Parsers;
using Xunit;

namespace Lemonad.ErrorHandling.Unit.ParserTests.Result {
    public class Long {
        [Fact]
        public void With_Invalid_String()
            => ResultParsers
                .Long("foobar")
                .AssertError(
                    AssertionUtilities.FormatStringParserMessage<long>(
                        "foobar"
                    )
                );

        [Fact]
        public void With_Invalid_String_With_NumberStyle()
            => ResultParsers
                .Long("foobar", NumberStyles.Any, CultureInfo.InvariantCulture)
                .AssertError(
                    AssertionUtilities.FormatStringParserMessage<long>(
                        "foobar"
                    )
                );

        [Fact]
        public void With_Null_String()
            => ResultParsers
                .Long(null)
                .AssertError(
                    AssertionUtilities.FormatStringParserMessage<long>(
                        null
                    )
                );

        [Fact]
        public void With_Null_String_With_NumberStyle()
            => ResultParsers
                .Long(null, NumberStyles.Any, CultureInfo.InvariantCulture)
                .AssertError(
                    AssertionUtilities.FormatStringParserMessage<long>(
                        null
                    )
                );

        [Fact]
        public void With_Valid_String()
            => ResultParsers
                .Long(long.MaxValue.ToString(), NumberStyles.Any, CultureInfo.InvariantCulture)
                .AssertValue(long.MaxValue);

        [Fact]
        public void With_Valid_String_With_NumberStyle()
            => ResultParsers
                .Long(long.MaxValue.ToString())
                .AssertValue(long.MaxValue);
    }
}