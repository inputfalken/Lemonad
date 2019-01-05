using Assertion;
using Lemonad.ErrorHandling.Parsers;
using Xunit;

namespace Lemonad.ErrorHandling.Unit.ParserTests.Result {
    public class Guid {
        [Fact]
        public void With_Invalid_String()
            => ResultParsers
                .Guid("foobar")
                .AssertError(
                    AssertionUtilities.FormatStringParserMessage<System.Guid>(
                        "foobar"
                    )
                );

        [Fact]
        public void With_Null_String()
            => ResultParsers
                .Guid(null)
                .AssertError(
                    AssertionUtilities.FormatStringParserMessage<System.Guid>(
                        null
                    )
                );

        [Fact]
        public void With_Valid_String() {
            var guid = System.Guid.NewGuid();
            ResultParsers
                .Guid(guid.ToString())
                .AssertValue(guid);
        }
    }
}