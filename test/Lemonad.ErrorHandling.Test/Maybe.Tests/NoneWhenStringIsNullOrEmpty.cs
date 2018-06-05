using System;
using Xunit;

namespace Lemonad.ErrorHandling.Test.Maybe.Tests {
    public class NoneWhenStringIsNullOrEmpty {
        [Fact]
        public void Empty_String() {
            var noneWhenStringIsNullOrEmpty = string.Empty.NoneWhenStringIsNullOrWhiteSpace();

            Assert.False(noneWhenStringIsNullOrEmpty.HasValue);
            Assert.Equal(default(string), noneWhenStringIsNullOrEmpty.Value);
        }

        [Fact]
        public void Null_String() {
            string f = null;
            var noneWhenStringIsNullOrEmpty = f.NoneWhenStringIsNullOrWhiteSpace();

            Assert.False(noneWhenStringIsNullOrEmpty.HasValue);
            Assert.Equal(default(string), noneWhenStringIsNullOrEmpty.Value);
        }

        [Fact]
        public void String_With_Content() {
            var noneWhenStringIsNullOrEmpty = "hello".NoneWhenStringIsNullOrWhiteSpace();

            Assert.True(noneWhenStringIsNullOrEmpty.HasValue);
            Assert.Equal("hello", noneWhenStringIsNullOrEmpty.Value);
        }
    }
}
