using System;
using System.Threading.Tasks;
using Lemonad.ErrorHandling.Extensions;
using static Lemonad.ErrorHandling.Test.Asynchronous.Result.Tests.AssertionUtilitiesAsync;
using Xunit;

namespace Lemonad.ErrorHandling.Test.Asynchronous.Result.Tests {
    public class CastErrorTests {
        [Fact]
        public Task Result_With_Error__With_Invalid_Casting() {
            return Assert.ThrowsAsync<InvalidCastException>(() => Program(1).CastError<string, ExitCodes, string>());
        }

        [Fact]
        public async Task Result_With_Error__With_Valid_Casting() {
            var programResult = Program(1);
            var castResult = await programResult.CastError<string, ExitCodes, int>();
            Assert.False(castResult.HasValue, "Casted Result not should have value.");
            Assert.True(castResult.HasError, "Casted Result should have error.");
            Assert.Equal(default, castResult.Value);
            Assert.Equal(1, castResult.Error);
        }

        [Fact]
        public async Task Result_With_Value__With_Invalid_Casting() {
            var programResult = Program(0);

            var exception = await Record.ExceptionAsync(async () => {
                var castResult = await programResult.CastError<string, ExitCodes, string>();
                Assert.True(castResult.HasValue, "Result should have value");
                Assert.False(castResult.HasError, "Result should not have error");
                Assert.Equal("Success", castResult.Value);
                Assert.Equal(default, castResult.Error);
            });
            Assert.Null(exception);
        }

        [Fact]
        public async Task Result_With_Value__With_Valid_Casting() {
            var programResult = Program(0);

            var castResult = await programResult.CastError<string, ExitCodes, int>();

            Assert.True(castResult.HasValue, "Casted Result should have value.");
            Assert.False(castResult.HasError, "Casted Result should not have error.");
            Assert.Equal(default, castResult.Error);
            Assert.Equal("Success", castResult.Value);
        }
    }
}