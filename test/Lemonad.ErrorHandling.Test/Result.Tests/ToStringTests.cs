using Xunit;

namespace Lemonad.ErrorHandling.Test.Result.Tests {
    public class ToStringTests {
        [Fact]
        public void Error_Result_With_Char__Expects_Char_To_have__Single_Quotes() {
            var either = 2.None().ToResult(() => 'I');

            Assert.True(either.HasError, "Result should have a error value.");
            Assert.False(either.HasValue, "Result should not have a Ok value.");
            Assert.Equal(default(int), either.Value);
            Assert.Equal('I', either.Error);
            Assert.Equal("Error ==> Result<Int32, Char>('I')", either.ToString());
        }

        [Fact]
        public void Error_Result_With_Int() {
            string hello = null;
            var either = hello.None().ToResult(() => 2);

            Assert.True(either.HasError, "Result should have a error value.");
            Assert.False(either.HasValue, "Result should not have a Ok value.");
            Assert.Equal(2, either.Error);
            Assert.Equal(default(string), either.Value);
            Assert.Equal("Error ==> Result<String, Int32>(2)", either.ToString());
        }

        [Fact]
        public void Error_Result_With_String__Expects_String_To_have__Doble_Quotes() {
            var either = 2.None().ToResult(() => "hello");

            Assert.True(either.HasError, "Result should have a error value.");
            Assert.False(either.HasValue, "Result should not have a Ok value.");
            Assert.Equal("hello", either.Error);
            Assert.Equal(default(int), either.Value);
            Assert.Equal("Error ==> Result<Int32, String>(\"hello\")", either.ToString());
        }

        [Fact]
        public void Ok_Result_With_Char__Expects_Char_To_have__Single_Quotes() {
            var either = 'I'.Some().ToResult(() => 2);

            Assert.False(either.HasError, "Result should not have a error value.");
            Assert.True(either.HasValue, "Result should have a Ok value.");
            Assert.Equal('I', either.Value);
            Assert.Equal(default(int), either.Error);
            Assert.Equal("Ok ==> Result<Char, Int32>('I')", either.ToString());
        }

        [Fact]
        public void Ok_Result_With_String__Expects_String_To_have__Doble_Quotes() {
            var either = "hello".Some().ToResult(() => 2);

            Assert.False(either.HasError, "Result should not have a error value.");
            Assert.True(either.HasValue, "Result should have a Ok value.");
            Assert.Equal("hello", either.Value);
            Assert.Equal(default(int), either.Error);
            Assert.Equal("Ok ==> Result<String, Int32>(\"hello\")", either.ToString());
        }

        [Fact]
        public void Ok_Result_With_String_Using_Backslash__Expects_String_To_have__Backslash() {
            var either = "hello\\".Some().ToResult(() => 2);

            Assert.False(either.HasError, "Result should not have a error value.");
            Assert.True(either.HasValue, "Result should have a Ok value.");
            Assert.Equal("hello\\", either.Value);
            Assert.Equal(default(int), either.Error);
            Assert.Equal("Ok ==> Result<String, Int32>(\"hello\\\")", either.ToString());
        }

        [Fact]
        public void Ok_Result_With_String_Using_NewLines__Expects_String_To_have__Escaped_Values() {
            var either = "hello\r\nfoo".Some().ToResult(() => 2);

            Assert.False(either.HasError, "Result should not have a error value.");
            Assert.True(either.HasValue, "Result should have a Ok value.");
            Assert.Equal("hello\r\nfoo", either.Value);
            Assert.Equal(default(int), either.Error);
            Assert.Equal("Ok ==> Result<String, Int32>(\"hello\r\nfoo\")", either.ToString());
        }

        [Fact]
        public void Ok_Result_With_String_Using_Tab__Expects_String_To_have__Escaped_Values() {
            var either = "hello\tfoo".Some().ToResult(() => 2);

            Assert.False(either.HasError, "Result should not have a error value.");
            Assert.True(either.HasValue, "Result should have a Ok value.");
            Assert.Equal("hello\tfoo", either.Value);
            Assert.Equal(default(int), either.Error);
            Assert.Equal("Ok ==> Result<String, Int32>(\"hello\tfoo\")", either.ToString());
        }
    }
}
