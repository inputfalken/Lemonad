using Lemonad.ErrorHandling.Extensions.Result;
using Xunit;

namespace Lemonad.ErrorHandling.Test.Result.Tests {
    public class ToMaybeTests {
        [Fact]
        public void Result_With_Error__Expects_Maybe_With_No_value() {
            var result = ErrorHandling.Result.Error<string, int>(2).ToMaybe();
            Assert.False(result.HasValue);
            Assert.Equal(default, result.Value);
        }

        [Fact]
        public void Result_With_Value__Expects_Maybe_With_Value() {
            var result = ErrorHandling.Result.Value<string, int>("Foo").ToMaybe();
            Assert.True(result.HasValue);
            Assert.Equal("Foo", result.Value);
        }
    }
}