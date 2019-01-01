using Assertion;
using Lemonad.ErrorHandling.Extensions.String;
using Xunit;

namespace Lemonad.ErrorHandling.Unit.ExtensionTests.String {
    public class IsNullOrEmpty {
        [Fact]
        public void Empty_String() => string.Empty.IsNullOrEmpty().AssertError(IsNullOrEmptyCase.Empty);

        [Fact]
        public void Null_String() => ((string) null).IsNullOrEmpty().AssertError(IsNullOrEmptyCase.Null);

        [Fact]
        public void String_With_Word() => "foobar".IsNullOrEmpty().AssertValue("foobar");
        
        [Fact]
        public void String_Mixed_With_Word_And_WhiteSpace() => " foobar ".IsNullOrEmpty().AssertValue(" foobar ");

        [Fact]
        public void String_With_Space() => " ".IsNullOrEmpty().AssertValue(" ");
        
        [Fact]
        public void String_With_Newline() => "\n".IsNullOrEmpty().AssertValue("\n");
        
        [Fact]
        public void String_With_Tab() => "\n".IsNullOrEmpty().AssertValue("\n");
    }
}