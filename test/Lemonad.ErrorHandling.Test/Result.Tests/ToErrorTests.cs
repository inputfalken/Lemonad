using System.Threading.Tasks;
using Xunit;

namespace Lemonad.ErrorHandling.Test.Result.Tests {
    public class ToResultErrorTests {
        [Fact]
        public void Convert_Int_To_ResultError() {
            var result = 2.ToResultError(i => true, () => "");

            Assert.False(result.HasValue, "Result should have error.");
            Assert.True(result.HasError, "Result should have a error value.");
            Assert.Equal(2, result.Error);
            Assert.Equal(default, result.Value);
        }

        [Fact]
        public void Convert_Null_String_To_ResultError() {
            string str = null;
            var result = str.ToResultError(x => true, () => "");

            Assert.False(result.HasValue, "Result should have error.");
            Assert.True(result.HasError, "Result should have a error value.");
            Assert.Null(result.Error);
            Assert.Equal(default, result.Value);
        }

        [Fact]
        public void Convert_String_To_ResultError() {
            var result = "hello".ToResultError(s => true, () => "");

            Assert.False(result.HasValue, "Result should have value.");
            Assert.True(result.HasError, "Result should have a error value.");
            Assert.Equal("hello", result.Error);
            Assert.Equal(default, result.Value);
        }
    }
}