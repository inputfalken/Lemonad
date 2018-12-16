using System;
using Lemonad.ErrorHandling.Extensions;
using Xunit;

namespace Lemonad.ErrorHandling.Unit.Maybe.Tests {
    public class ToMaybeNoneTests {
        [Fact]
        public void Empty_String() {
            var noneWhenStringIsNullOrEmpty = string.Empty.ToMaybeNone(string.IsNullOrWhiteSpace);

            Assert.False(noneWhenStringIsNullOrEmpty.HasValue);
            Assert.Equal(default, noneWhenStringIsNullOrEmpty.Value);
        }

        [Fact]
        public void Null_String_With_Value_Should_Throw() {
            Assert.Throws<ArgumentNullException>(AssertionUtilities.MaybeValueName, () => {
                string f = null;
                var noneWhenStringIsNullOrEmpty = f.ToMaybeNone(_ => false);

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