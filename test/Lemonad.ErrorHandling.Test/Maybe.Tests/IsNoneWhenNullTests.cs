using Lemonad.ErrorHandling.Extensions;
using Xunit;

namespace Lemonad.ErrorHandling.Test.Maybe.Tests {
    public class NoneWhenNull {
        [Fact]
        public void Empty_String() {
            var maybe = "".Some().IsNoneWhenNull();
            Assert.True(maybe.HasValue, "Maybe should have a value, since the string is not null.");
            Assert.Equal(string.Empty, maybe.Value);
        }

        [Fact]
        public void Maybe_Overload__With_None_Empty_String() {
            var maybe = "".None().IsNoneWhenNull();
            Assert.False(maybe.HasValue, "Maybe should have a value, since the string is not null.");
            Assert.Equal(default, maybe.Value);
        }

        [Fact]
        public void Maybe_Overload__With_None_Null_string() {
            string str = null;
            var maybe = str.None().IsNoneWhenNull();
            Assert.False(maybe.HasValue, "Maybe not should have a value, since the string is null.");
            Assert.Equal(default, maybe.Value);
        }

        [Fact]
        public void Maybe_Overload__With_None_String_With_Content() {
            var maybe = "hello".None().IsNoneWhenNull();
            Assert.False(maybe.HasValue, "Maybe should have a value, since the string has content.");
            Assert.Equal(default, maybe.Value);
        }

        [Fact]
        public void Maybe_Overload__With_Some_Empty_String() {
            var some = "".Some();
            var maybe = some.IsNoneWhenNull();
            Assert.True(maybe.HasValue, "Maybe should have a value, since the string is not null.");
            Assert.Equal(string.Empty, maybe.Value);
        }

        [Fact]
        public void Maybe_Overload__With_Some_Null_string() {
            string str = null;
            var maybe = str.Some().IsNoneWhenNull();
            Assert.False(maybe.HasValue, "Maybe not should have a value, since the string is null.");
            Assert.Equal(default, maybe.Value);
        }

        [Fact]
        public void Maybe_Overload__With_Some_String_With_Content() {
            var maybe = "hello".Some().IsNoneWhenNull();
            Assert.True(maybe.HasValue, "Maybe should have a value, since the string has content.");
            Assert.Equal("hello", maybe.Value);
        }

        [Fact]
        public void Null_string() {
            string str = null;
            var maybe = str.Some().IsNoneWhenNull();
            Assert.False(maybe.HasValue, "Maybe not should have a value, since the string is null.");
            Assert.Equal(default, maybe.Value);
        }

        [Fact]
        public void String_With_Content() {
            var maybe = "hello".Some().IsNoneWhenNull();
            Assert.True(maybe.HasValue, "Maybe should have a value, since the string has content.");
            Assert.Equal("hello", maybe.Value);
        }
    }
}