using System;
using System.Threading.Tasks;
using Lemonad.ErrorHandling.Extensions;
using Xunit;

namespace Lemonad.ErrorHandling.Test.Asynchronous.Result.Tests {
    public class CastErrorTests {
        private enum ExitCodes {
            Fail = 1,
            Unhandled
        }

        private static async Task<Result<string, ExitCodes>> Program(int code) {
            await Task.Delay(50);

            switch (code) {
                case 0:
                    return "Success";
                case 1:
                    return ExitCodes.Fail;
                default:
                    return ExitCodes.Unhandled;
            }
        }

        [Fact]
        public Task Result_With_Error__With_Invalid_Casting() {
            return Assert.ThrowsAsync<InvalidCastException>(() => Program(1).CastError<string, string, ExitCodes>());
        }

        [Fact]
        public async Task Result_With_Error__With_Valid_Casting() {
            var programResult = Program(1);
            var castResult = await programResult.CastError<string, int, ExitCodes>();
            Assert.False(castResult.HasValue, "Casted Result not should have value.");
            Assert.True(castResult.HasError, "Casted Result should have error.");
            Assert.Equal(default, castResult.Value);
            Assert.Equal(1, castResult.Error);
        }

        [Fact]
        public async Task Result_With_Value__With_Invalid_Casting() {
            var programResult = Program(0);

            var exception = await Record.ExceptionAsync(async () => {
                var castResult = await programResult.CastError<string, string, ExitCodes>();
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

            var castResult = await programResult.CastError<string, int, ExitCodes>();

            Assert.True(castResult.HasValue, "Casted Result should have value.");
            Assert.False(castResult.HasError, "Casted Result should not have error.");
            Assert.Equal(default, castResult.Error);
            Assert.Equal("Success", castResult.Value);
        }
    }
}