using Assertion;
using Lemonad.ErrorHandling.Extensions.String;
using Xunit;

namespace Lemonad.ErrorHandling.Unit.ExtensionTests.String {
    public class IsNullOrWhiteSpace {
        [Fact]
        public void Empty_String() => string.Empty.IsNullOrWhiteSpace().AssertError(IsNullOrWhiteSpaceCase.Empty);

        [Fact]
        public void Null_String() => ((string) null).IsNullOrWhiteSpace().AssertError(IsNullOrWhiteSpaceCase.Null);

        [Fact]
        public void String_With_Word() => "foobar".IsNullOrWhiteSpace().AssertValue("foobar");

        [Fact]
        public void String_Mixed_With_Word_And_WhiteSpace() => " foobar ".IsNullOrWhiteSpace().AssertValue(" foobar ");

        [Fact]
        public void String_With_Space() => " ".IsNullOrWhiteSpace().AssertError(IsNullOrWhiteSpaceCase.WhiteSpace);

        [Fact]
        public void String_With_Newline() => "\n".IsNullOrWhiteSpace().AssertError(IsNullOrWhiteSpaceCase.WhiteSpace);

        [Fact]
        public void String_With_Tab() => "\n".IsNullOrWhiteSpace().AssertError(IsNullOrWhiteSpaceCase.WhiteSpace);
    }
}