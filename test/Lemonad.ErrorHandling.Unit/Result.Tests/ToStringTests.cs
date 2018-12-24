using Assertion;
using Lemonad.ErrorHandling.Extensions;
using Lemonad.ErrorHandling.Extensions.Maybe;
using Xunit;

namespace Lemonad.ErrorHandling.Unit.Result.Tests {
    public class ToStringTests {
        [Fact]
        public void Error_Result_With_Char__Expects_Char_To_have__Single_Quotes() {
            var result = 2.ToMaybeNone()
                .ToResult(() => 'I')
                .AssertError('I');

            Assert.Equal("Error ==> Result<Int32, Char>('I')", result.ToString());
        }

        [Fact]
        public void Error_Result_With_Int() {
            string hello = null;
            var result = hello.ToMaybeNone()
                .ToResult(() => 2)
                .AssertError(2);

            Assert.Equal("Error ==> Result<String, Int32>(2)", result.ToString());
        }

        [Fact]
        public void Error_Result_With_String__Expects_String_To_have__Doble_Quotes() {
            var result = 2.ToMaybeNone()
                .ToResult(() => "hello")
                .AssertError("hello");

            Assert.Equal("Error ==> Result<Int32, String>(\"hello\")", result.ToString());
        }

        [Fact]
        public void Ok_Result_With_Char__Expects_Char_To_have__Single_Quotes() {
            var result = ErrorHandling.Maybe
                .Value('I')
                .ToResult(() => 2)
                .AssertValue('I');

            Assert.Equal("Ok ==> Result<Char, Int32>('I')", result.ToString());
        }

        [Fact]
        public void Ok_Result_With_String__Expects_String_To_have__Doble_Quotes() {
            var result = ErrorHandling.Maybe
                .Value("hello")
                .ToResult(() => 2)
                .AssertValue("hello");

            Assert.Equal("Ok ==> Result<String, Int32>(\"hello\")", result.ToString());
        }

        [Fact]
        public void Ok_Result_With_String_Using_Backslash__Expects_String_To_have__Backslash() {
            var result = ErrorHandling.Maybe
                .Value("hello\\")
                .ToResult(() => 2)
                .AssertValue("hello\\");

            Assert.Equal("Ok ==> Result<String, Int32>(\"hello\\\")", result.ToString());
        }

        [Fact]
        public void Ok_Result_With_String_Using_NewLines__Expects_String_To_have__Escaped_Values() {
            var result = ErrorHandling.Maybe
                .Value("hello\r\nfoo")
                .ToResult(() => 2)
                .AssertValue("hello\r\nfoo");

            Assert.Equal("Ok ==> Result<String, Int32>(\"hello\r\nfoo\")", result.ToString());
        }

        [Fact]
        public void Ok_Result_With_String_Using_Tab__Expects_String_To_have__Escaped_Values() {
            var result = ErrorHandling.Maybe
                .Value("hello\tfoo")
                .ToResult(() => 2)
                .AssertValue("hello\tfoo");

            Assert.Equal("Ok ==> Result<String, Int32>(\"hello\tfoo\")", result.ToString());
        }
    }
}