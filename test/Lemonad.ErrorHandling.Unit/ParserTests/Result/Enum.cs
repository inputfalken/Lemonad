using Assertion;
using Lemonad.ErrorHandling.Parsers;
using Xunit;

namespace Lemonad.ErrorHandling.Unit.ParserTests.Result {
    public class Enum {
        [Fact]
        public void With_Invalid_String()
            => ResultParsers
                .Enum<AssertionUtilities.Gender>("foobar")
                .AssertError(
                    AssertionUtilities.FormatStringParserMessage<AssertionUtilities.Gender>(
                        "foobar"
                    )
                );

        [Fact]
        public void With_Invalid_String_With_Ignore_Case()
            => ResultParsers
                .Enum<AssertionUtilities.Gender>("foobar", true)
                .AssertError(
                    AssertionUtilities.FormatStringParserMessage<AssertionUtilities.Gender>(
                        "foobar"
                    )
                );

        [Fact]
        public void With_Invalid_String_With_Ignore_Case_Set_To_False()
            => ResultParsers
                .Enum<AssertionUtilities.Gender>("foobar", false)
                .AssertError(
                    AssertionUtilities.FormatStringParserMessage<AssertionUtilities.Gender>(
                        "foobar"
                    )
                );

        [Fact]
        public void With_Null_String()
            => ResultParsers
                .Enum<AssertionUtilities.Gender>(null)
                .AssertError(
                    AssertionUtilities.FormatStringParserMessage<AssertionUtilities.Gender>(
                        null
                    )
                );

        [Fact]
        public void With_Valid_String()
            => ResultParsers
                .Enum<AssertionUtilities.Gender>("Male")
                .AssertValue(AssertionUtilities.Gender.Male);

        [Fact]
        public void With_Valid_String_With_Ignore_Case()
            => ResultParsers
                .Enum<AssertionUtilities.Gender>("malE", true)
                .AssertValue(AssertionUtilities.Gender.Male);

        [Fact]
        public void With_Valid_String_With_Ignore_Case_Set_To_False()
            => ResultParsers
                .Enum<AssertionUtilities.Gender>("malE", false)
                .AssertError(
                    AssertionUtilities.FormatStringParserMessage<AssertionUtilities.Gender>(
                        "malE"
                    )
                );
    }
}