using Assertion;
using Xunit;

namespace Lemonad.ErrorHandling.Unit.ParserTests.Result {
    public class Bool {
        [Fact]
        public void With_Valid_String_For_True()
            => Lemonad.ErrorHandling.Parsers.ResultParsers.Bool("true").AssertValue(true);

        [Fact]
        public void With_Valid_String_For_False()
            => Lemonad.ErrorHandling.Parsers.ResultParsers.Bool("false").AssertValue(false);

        [Fact]
        public void With_Invalid_String()
            => Lemonad.ErrorHandling.Parsers.ResultParsers
                .Bool("foobar")
                .AssertError(AssertionUtilities.FormatStringParserMessage<bool>("foobar"));

        [Fact]
        public void With_Null_String()
            => Lemonad.ErrorHandling.Parsers.ResultParsers
                .Bool(null)
                .AssertError(AssertionUtilities.FormatStringParserMessage<bool>(null));
    }
}