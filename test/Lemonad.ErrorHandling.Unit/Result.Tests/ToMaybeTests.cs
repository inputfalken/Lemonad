using Lemonad.ErrorHandling.Extensions.Result;
using Xunit;

namespace Lemonad.ErrorHandling.Unit.Result.Tests {
    public class ToMaybeTests {
        [Fact]
        public void Result_With_Error__Expects_Maybe_With_No_value() {
            var maybe = ErrorHandling.Result.Error<string, int>(2).ToMaybe();
            Assert.False(maybe.HasValue);
            Assert.Equal(default, maybe.Value);
        }

        [Fact]
        public void Result_With_Value__Expects_Maybe_With_Value() {
            var maybe = ErrorHandling.Result.Value<string, int>("Foo").ToMaybe();
            Assert.True(maybe.HasValue);
            Assert.Equal("Foo", maybe.Value);
        }
    }
}