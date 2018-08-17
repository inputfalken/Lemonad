using System;
using System.Threading.Tasks;
using Lemonad.ErrorHandling.Extensions.Internal;
using Xunit;
using static Lemonad.ErrorHandling.Test.AssertionUtilities;

namespace Lemonad.ErrorHandling.Test.Asynchronous.Result.Tests {
    public class CastErrorTests {
        [Fact]
        public Task Result_With_Error__With_Invalid_Casting() {
            return Assert.ThrowsAsync<InvalidCastException>(() => TaskResultFunctions.CastError<string, ExitCodes, string>(Program(1)));
        }

        [Fact]
        public async Task Result_With_Error__With_Valid_Casting() {
            var programResult = Program(1);
            var castResult = await TaskResultFunctions.CastError<string, ExitCodes, int>(programResult);
            Assert.False(castResult.HasValue, "Casted Result not should have value.");
            Assert.True(castResult.HasError, "Casted Result should have error.");
            Assert.Equal(default, castResult.Value);
            Assert.Equal(1, castResult.Error);
        }

        [Fact]
        public async Task Result_With_Value__With_Invalid_Casting() {
            var programResult = Program(0);

            var exception = await Record.ExceptionAsync(async () => {
                var castResult = await TaskResultFunctions.CastError<string, ExitCodes, string>(programResult);
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

            var castResult = await TaskResultFunctions.CastError<string, ExitCodes, int>(programResult);

            Assert.True(castResult.HasValue, "Casted Result should have value.");
            Assert.False(castResult.HasError, "Casted Result should not have error.");
            Assert.Equal(default, castResult.Error);
            Assert.Equal("Success", castResult.Value);
        }
    }
}
