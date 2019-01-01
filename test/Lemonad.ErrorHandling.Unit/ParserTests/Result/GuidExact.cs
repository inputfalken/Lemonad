using Assertion;
using Lemonad.ErrorHandling.Parsers;
using Xunit;

namespace Lemonad.ErrorHandling.Unit.ParserTests.Result {
    public class GuidExact {
        [Fact]
        public void With_Valid_String_Digits_With_Four_HexaDecimals() {
            var guid = System.Guid.NewGuid().ToString("X");
            ResultParsers
                .GuidExact(guid, GuidFormat.FourHexadecimalWrappedInBrackets)
                .AssertValue(System.Guid.Parse(guid));
        }

        [Fact]
        public void With_Null_String_Digits_With_Four_HexaDecimals() =>
            ResultParsers
                .GuidExact(null, GuidFormat.FourHexadecimalWrappedInBrackets)
                .AssertError(AssertionUtilities.FormatStringParserMessage<Guid>(null));

        [Fact]
        public void With_Invalid_String_Digits_With_Four_HexaDecimals() =>
            ResultParsers
                .GuidExact("foobar", GuidFormat.FourHexadecimalWrappedInBrackets)
                .AssertError(AssertionUtilities.FormatStringParserMessage<Guid>("foobar"));

        [Fact]
        public void With_Valid_String_Digits_With_Hyphens_Wrapped_In_Parentheses() {
            var guid = System.Guid.NewGuid().ToString("P");
            ResultParsers
                .GuidExact(guid, GuidFormat.DigitsWithHyphensWrappedInParentheses)
                .AssertValue(System.Guid.Parse(guid));
        }

        [Fact]
        public void With_Null_String_Digits_With_Hyphens_Wrapped_In_Parentheses() =>
            ResultParsers
                .GuidExact(null, GuidFormat.DigitsWithHyphensWrappedInParentheses)
                .AssertError(AssertionUtilities.FormatStringParserMessage<Guid>(null));

        [Fact]
        public void With_Invalid_String_Digits_With_Hyphens_Wrapped_In_Parentheses() {
            var guid = "foobar";
            ResultParsers
                .GuidExact(guid, GuidFormat.DigitsWithHyphensWrappedInParentheses)
                .AssertError(AssertionUtilities.FormatStringParserMessage<Guid>("foobar"));
        }

        [Fact]
        public void With_Valid_String_Digits_With_Hyphens_Wrapped_In_Brackets() {
            var guid = System.Guid.NewGuid().ToString("B");
            ResultParsers
                .GuidExact(guid, GuidFormat.DigitsWithHyphensWrappedInBrackets)
                .AssertValue(System.Guid.Parse(guid));
        }

        [Fact]
        public void With_Null_String_Digits_With_Hyphens_Wrapped_In_Brackets() =>
            ResultParsers
                .GuidExact(null, GuidFormat.DigitsWithHyphensWrappedInBrackets)
                .AssertError(AssertionUtilities.FormatStringParserMessage<Guid>(null));

        [Fact]
        public void With_Invalid_String_Digits_With_Hyphens_Wrapped_In_Brackets() {
            var guid = "foobar";
            ResultParsers
                .GuidExact(guid, GuidFormat.DigitsWithHyphensWrappedInBrackets)
                .AssertError(AssertionUtilities.FormatStringParserMessage<Guid>("foobar"));
        }

        [Fact]
        public void With_Valid_String_Digits_With_Hyphens() {
            var guid = System.Guid.NewGuid().ToString("D");
            ResultParsers
                .GuidExact(guid, GuidFormat.DigitsWithHyphens)
                .AssertValue(System.Guid.Parse(guid));
        }

        [Fact]
        public void With_Null_String_Digits_With_Hyphens() =>
            ResultParsers
                .GuidExact(null, GuidFormat.DigitsWithHyphens)
                .AssertError(AssertionUtilities.FormatStringParserMessage<Guid>(null));

        [Fact]
        public void With_Invalid_String_Digits_With_Hyphens() =>
            ResultParsers
                .GuidExact("foobar", GuidFormat.DigitsWithHyphens)
                .AssertError(AssertionUtilities.FormatStringParserMessage<Guid>("foobar"));

        [Fact]
        public void With_Valid_String_Digits_Only() {
            var guid = System.Guid.NewGuid().ToString("N");
            ResultParsers
                .GuidExact(guid, GuidFormat.DigitsOnly)
                .AssertValue(System.Guid.Parse(guid));
        }

        [Fact]
        public void With_Invalid_String_Digits_Only() =>
            ResultParsers
                .GuidExact("foobar", GuidFormat.DigitsOnly)
                .AssertError(AssertionUtilities.FormatStringParserMessage<Guid>("foobar"));

        [Fact]
        public void With_Null_String_Digits_Only() =>
            ResultParsers
                .GuidExact(null, GuidFormat.DigitsOnly)
                .AssertError(AssertionUtilities.FormatStringParserMessage<Guid>(null));
    }
}