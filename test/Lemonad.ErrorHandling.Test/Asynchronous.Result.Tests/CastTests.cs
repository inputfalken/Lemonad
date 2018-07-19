using System;
using System.Threading.Tasks;
using Lemonad.ErrorHandling.Extensions;
using static Lemonad.ErrorHandling.Test.AssertionUtilities;
using Xunit;

namespace Lemonad.ErrorHandling.Test.Asynchronous.Result.Tests {
    public class CastTests {
        [Fact]
        public async Task Result_With_Error__With_Invalid_Casting() {
            var genderResult = GetGender(3);
            var exception = await Record.ExceptionAsync(async () => {
                var result = genderResult.Cast<Gender, int, string>();
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
            var genderResult = GetGender(3);

            var castResult = await genderResult.Cast<Gender, int, string>();

            Assert.False(castResult.HasValue, "Casted Result not should have value.");
            Assert.True(castResult.HasError, "Casted Result should have error.");
            Assert.Equal(default, castResult.Value);
            Assert.Equal("Could not determine gender", castResult.Error);
        }

        [Fact]
        public Task Result_With_Value__With_Invalid_Casting() =>
            Assert.ThrowsAsync<InvalidCastException>(() => GetGender(1).Cast<Gender, string, string>());

        [Fact]
        public async Task Result_With_Value__With_Valid_Casting() {
            var genderResult = GetGender(1);

            var castResult = await genderResult.Cast<Gender, int, string>();

            Assert.True(castResult.HasValue, "Casted Result should have value.");
            Assert.False(castResult.HasError, "Casted Result should not have error.");
            Assert.Equal(1, castResult.Value);
            Assert.Equal(default, castResult.Error);
        }
    }
}