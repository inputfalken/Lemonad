using Assertion;
using Xunit;

namespace Lemonad.ErrorHandling.Unit.ParserTests.Maybe {
    public class Enum {
        [Fact]
        public void With_Valid_String()
            => Lemonad.ErrorHandling.Parsers.MaybeParsers
                .Enum<AssertionUtilities.Gender>("Male")
                .AssertValue(AssertionUtilities.Gender.Male);

        [Fact]
        public void With_Valid_String_With_Ignore_Case_Set_To_False()
            => Lemonad.ErrorHandling.Parsers.MaybeParsers
                .Enum<AssertionUtilities.Gender>("malE", ignoreCase: false)
                .AssertNone();

        [Fact]
        public void With_Invalid_String_With_Ignore_Case_Set_To_False()
            => Lemonad.ErrorHandling.Parsers.MaybeParsers
                .Enum<AssertionUtilities.Gender>("foobar", ignoreCase: false)
                .AssertNone();

        [Fact]
        public void With_Invalid_String_With_Ignore_Case()
            => Lemonad.ErrorHandling.Parsers.MaybeParsers
                .Enum<AssertionUtilities.Gender>("foobar", ignoreCase: true)
                .AssertNone();

        [Fact]
        public void With_Valid_String_With_Ignore_Case()
            => Lemonad.ErrorHandling.Parsers.MaybeParsers
                .Enum<AssertionUtilities.Gender>("malE", ignoreCase: true)
                .AssertValue(AssertionUtilities.Gender.Male);

        [Fact]
        public void With_Invalid_String()
            => Lemonad.ErrorHandling.Parsers.MaybeParsers
                .Enum<AssertionUtilities.Gender>("foobar")
                .AssertNone();

        [Fact]
        public void With_Null_String()
            => Lemonad.ErrorHandling.Parsers.MaybeParsers
                .Enum<AssertionUtilities.Gender>(null)
                .AssertNone();
    }
}