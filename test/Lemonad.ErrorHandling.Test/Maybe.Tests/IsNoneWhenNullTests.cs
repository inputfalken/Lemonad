using Xunit;

namespace Lemonad.ErrorHandling.Test.Maybe.Tests {
    public class NoneWhenNull {
        [Fact]
        public void Empty_String() {
            var maybe = "".ToMaybe().IsNoneWhenNull();
            Assert.True(maybe.HasValue, "Maybe should have a value, since the string is not null.");
            Assert.Equal(string.Empty, maybe.Value);
        }

        [Fact]
        public void Maybe_Overload__With_None_Empty_String() {
            var maybe = "".ToMaybeNone().IsNoneWhenNull();
            Assert.False(maybe.HasValue, "Maybe should have a value, since the string is not null.");
            Assert.Equal(default, maybe.Value);
        }

        [Fact]
        public void Maybe_Overload__With_None_Null_string() {
            string str = null;
            var maybe = str.ToMaybeNone().IsNoneWhenNull();
            Assert.False(maybe.HasValue, "Maybe not should have a value, since the string is null.");
            Assert.Equal(default, maybe.Value);
        }

        [Fact]
        public void Maybe_Overload__With_None_String_With_Content() {
            var maybe = "hello".ToMaybeNone().IsNoneWhenNull();
            Assert.False(maybe.HasValue, "Maybe should have a value, since the string has content.");
            Assert.Equal(default, maybe.Value);
        }

        [Fact]
        public void Maybe_Overload__With_Some_Empty_String() {
            var some = "".ToMaybe();
            var maybe = some.IsNoneWhenNull();
            Assert.True(maybe.HasValue, "Maybe should have a value, since the string is not null.");
            Assert.Equal(string.Empty, maybe.Value);
        }

        [Fact]
        public void Maybe_Overload__With_Some_Null_string() {
            string str = null;
            var maybe = str.ToMaybe().IsNoneWhenNull();
            Assert.False(maybe.HasValue, "Maybe not should have a value, since the string is null.");
            Assert.Equal(default, maybe.Value);
        }

        [Fact]
        public void Maybe_Overload__With_Some_String_With_Content() {
            var maybe = "hello".ToMaybe().IsNoneWhenNull();
            Assert.True(maybe.HasValue, "Maybe should have a value, since the string has content.");
            Assert.Equal("hello", maybe.Value);
        }

        [Fact]
        public void Null_string() {
            string str = null;
            var maybe = str.ToMaybe().IsNoneWhenNull();
            Assert.False(maybe.HasValue, "Maybe not should have a value, since the string is null.");
            Assert.Equal(default, maybe.Value);
        }

        [Fact]
        public void String_With_Content() {
            var maybe = "hello".ToMaybe().IsNoneWhenNull();
            Assert.True(maybe.HasValue, "Maybe should have a value, since the string has content.");
            Assert.Equal("hello", maybe.Value);
        }
    }
}