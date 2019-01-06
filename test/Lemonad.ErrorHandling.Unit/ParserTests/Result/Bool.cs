using Assertion;
using Lemonad.ErrorHandling.Parsers;
using Xunit;

namespace Lemonad.ErrorHandling.Unit.ParserTests.Result {
    public class Bool {
        [Fact]
        public void With_Invalid_String()
            => ResultParsers
                .Bool("foobar")
                .AssertError(AssertionUtilities.FormatStringParserMessage<bool>("foobar"));

        [Fact]
        public void With_Null_String()
            => ResultParsers
                .Bool(null)
                .AssertError(AssertionUtilities.FormatStringParserMessage<bool>(null));

        [Fact]
        public void With_Valid_String_For_False()
            => ResultParsers.Bool("false").AssertValue(false);

        [Fact]
        public void With_Valid_String_For_True()
            => ResultParsers.Bool("true").AssertValue(true);
    }
}