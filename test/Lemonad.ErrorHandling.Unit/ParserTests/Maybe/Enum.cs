using Assertion;
using Lemonad.ErrorHandling.Parsers;
using Xunit;

namespace Lemonad.ErrorHandling.Unit.ParserTests.Maybe {
    public class Enum {
        [Fact]
        public void With_Invalid_String()
            => MaybeParsers
                .Enum<AssertionUtilities.Gender>("foobar")
                .AssertNone();

        [Fact]
        public void With_Invalid_String_With_Ignore_Case()
            => MaybeParsers
                .Enum<AssertionUtilities.Gender>("foobar", true)
                .AssertNone();

        [Fact]
        public void With_Invalid_String_With_Ignore_Case_Set_To_False()
            => MaybeParsers
                .Enum<AssertionUtilities.Gender>("foobar", false)
                .AssertNone();

        [Fact]
        public void With_Null_String()
            => MaybeParsers
                .Enum<AssertionUtilities.Gender>(null)
                .AssertNone();

        [Fact]
        public void With_Valid_String()
            => MaybeParsers
                .Enum<AssertionUtilities.Gender>("Male")
                .AssertValue(AssertionUtilities.Gender.Male);

        [Fact]
        public void With_Valid_String_With_Ignore_Case()
            => MaybeParsers
                .Enum<AssertionUtilities.Gender>("malE", true)
                .AssertValue(AssertionUtilities.Gender.Male);

        [Fact]
        public void With_Valid_String_With_Ignore_Case_Set_To_False()
            => MaybeParsers
                .Enum<AssertionUtilities.Gender>("malE", false)
                .AssertNone();
    }
}