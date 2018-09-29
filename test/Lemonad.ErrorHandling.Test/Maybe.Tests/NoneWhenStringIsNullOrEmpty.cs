using System;
using Xunit;

namespace Lemonad.ErrorHandling.Test.Maybe.Tests {
    public class NoneWhenStringIsNullOrEmpty {
        [Fact]
        public void Empty_String() {
            var noneWhenStringIsNullOrEmpty = string.Empty.ToMaybeNone(string.IsNullOrWhiteSpace);

            Assert.False(noneWhenStringIsNullOrEmpty.HasValue);
            Assert.Equal(default, noneWhenStringIsNullOrEmpty.Value);
        }

        [Fact]
        public void Null_String() {
            Assert.Throws<ArgumentNullException>(AssertionUtilities.EitherValueName, () => {
                string f = null;
                var noneWhenStringIsNullOrEmpty = f.ToMaybeNone(string.IsNullOrWhiteSpace);

                Assert.False(noneWhenStringIsNullOrEmpty.HasValue);
                Assert.Equal(default, noneWhenStringIsNullOrEmpty.Value);
            });
        }

        [Fact]
        public void String_With_Content() {
            var noneWhenStringIsNullOrEmpty = "hello".ToMaybeNone(string.IsNullOrWhiteSpace);

            Assert.True(noneWhenStringIsNullOrEmpty.HasValue);
            Assert.Equal("hello", noneWhenStringIsNullOrEmpty.Value);
        }
    }
}