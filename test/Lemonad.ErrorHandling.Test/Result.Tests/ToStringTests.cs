using Xunit;

namespace Lemonad.ErrorHandling.Test.Result.Tests {
    public class ToStringTests {
        [Fact]
        public void Error_Result_With_Char__Expects_Char_To_have__Single_Quotes() {
            var result = 2.ToMaybeNone().ToResult(x => 'I');

            Assert.True(result.Either.HasError, "Result should have a error value.");
            Assert.False(result.Either.HasValue, "Result should not have a Ok value.");
            Assert.Equal(default, result.Either.Value);
            Assert.Equal('I', result.Either.Error);
            Assert.Equal("Error ==> Result<Int32, Char>('I')", result.ToString());
        }

        [Fact]
        public void Error_Result_With_Int() {
            string hello = null;
            var result = hello.ToMaybeNone().ToResult(x => 2);

            Assert.True(result.Either.HasError, "Result should have a error value.");
            Assert.False(result.Either.HasValue, "Result should not have a Ok value.");
            Assert.Equal(2, result.Either.Error);
            Assert.Equal(default, result.Either.Value);
            Assert.Equal("Error ==> Result<String, Int32>(2)", result.ToString());
        }

        [Fact]
        public void Error_Result_With_String__Expects_String_To_have__Doble_Quotes() {
            var result = 2.ToMaybeNone().ToResult(x => "hello");

            Assert.True(result.Either.HasError, "Result should have a error value.");
            Assert.False(result.Either.HasValue, "Result should not have a Ok value.");
            Assert.Equal("hello", result.Either.Error);
            Assert.Equal(default, result.Either.Value);
            Assert.Equal("Error ==> Result<Int32, String>(\"hello\")", result.ToString());
        }

        [Fact]
        public void Ok_Result_With_Char__Expects_Char_To_have__Single_Quotes() {
            var result = 'I'.ToMaybe().ToResult(x => 2);

            Assert.False(result.Either.HasError, "Result should not have a error value.");
            Assert.True(result.Either.HasValue, "Result should have a Ok value.");
            Assert.Equal('I', result.Either.Value);
            Assert.Equal(default, result.Either.Error);
            Assert.Equal("Ok ==> Result<Char, Int32>('I')", result.ToString());
        }

        [Fact]
        public void Ok_Result_With_String__Expects_String_To_have__Doble_Quotes() {
            var result = "hello".ToMaybe().ToResult(x => 2);

            Assert.False(result.Either.HasError, "Result should not have a error value.");
            Assert.True(result.Either.HasValue, "Result should have a Ok value.");
            Assert.Equal("hello", result.Either.Value);
            Assert.Equal(default, result.Either.Error);
            Assert.Equal("Ok ==> Result<String, Int32>(\"hello\")", result.ToString());
        }

        [Fact]
        public void Ok_Result_With_String_Using_Backslash__Expects_String_To_have__Backslash() {
            var result = "hello\\".ToMaybe().ToResult(x => 2);

            Assert.False(result.Either.HasError, "Result should not have a error value.");
            Assert.True(result.Either.HasValue, "Result should have a Ok value.");
            Assert.Equal("hello\\", result.Either.Value);
            Assert.Equal(default, result.Either.Error);
            Assert.Equal("Ok ==> Result<String, Int32>(\"hello\\\")", result.ToString());
        }

        [Fact]
        public void Ok_Result_With_String_Using_NewLines__Expects_String_To_have__Escaped_Values() {
            var result = "hello\r\nfoo".ToMaybe().ToResult(x => 2);

            Assert.False(result.Either.HasError, "Result should not have a error value.");
            Assert.True(result.Either.HasValue, "Result should have a Ok value.");
            Assert.Equal("hello\r\nfoo", result.Either.Value);
            Assert.Equal(default, result.Either.Error);
            Assert.Equal("Ok ==> Result<String, Int32>(\"hello\r\nfoo\")", result.ToString());
        }

        [Fact]
        public void Ok_Result_With_String_Using_Tab__Expects_String_To_have__Escaped_Values() {
            var result = "hello\tfoo".ToMaybe().ToResult(x => 2);

            Assert.False(result.Either.HasError, "Result should not have a error value.");
            Assert.True(result.Either.HasValue, "Result should have a Ok value.");
            Assert.Equal("hello\tfoo", result.Either.Value);
            Assert.Equal(default, result.Either.Error);
            Assert.Equal("Ok ==> Result<String, Int32>(\"hello\tfoo\")", result.ToString());
        }
    }
}