using Xunit;

namespace Lemonad.ErrorHandling.Test.Result.Tests {
    public class ToResultOkTests {
        [Fact]
        public void Convert_Int_To_ResultOk() {
            var result = 2.ToResult(x => true, () => "");

            Assert.True(result.HasValue, "Result should have value.");
            Assert.False(result.HasError, "Result should not have a error value.");
            Assert.Equal(2, result.Value);
            Assert.Equal(default, result.Error);
        }

        [Fact]
        public void Convert_Null_String_To_ResultOk() {
            string str = null;
            var result = str.ToResult(x => true, () => "");

            Assert.True(result.HasValue, "Result should have value.");
            Assert.False(result.HasError, "Result should not have a error value.");
            Assert.Null(result.Value);
        }

        [Fact]
        public void Convert_String_To_ResultOk() {
            var result = "hello".ToResult(x => true, () => "");

            Assert.True(result.HasValue, "Result should have value.");
            Assert.False(result.HasError, "Result should not have a error value.");
            Assert.Equal("hello", result.Value);
            Assert.Equal(default, result.Error);
        }
    }
}