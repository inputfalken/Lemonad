using System.Globalization;
using Assertion;
using Xunit;

namespace Lemonad.ErrorHandling.Unit.ParserTests.Result {
    public class DateTime {
        [Fact]
        public void With_Valid_String()
            => Lemonad.ErrorHandling.Parsers.ResultParsers
                .DateTime(System.DateTime.Today.ToString(CultureInfo.InvariantCulture))
                .AssertValue(System.DateTime.Today);

        [Fact]
        public void With_Invalid_String()
            => Lemonad.ErrorHandling.Parsers.ResultParsers
                .DateTime("foobar")
                .AssertError(
                    AssertionUtilities.FormatStringParserMessage<System.DateTime>(
                        "foobar"
                    )
                );

        [Fact]
        public void With_Null_String()
            => Lemonad.ErrorHandling.Parsers.ResultParsers
                .DateTime(null)
                .AssertError(
                    AssertionUtilities.FormatStringParserMessage<System.DateTime>(
                        null
                    )
                );
    }
}