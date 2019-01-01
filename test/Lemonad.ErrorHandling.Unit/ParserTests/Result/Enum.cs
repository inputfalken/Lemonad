using Assertion;
using Xunit;

namespace Lemonad.ErrorHandling.Unit.ParserTests.Result {
    public class Enum {
        [Fact]
        public void With_Valid_String()
            => Parsers.ResultParsers
                .Enum<AssertionUtilities.Gender>("Male")
                .AssertValue(AssertionUtilities.Gender.Male);

        [Fact]
        public void With_Valid_String_With_Ignore_Case()
            => Parsers.ResultParsers
                .Enum<AssertionUtilities.Gender>("malE", ignoreCase: true)
                .AssertValue(AssertionUtilities.Gender.Male);

        [Fact]
        public void With_Invalid_String()
            => Parsers.ResultParsers
                .Enum<AssertionUtilities.Gender>("foobar")
                .AssertError(
                    AssertionUtilities.FormatStringParserMessage<AssertionUtilities.Gender>(
                        "foobar"
                    )
                );

        [Fact]
        public void With_Valid_String_With_Ignore_Case_Set_To_False()
            => Parsers.ResultParsers
                .Enum<AssertionUtilities.Gender>("malE", ignoreCase: false)
                .AssertError(
                    AssertionUtilities.FormatStringParserMessage<AssertionUtilities.Gender>(
                        "malE"
                    )
                );

        [Fact]
        public void With_Invalid_String_With_Ignore_Case_Set_To_False()
            => Parsers.ResultParsers
                .Enum<AssertionUtilities.Gender>("foobar", ignoreCase: false)
                .AssertError(
                    AssertionUtilities.FormatStringParserMessage<AssertionUtilities.Gender>(
                        "foobar"
                    )
                );

        [Fact]
        public void With_Invalid_String_With_Ignore_Case()
            => Parsers.ResultParsers
                .Enum<AssertionUtilities.Gender>("foobar", ignoreCase: true)
                .AssertError(
                    AssertionUtilities.FormatStringParserMessage<AssertionUtilities.Gender>(
                        "foobar"
                    )
                );

        [Fact]
        public void With_Null_String()
            => Parsers.ResultParsers
                .Enum<AssertionUtilities.Gender>(null)
                .AssertError(
                    AssertionUtilities.FormatStringParserMessage<AssertionUtilities.Gender>(
                        null
                    )
                );
    }
}