using Xunit;

namespace Lemonad.ErrorHandling.Test.Maybe.Tests {
    public class NoneWhenNull {
        [Fact]
        public void Empty_String() {
            var maybe = "".NoneWhenNull();
            Assert.True(maybe.HasValue, "Maybe should have a value, since the string is not null.");
            Assert.Equal(string.Empty, maybe.Value);
        }
        
        [Fact]
        public void Null_string() {
            string str = null;
            var maybe = str.NoneWhenNull();
            Assert.False(maybe.HasValue, "Maybe not should have a value, since the string is null.");
            Assert.Equal(default(string), maybe.Value);
        }
        
        [Fact]
        public void String_With_Content() {
            var maybe = "hello".NoneWhenNull();
            Assert.True(maybe.HasValue, "Maybe should have a value, since the string has content.");
            Assert.Equal("hello", maybe.Value);
        }
        
        [Fact]
        public void Maybe_Overload__With_Some_Empty_String() {
            var maybe = "".Some().NoneWhenNull();
            Assert.True(maybe.HasValue, "Maybe should have a value, since the string is not null.");
            Assert.Equal(string.Empty, maybe.Value);
        }
        
        [Fact]
        public void Maybe_Overload__With_Some_Null_string() {
            string str = null;
            var maybe = str.Some().NoneWhenNull();
            Assert.False(maybe.HasValue, "Maybe not should have a value, since the string is null.");
            Assert.Equal(default(string), maybe.Value);
        }
        
        [Fact]
        public void Maybe_Overload__With_Some_String_With_Content() {
            var maybe = "hello".Some().NoneWhenNull();
            Assert.True(maybe.HasValue, "Maybe should have a value, since the string has content.");
            Assert.Equal("hello", maybe.Value);
        }
        
        [Fact]
        public void Maybe_Overload__With_None_Empty_String() {
            var maybe = "".None().NoneWhenNull();
            Assert.False(maybe.HasValue, "Maybe should have a value, since the string is not null.");
            Assert.Equal(default(string), maybe.Value);
        }
        
        [Fact]
        public void Maybe_Overload__With_None_Null_string() {
            string str = null;
            var maybe = str.None().NoneWhenNull();
            Assert.False(maybe.HasValue, "Maybe not should have a value, since the string is null.");
            Assert.Equal(default(string), maybe.Value);
        }
        
        [Fact]
        public void Maybe_Overload__With_None_String_With_Content() {
            var maybe = "hello".None().NoneWhenNull();
            Assert.False(maybe.HasValue, "Maybe should have a value, since the string has content.");
            Assert.Equal(default(string), maybe.Value);
        }
    }
}