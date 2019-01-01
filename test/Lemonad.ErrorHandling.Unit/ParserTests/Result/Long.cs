using Assertion;
using Xunit;

namespace Lemonad.ErrorHandling.Unit.ParserTests.Result {
    public class Long {
        [Fact]
        public void With_Valid_String()
            => Parsers.ResultParsers
                .Long(long.MaxValue.ToString())
                .AssertValue(long.MaxValue);

        [Fact]
        public void With_Invalid_String()
            => Parsers.ResultParsers
                .Long("foobar")
                .AssertError(
                    AssertionUtilities.FormatStringParserMessage<long>(
                        "foobar"
                    )
                );

        [Fact]
        public void With_Null_String()
            => Parsers.ResultParsers
                .Long(null)
                .AssertError(
                    AssertionUtilities.FormatStringParserMessage<long>(
                        null
                    )
                );
    }
}