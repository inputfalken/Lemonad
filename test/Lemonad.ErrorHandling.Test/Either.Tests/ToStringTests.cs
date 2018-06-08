using Xunit;

namespace Lemonad.ErrorHandling.Test.Either.Tests {
    public class ToStringTests {
        [Fact]
        public void Right_Either_With_String__Expects_String_To_have__Doble_Quotes() {
            var either = "hello".Some().ToEither(() => 2);

            Assert.False(either.IsLeft, "Either should not have a left value.");
            Assert.True(either.IsRight, "Either should have a Right value.");
            Assert.Equal(either.Right, "hello");
            Assert.Equal(default(int), either.Left);
            Assert.Equal("Right ==> Either<Int32, String>(\"hello\")", either.ToString());
        }
        
        [Fact]
        public void Right_Either_With_String_Using_NewLines__Expects_String_To_have__Escaped_Values() {
            var either = "hello\r\nfoo".Some().ToEither(() => 2);

            Assert.False(either.IsLeft, "Either should not have a left value.");
            Assert.True(either.IsRight, "Either should have a Right value.");
            Assert.Equal(either.Right, "hello\r\nfoo");
            Assert.Equal(default(int), either.Left);
            Assert.Equal("Right ==> Either<Int32, String>(\"hello\r\nfoo\")", either.ToString());
        }
        
        [Fact]
        public void Right_Either_With_String_Using_Tab__Expects_String_To_have__Escaped_Values() {
            var either = "hello\tfoo".Some().ToEither(() => 2);

            Assert.False(either.IsLeft, "Either should not have a left value.");
            Assert.True(either.IsRight, "Either should have a Right value.");
            Assert.Equal(either.Right, "hello\tfoo");
            Assert.Equal(default(int), either.Left);
            Assert.Equal("Right ==> Either<Int32, String>(\"hello\tfoo\")", either.ToString());
        }
        
        [Fact]
        public void Right_Either_With_String_Using_Backslash__Expects_String_To_have__Backslash() {
            var either = "hello\\".Some().ToEither(() => 2);

            Assert.False(either.IsLeft, "Either should not have a left value.");
            Assert.True(either.IsRight, "Either should have a Right value.");
            Assert.Equal(either.Right, "hello\\");
            Assert.Equal(default(int), either.Left);
            Assert.Equal("Right ==> Either<Int32, String>(\"hello\\\")", either.ToString());
        }

        [Fact]
        public void Right_Either_With_Char__Expects_Char_To_have__Single_Quotes() {
            var either = 'I'.Some().ToEither(() => 2);

            Assert.False(either.IsLeft, "Either should not have a left value.");
            Assert.True(either.IsRight, "Either should have a Right value.");
            Assert.Equal('I', either.Right);
            Assert.Equal(default(int), either.Left);
            Assert.Equal("Right ==> Either<Int32, Char>('I')", either.ToString());
        }

        [Fact]
        public void Left_Either_With_String__Expects_String_To_have__Doble_Quotes() {
            var either = 2.None().ToEither(() => "hello");

            Assert.True(either.IsLeft, "Either should have a left value.");
            Assert.False(either.IsRight, "Either should not have a Right value.");
            Assert.Equal(either.Left, "hello");
            Assert.Equal(default(int), either.Right);
            Assert.Equal("Left ==> Either<String, Int32>(\"hello\")", either.ToString());
        }

        [Fact]
        public void Left_Either_With_Char__Expects_Char_To_have__Single_Quotes() {
            var either = 2.None().ToEither(() => 'I');

            Assert.True(either.IsLeft, "Either should have a left value.");
            Assert.False(either.IsRight, "Either should not have a Right value.");
            Assert.Equal(default(int), either.Right);
            Assert.Equal('I', either.Left);
            Assert.Equal("Left ==> Either<Char, Int32>('I')", either.ToString());
        }

        [Fact]
        public void Left_Either_With_Int() {
            string hello = null;
            var either = hello.None().ToEither(() => 2);

            Assert.True(either.IsLeft, "Either should have a left value.");
            Assert.False(either.IsRight, "Either should not have a Right value.");
            Assert.Equal(2, either.Left);
            Assert.Equal(default(string), either.Right);
            Assert.Equal("Left ==> Either<Int32, String>(2)", either.ToString());
        }
    }
}