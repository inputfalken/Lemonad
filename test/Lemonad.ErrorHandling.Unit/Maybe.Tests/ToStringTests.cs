using Assertion;
using Lemonad.ErrorHandling.Exceptions;
using Lemonad.ErrorHandling.Extensions;
using Lemonad.ErrorHandling.Internal;
using Xunit;

namespace Lemonad.ErrorHandling.Unit.Maybe.Tests {
    /// <summary>
    ///     Maybe is based on <see cref="Result{T,TError}" /> and Result currently does not support showing the value of
    ///     TError. instead it will be default keyword.
    /// </summary>
    public class ToStringTests {
        [Fact]
        public void None_Maybe_Char_With__Expects_String_To_have__Doble_Quotes() {
            var maybe = 'x'.ToMaybeNone().AssertNone();
            Assert.Equal($@"None ==> Maybe<Char>('{default(char)}')", maybe.ToString());
        }

        [Fact]
        public void None_Maybe_String_With_Content__Expects_String_To_have__Doble_Quotes() {
            var maybe = "hello".ToMaybeNone().AssertNone();
            Assert.Equal(@"None ==> Maybe<String>(null)", maybe.ToString());
        }

        [Fact]
        public void None_Maybe_String_With_Null_String__Expects_String_To_Be_Empty() {
            string hello = null;
            var maybe = hello.ToMaybeNone().AssertNone();
            Assert.Equal("None ==> Maybe<String>(null)", maybe.ToString());
        }

        [Fact]
        public void None_Maybe_String_Without_Content__Expects_String_To_have__Doble_Quotes() {
            var maybe = string.Empty.ToMaybeNone().AssertNone();
            Assert.Equal(@"None ==> Maybe<String>(null)", maybe.ToString());
        }

        [Fact]
        public void Some_Maybe_Char_With__Expects_String_To_have__Doble_Quotes() {
            var maybe = ErrorHandling.Maybe.Value('x').AssertValue('x');
            Assert.Equal("Some ==> Maybe<Char>(\'x\')", maybe.ToString());
        }

        [Fact]
        public void Some_Maybe_Integer_With__Expects_Integer_Inside_Parantheses() {
            var maybe = ErrorHandling.Maybe.Value(2).AssertValue(2);
            Assert.Equal($"Some ==> Maybe<Int32>({2})", maybe.ToString());
        }

        [Fact]
        public void Some_Maybe_Integer_With_Null_Integer__Expects_Integer_To_Be_Empty() {
            int? hello = null;
            var maybe = hello.ToMaybe().AssertNone();
            Assert.Equal("None ==> Maybe<Int32>(0)", maybe.ToString());
        }

        [Fact]
        public void Some_Maybe_String_With_Content__Expects_String_To_have__Doble_Quotes() {
            var maybe = ErrorHandling.Maybe.Value("hello").AssertValue("hello");
            Assert.Equal("Some ==> Maybe<String>(\"hello\")", maybe.ToString());
        }

        [Fact]
        public void Some_Maybe_String_With_Null_String__Expects_String_To_Be_Empty() {
            Assert.Throws<InvalidMaybeStateException>(() => {
                string hello = null;
                var maybe = ErrorHandling.Maybe.Value(hello).AssertValue(null);
                Assert.Equal("Some ==> Maybe<String>(null)", maybe.ToString());
            });
        }

        [Fact]
        public void Some_Maybe_String_Without_Content__Expects_String_To_have__Doble_Quotes() {
            var hello = string.Empty;
            var maybe = ErrorHandling.Maybe.Value(hello).AssertValue(string.Empty);
            Assert.Equal("Some ==> Maybe<String>(\"\")", maybe.ToString());
        }
    }
}