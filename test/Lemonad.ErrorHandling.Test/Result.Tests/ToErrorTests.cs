using Xunit;

namespace Lemonad.ErrorHandling.Test.Result.Tests {
    public class ToResulterrorTests {
        [Fact]
        public void Convert_Int_To_Resulterror() {
            var either = 2.ToResultError<string, int>();

            Assert.False(either.HasValue, "Result should not have a right value.");
            Assert.True(either.HasError, "Result should have a error value.");
            Assert.Equal(2, either.Error);
            Assert.Equal(default(string), either.Value);
        }

        [Fact]
        public void Convert_Null_String_To_Resulterror() {
            string str = null;
            var either = str.ToResultError<int, string>();

            Assert.False(either.HasValue, "Result should not have a right value.");
            Assert.True(either.HasError, "Result should have a error value.");
            Assert.Null(either.Error);
            Assert.Equal(default(int), either.Value);
        }

        [Fact]
        public void Convert_String_To_Resulterror() {
            var either = "hello".ToResultError<int, string>();

            Assert.False(either.HasValue, "Result should have a right value.");
            Assert.True(either.HasError, "Result should have a error value.");
            Assert.Equal("hello", either.Error);
            Assert.Equal(default(int), either.Value);
        }
    }
}
