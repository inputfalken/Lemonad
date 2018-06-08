using Xunit;

namespace Lemonad.ErrorHandling.Test.Either.Tests {
    public class ToEitherLeftTests {
        [Fact]
        public void Convert_Int_To_EitherLeft() {
            var either = 2.ToEitherLeft<int, string>();

            Assert.False(either.IsRight, "Either should not have a right value.");
            Assert.True(either.IsLeft, "Either should have a left value.");
            Assert.Equal(2, either.Left);
            Assert.Equal(default(string), either.Right);
        }

        [Fact]
        public void Convert_Null_String_To_EitherLeft() {
            string str = null;
            var either = str.ToEitherLeft<string, int>();

            Assert.False(either.IsRight, "Either should not have a right value.");
            Assert.True(either.IsLeft, "Either should have a left value.");
            Assert.Null(either.Left);
            Assert.Equal(default(int), either.Right);
        }

        [Fact]
        public void Convert_String_To_EitherLeft() {
            var either = "hello".ToEitherLeft<string, int>();

            Assert.False(either.IsRight, "Either should have a right value.");
            Assert.True(either.IsLeft, "Either should have a left value.");
            Assert.Equal("hello", either.Left);
            Assert.Equal(default(int), either.Right);
        }
    }
}