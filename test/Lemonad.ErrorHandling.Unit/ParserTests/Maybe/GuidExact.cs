using Assertion;
using Lemonad.ErrorHandling.Parsers;
using Xunit;

namespace Lemonad.ErrorHandling.Unit.ParserTests.Maybe {
    public class GuidExact {
        [Fact]
        public void With_Valid_String_Digits_With_Four_HexaDecimals() {
            var guid = System.Guid.NewGuid().ToString("X");
            MaybeParsers
                .GuidExact(guid, GuidFormat.FourHexadecimalWrappedInBrackets)
                .AssertValue(System.Guid.Parse(guid));
        }

        [Fact]
        public void With_Null_String_Digits_With_Four_HexaDecimals() {
            MaybeParsers
                .GuidExact(null, GuidFormat.FourHexadecimalWrappedInBrackets)
                .AssertNone();
        }

        [Fact]
        public void With_Invalid_String_Digits_With_Four_HexaDecimals() {
            MaybeParsers
                .GuidExact("foobar", GuidFormat.FourHexadecimalWrappedInBrackets)
                .AssertNone();
        }

        [Fact]
        public void With_Valid_String_Digits_With_Hyphens_Wrapped_In_Parentheses() {
            var guid = System.Guid.NewGuid().ToString("P");
            MaybeParsers
                .GuidExact(guid, GuidFormat.DigitsWithHyphensWrappedInParentheses)
                .AssertValue(System.Guid.Parse(guid));
        }

        [Fact]
        public void With_Null_String_Digits_With_Hyphens_Wrapped_In_Parentheses() {
            MaybeParsers
                .GuidExact(null, GuidFormat.DigitsWithHyphensWrappedInParentheses)
                .AssertNone();
        }

        [Fact]
        public void With_Invalid_String_Digits_With_Hyphens_Wrapped_In_Parentheses() {
            MaybeParsers
                .GuidExact("foobar", GuidFormat.DigitsWithHyphensWrappedInParentheses)
                .AssertNone();
        }

        [Fact]
        public void With_Valid_String_Digits_With_Hyphens_Wrapped_In_Brackets() {
            var guid = System.Guid.NewGuid().ToString("B");
            MaybeParsers
                .GuidExact(guid, GuidFormat.DigitsWithHyphensWrappedInBrackets)
                .AssertValue(System.Guid.Parse(guid));
        }

        [Fact]
        public void With_Null_String_Digits_With_Hyphens_Wrapped_In_Brackets() {
            MaybeParsers
                .GuidExact(null, GuidFormat.DigitsWithHyphensWrappedInBrackets)
                .AssertNone();
        }

        [Fact]
        public void With_Invalid_String_Digits_With_Hyphens_Wrapped_In_Brackets() {
            MaybeParsers
                .GuidExact("foobar", GuidFormat.DigitsWithHyphensWrappedInBrackets)
                .AssertNone();
        }

        [Fact]
        public void With_Valid_String_Digits_With_Hyphens() {
            var guid = System.Guid.NewGuid().ToString("D");
            MaybeParsers
                .GuidExact(guid, GuidFormat.DigitsWithHyphens)
                .AssertValue(System.Guid.Parse(guid));
        }

        [Fact]
        public void With_Null_String_Digits_With_Hyphens() {
            MaybeParsers
                .GuidExact(null, GuidFormat.DigitsWithHyphens)
                .AssertNone();
        }

        [Fact]
        public void With_Invalid_String_Digits_With_Hyphens() {
            MaybeParsers
                .GuidExact("foobar", GuidFormat.DigitsWithHyphens)
                .AssertNone();
        }

        [Fact]
        public void With_Valid_String_Digits_Only() {
            var guid = System.Guid.NewGuid().ToString("N");
            MaybeParsers
                .GuidExact(guid, GuidFormat.DigitsOnly)
                .AssertValue(System.Guid.Parse(guid));
        }

        [Fact]
        public void With_Null_String_Digits_Only() {
            MaybeParsers
                .GuidExact(null, GuidFormat.DigitsOnly)
                .AssertNone();
        }

        [Fact]
        public void With_Invalid_String_Digits_Only() {
            MaybeParsers
                .GuidExact("foobar", GuidFormat.DigitsOnly)
                .AssertNone();
        }
    }
}