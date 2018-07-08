using Lemonad.ErrorHandling.Extensions;
using Xunit;

namespace Lemonad.ErrorHandling.Test.Maybe.Tests {
    public class NoneWhenStringIsNullOrEmpty {
        [Fact]
        public void Empty_String() {
            var noneWhenStringIsNullOrEmpty = string.Empty.None(string.IsNullOrWhiteSpace);

            Assert.False(noneWhenStringIsNullOrEmpty.HasValue);
            Assert.Equal(default, noneWhenStringIsNullOrEmpty.Value);
        }

        [Fact]
        public void Null_String() {
            string f = null;
            var noneWhenStringIsNullOrEmpty = f.None(string.IsNullOrWhiteSpace);

            Assert.False(noneWhenStringIsNullOrEmpty.HasValue);
            Assert.Equal(default, noneWhenStringIsNullOrEmpty.Value);
        }

        [Fact]
        public void String_With_Content() {
            var noneWhenStringIsNullOrEmpty = "hello".None(string.IsNullOrWhiteSpace);

            Assert.True(noneWhenStringIsNullOrEmpty.HasValue);
            Assert.Equal("hello", noneWhenStringIsNullOrEmpty.Value);
        }
    }
}