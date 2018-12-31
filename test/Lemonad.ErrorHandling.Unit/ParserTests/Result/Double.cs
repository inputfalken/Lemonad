using Assertion;
using Xunit;

namespace Lemonad.ErrorHandling.Unit.ParserTests.Result {
    public class Double {
        [Fact]
        public void With_Valid_String()
            => Lemonad.ErrorHandling.Parsers.ResultParsers
                .Double("0.00000001")
                .AssertValue(0.00000001);

        [Fact]
        public void With_Invalid_String()
            => Lemonad.ErrorHandling.Parsers.ResultParsers
                .Double("foobar")
                .AssertError(
                    AssertionUtilities.FormatStringParserMessage<double>(
                        "foobar"
                    )
                );

        [Fact]
        public void With_Null_String()
            => Lemonad.ErrorHandling.Parsers.ResultParsers
                .Double(null)
                .AssertError(
                    AssertionUtilities.FormatStringParserMessage<double>(
                        null
                    )
                );
    }
}