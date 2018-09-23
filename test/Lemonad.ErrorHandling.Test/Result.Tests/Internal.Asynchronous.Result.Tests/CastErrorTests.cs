using System;
using System.Threading.Tasks;
using Lemonad.ErrorHandling.Internal;
using Xunit;

namespace Lemonad.ErrorHandling.Test.Result.Tests.Internal.Asynchronous.Result.Tests {
    public class CastErrorTests {
        [Fact]
        public Task Result_With_Error__With_Invalid_Casting() {
            return Assert.ThrowsAsync<InvalidCastException>(() =>
                TaskResultFunctions.CastError<string, AssertionUtilities.ExitCodes, string>(
                    AssertionUtilities.Program(1)));
        }

        [Fact]
        public async Task Result_With_Error__With_Valid_Casting() {
            var programResult = AssertionUtilities.Program(1);
            var castResult =
                await TaskResultFunctions.CastError<string, AssertionUtilities.ExitCodes, int>(programResult);
            Assert.False(castResult.Either.HasValue, "Casted Result not should have value.");
            Assert.True(castResult.Either.HasError, "Casted Result should have error.");
            Assert.Equal(default, castResult.Either.Value);
            Assert.Equal(1, castResult.Either.Error);
        }

        [Fact]
        public async Task Result_With_Value__With_Invalid_Casting() {
            var programResult = AssertionUtilities.Program(0);

            var exception = await Record.ExceptionAsync(async () => {
                var castResult =
                    await TaskResultFunctions.CastError<string, AssertionUtilities.ExitCodes, string>(programResult);
                Assert.True(castResult.Either.HasValue, "Result should have value");
                Assert.False(castResult.Either.HasError, "Result should not have error");
                Assert.Equal("Success", castResult.Either.Value);
                Assert.Equal(default, castResult.Either.Error);
            });
            Assert.Null(exception);
        }

        [Fact]
        public async Task Result_With_Value__With_Valid_Casting() {
            var programResult = AssertionUtilities.Program(0);

            var castResult =
                await TaskResultFunctions.CastError<string, AssertionUtilities.ExitCodes, int>(programResult);

            Assert.True(castResult.Either.HasValue, "Casted Result should have value.");
            Assert.False(castResult.Either.HasError, "Casted Result should not have error.");
            Assert.Equal(default, castResult.Either.Error);
            Assert.Equal("Success", castResult.Either.Value);
        }
    }
}