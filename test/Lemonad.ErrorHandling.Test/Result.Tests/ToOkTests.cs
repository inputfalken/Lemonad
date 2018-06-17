using Xunit;

namespace Lemonad.ErrorHandling.Test.Result.Tests {
    public class ToResultOkTests {
        [Fact]
        public void Convert_Int_To_ResultOk() {
            var either = 2.ToResult<int, string>();

            Assert.True(either.HasValue, "Result should have a right value.");
            Assert.False(either.HasError, "Result should not have a error value.");
            Assert.Equal(2, either.Value);
            Assert.Equal(default(string), either.Error);
        }

        [Fact]
        public void Convert_Null_String_To_ResultOk() {
            string str = null;
            var either = str.ToResult<string, int>();

            Assert.True(either.HasValue, "Result should have a right value.");
            Assert.False(either.HasError, "Result should not have a error value.");
            Assert.Null(either.Value);
        }

        [Fact]
        public void Convert_String_To_ResultOk() {
            var either = "hello".ToResult<string, int>();

            Assert.True(either.HasValue, "Result should have a right value.");
            Assert.False(either.HasError, "Result should not have a error value.");
            Assert.Equal("hello", either.Value);
            Assert.Equal(default(int), either.Error);
        }
    }
}
