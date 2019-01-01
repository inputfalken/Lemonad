using Assertion;
using Xunit;

namespace Lemonad.ErrorHandling.Unit.ParserTests.Result {
    public class Int {
        [Fact]
        public void With_Valid_String()
            => Parsers.ResultParsers
                .Int("20")
                .AssertValue(20);

        [Fact]
        public void With_Invalid_String()
            => Parsers.ResultParsers
                .Int("foobar")
                .AssertError(
                    AssertionUtilities.FormatStringParserMessage<int>(
                        "foobar"
                    )
                );

        [Fact]
        public void With_Null_String()
            => Parsers.ResultParsers
                .Int(null)
                .AssertError(
                    AssertionUtilities.FormatStringParserMessage<int>(
                        null
                    )
                );
    }
}