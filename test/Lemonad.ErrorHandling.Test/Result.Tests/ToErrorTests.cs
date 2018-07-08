using Lemonad.ErrorHandling.Extensions;
using Xunit;

namespace Lemonad.ErrorHandling.Test.Result.Tests {
    public class ToResulterrorTests {
        [Fact]
        public void Convert_Int_To_Resulterror() {
            var result = 2.ToResultError<string, int>();

            Assert.False(result.HasValue, "Result should have error.");
            Assert.True(result.HasError, "Result should have a error value.");
            Assert.Equal(2, result.Error);
            Assert.Equal(default(string), result.Value);
        }

        [Fact]
        public void Convert_Null_String_To_Resulterror() {
            string str = null;
            var result = str.ToResultError<int, string>();

            Assert.False(result.HasValue, "Result should have error.");
            Assert.True(result.HasError, "Result should have a error value.");
            Assert.Null(result.Error);
            Assert.Equal(default(int), result.Value);
        }

        [Fact]
        public void Convert_String_To_Resulterror() {
            var result = "hello".ToResultError<int, string>();

            Assert.False(result.HasValue, "Result should have value.");
            Assert.True(result.HasError, "Result should have a error value.");
            Assert.Equal("hello", result.Error);
            Assert.Equal(default(int), result.Value);
        }
    }
}
