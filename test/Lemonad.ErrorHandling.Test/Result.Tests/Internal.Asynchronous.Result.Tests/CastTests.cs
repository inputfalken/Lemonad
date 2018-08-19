using System;
using System.Threading.Tasks;
using Lemonad.ErrorHandling.Extensions.Internal;
using Xunit;

namespace Lemonad.ErrorHandling.Test.Result.Tests.Internal.Asynchronous.Result.Tests {
    public class CastTests {
        [Fact]
        public async Task Result_With_Error__With_Invalid_Casting() {
            var genderResult = AssertionUtilities.GetGender(3);
            var exception = await Record.ExceptionAsync(async () => {
                var result = TaskResultFunctions.Cast<AssertionUtilities.Gender, int, string>(genderResult);
                var castResult = await result;
                Assert.False(castResult.HasValue, "Casted Result not should have value.");
                Assert.True(castResult.HasError, "Casted Result should have error.");
                Assert.Equal(default, castResult.Value);
                Assert.Equal("Could not determine gender", castResult.Error);
            });

            Assert.Null(exception);
        }

        [Fact]
        public async Task Result_With_Error__With_Valid_Casting() {
            var genderResult = AssertionUtilities.GetGender(3);

            var castResult = await TaskResultFunctions.Cast<AssertionUtilities.Gender, int, string>(genderResult);

            Assert.False(castResult.HasValue, "Casted Result not should have value.");
            Assert.True(castResult.HasError, "Casted Result should have error.");
            Assert.Equal(default, castResult.Value);
            Assert.Equal("Could not determine gender", castResult.Error);
        }

        [Fact]
        public Task Result_With_Value__With_Invalid_Casting() =>
            Assert.ThrowsAsync<InvalidCastException>(() => TaskResultFunctions.Cast<AssertionUtilities.Gender, string, string>(AssertionUtilities.GetGender(1)));

        [Fact]
        public async Task Result_With_Value__With_Valid_Casting() {
            var genderResult = AssertionUtilities.GetGender(1);

            var castResult = await TaskResultFunctions.Cast<AssertionUtilities.Gender, int, string>(genderResult);

            Assert.True(castResult.HasValue, "Casted Result should have value.");
            Assert.False(castResult.HasError, "Casted Result should not have error.");
            Assert.Equal(1, castResult.Value);
            Assert.Equal(default, castResult.Error);
        }
    }
}
