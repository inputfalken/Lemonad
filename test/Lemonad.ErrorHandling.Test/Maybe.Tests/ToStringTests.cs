using Lemonad.ErrorHandling.DataTypes.Maybe.Extensions;
using Xunit;

namespace Lemonad.ErrorHandling.Test.Maybe.Tests {
    public class ToStringTests {
        [Fact]
        public void None_Maybe_Char_With__Expects_String_To_have__Doble_Quotes() {
            var maybe = 'x'.None();
            Assert.False(maybe.HasValue, "Maybe Should not have value.");
            Assert.Equal("None ==> Maybe<Char>(\'x\')", maybe.ToString());
        }

        [Fact]
        public void None_Maybe_String_With_Content__Expects_String_To_have__Doble_Quotes() {
            var maybe = "hello".None();
            Assert.False(maybe.HasValue, "Maybe Should not have value.");
            Assert.Equal("None ==> Maybe<String>(\"hello\")", maybe.ToString());
        }

        [Fact]
        public void None_Maybe_String_With_Null_String__Expects_String_To_Be_Empty() {
            string hello = null;
            var maybe = hello.None();
            Assert.False(maybe.HasValue, "Maybe Should not have value.");
            Assert.Equal("None ==> Maybe<String>(null)", maybe.ToString());
        }

        [Fact]
        public void None_Maybe_String_Without_Content__Expects_String_To_have__Doble_Quotes() {
            var maybe = string.Empty.None();
            Assert.False(maybe.HasValue, "Maybe Should not have value.");
            Assert.Equal("None ==> Maybe<String>(\"\")", maybe.ToString());
        }

        [Fact]
        public void Some_Maybe_Char_With__Expects_String_To_have__Doble_Quotes() {
            var maybe = 'x'.Some();
            Assert.True(maybe.HasValue, "Should have value.");
            Assert.Equal("Some ==> Maybe<Char>(\'x\')", maybe.ToString());
        }

        [Fact]
        public void Some_Maybe_Integer_With__Expects_Integer_Inside_Parantheses() {
            var maybe = 2.Some();
            Assert.True(maybe.HasValue, "Should have value.");
            Assert.Equal($"Some ==> Maybe<Int32>({2})", maybe.ToString());
        }

        [Fact]
        public void Some_Maybe_Integer_With_Null_Integer__Expects_Integer_To_Be_Empty() {
            int? hello = null;
            var maybe = hello.Some();
            Assert.True(maybe.HasValue, "Should have value.");
            Assert.Equal("Some ==> Maybe<Nullable<Int32>>(null)", maybe.ToString());
        }

        [Fact]
        public void Some_Maybe_String_With_Content__Expects_String_To_have__Doble_Quotes() {
            var maybe = "hello".Some();
            Assert.True(maybe.HasValue, "Should have value.");
            Assert.Equal("Some ==> Maybe<String>(\"hello\")", maybe.ToString());
        }

        [Fact]
        public void Some_Maybe_String_With_Null_String__Expects_String_To_Be_Empty() {
            string hello = null;
            var maybe = hello.Some();
            Assert.True(maybe.HasValue, "Should have value.");
            Assert.Equal("Some ==> Maybe<String>(null)", maybe.ToString());
        }

        [Fact]
        public void Some_Maybe_String_Without_Content__Expects_String_To_have__Doble_Quotes() {
            var hello = string.Empty;
            var maybe = hello.Some();
            Assert.True(maybe.HasValue, "Should have value.");
            Assert.Equal("Some ==> Maybe<String>(\"\")", maybe.ToString());
        }
    }
}