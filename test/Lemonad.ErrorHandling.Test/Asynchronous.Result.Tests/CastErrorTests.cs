using System;
using System.Threading.Tasks;
using Lemonad.ErrorHandling.Extensions;
using Xunit;

namespace Lemonad.ErrorHandling.Test.Asynchronous.Result.Tests {
    public class CastErrorTests {
        private enum Gender {
            Male = 0,
            Female = 1
        }

        private static async Task<Result<Gender, string>> GetGender(int identity) {
            await Task.Delay(50);
            switch (identity) {
                case 0:
                    return Gender.Male;
                case 1:
                    return Gender.Female;
                default:
                    return "Could not determine gender";
            }
        }

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