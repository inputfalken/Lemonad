using System;
using System.Threading.Tasks;
using Lemonad.ErrorHandling.Internal;
using Xunit;

namespace Lemonad.ErrorHandling.Test.Result.Tests.Internal.Asynchronous.Result.Tests {
    public class CastTests {
        [Fact]
        public async Task Result_With_Error__With_Invalid_Casting() {
            var genderResult = AssertionUtilities.GetGender(3);
            var exception = await Record.ExceptionAsync(async () => {
                var result = TaskResultFunctions.Cast<AssertionUtilities.Gender, int, string>(genderResult);
                var castResult = await result;
                Assert.False(castResult.Either.HasValue, "Casted Result not should have value.");
                Assert.True(castResult.Either.HasError, "Casted Result should have error.");
                Assert.Equal(default, castResult.Either.Value);
                Assert.Equal("Could not determine gender", castResult.Either.Error);
            });

            Assert.Null(exception);
        }

        [Fact]
        public async Task Result_With_Error__With_Valid_Casting() {
            var genderResult = AssertionUtilities.GetGender(3);

            var castResult = await TaskResultFunctions.Cast<AssertionUtilities.Gender, int, string>(genderResult);

            Assert.False(castResult.Either.HasValue, "Casted Result not should have value.");
            Assert.True(castResult.Either.HasError, "Casted Result should have error.");
            Assert.Equal(default, castResult.Either.Value);
            Assert.Equal("Could not determine gender", castResult.Either.Error);
        }

        [Fact]
        public Task Result_With_Value__With_Invalid_Casting() =>
            Assert.ThrowsAsync<InvalidCastException>(() =>
                TaskResultFunctions.Cast<AssertionUtilities.Gender, string, string>(AssertionUtilities.GetGender(1)));

        [Fact]
        public async Task Result_With_Value__With_Valid_Casting() {
            var genderResult = AssertionUtilities.GetGender(1);

            var castResult = await TaskResultFunctions.Cast<AssertionUtilities.Gender, int, string>(genderResult);

            Assert.True(castResult.Either.HasValue, "Casted Result should have value.");
            Assert.False(castResult.Either.HasError, "Casted Result should not have error.");
            Assert.Equal(1, castResult.Either.Value);
            Assert.Equal(default, castResult.Either.Error);
        }
    }
}