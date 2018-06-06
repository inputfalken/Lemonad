using Xunit;

namespace Lemonad.ErrorHandling.Test.Either.Tests {
    public class ToEitherRightTests {
        [Fact]
        public void Convert_String_To_EitherRight() {
            var either = "hello".ToEitherRight<int, string>();

            Assert.True(either.IsRight, "Either should have a right value.");
            Assert.False(either.IsLeft, "Either should not have a left value.");
            Assert.Equal("hello", either.Right);
            Assert.Equal(default(int), either.Left);
        }

        [Fact]
        public void Convert_Null_String_To_EitherRight() {
            string str = null;
            var either = str.ToEitherRight<int, string>();

            Assert.True(either.IsRight, "Either should have a right value.");
            Assert.False(either.IsLeft, "Either should not have a left value.");
            Assert.Null(either.Right);
            Assert.Equal(default(int), either.Left);
        }

        [Fact]
        public void Convert_Int_To_EitherRight() {
            var either = 2.ToEitherRight<string, int>();

            Assert.True(either.IsRight, "Either should have a right value.");
            Assert.False(either.IsLeft, "Either should not have a left value.");
            Assert.Equal(2, either.Right);
            Assert.Equal(default(string), either.Left);
        }
    }
}