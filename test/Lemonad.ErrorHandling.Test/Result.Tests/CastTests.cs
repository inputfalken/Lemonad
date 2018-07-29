using System;
using Xunit;

namespace Lemonad.ErrorHandling.Test.Result.Tests {
    public class CastTests {
        private enum Gender {
            Male = 0,
            Female = 1
        }

        private static Result<Gender, string> GetGender(int identity) {
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
        public void Result_With_Error__With_Invalid_Casting() {
            var genderResult = GetGender(3);
            Assert.False(genderResult.HasValue, "Result should not have value");
            Assert.True(genderResult.HasError, "Result should have error");
            Assert.Equal(default, genderResult.Value);
            Assert.Equal("Could not determine gender", genderResult.Error);

            var exception = Record.Exception(() => {
                var castResult = genderResult.Cast<string>();
                Assert.False(castResult.HasValue, "Casted Result not should have value.");
                Assert.True(castResult.HasError, "Casted Result should have error.");
                Assert.Equal(default, castResult.Value);
                Assert.Equal("Could not determine gender", castResult.Error);
            });

            Assert.Null(exception);
        }

        [Fact]
        public void Result_With_Error__With_Valid_Casting() {
            var genderResult = GetGender(3);
            Assert.False(genderResult.HasValue, "Result should not have value");
            Assert.True(genderResult.HasError, "Result should have error");
            Assert.Equal(default, genderResult.Value);
            Assert.Equal("Could not determine gender", genderResult.Error);

            var castResult = genderResult.Cast<int>();

            Assert.False(castResult.HasValue, "Casted Result not should have value.");
            Assert.True(castResult.HasError, "Casted Result should have error.");
            Assert.Equal(default, castResult.Value);
            Assert.Equal("Could not determine gender", castResult.Error);
        }

        [Fact]
        public void Result_With_Value__With_Invalid_Casting() {
            var genderResult = GetGender(1);
            Assert.True(genderResult.HasValue, "Result should have value");
            Assert.False(genderResult.HasError, "Result should not have error");
            Assert.Equal(Gender.Female, genderResult.Value);
            Assert.Equal(default, genderResult.Error);

            Assert.Throws<InvalidCastException>(() => genderResult.Cast<string>());
        }

        [Fact]
        public void Result_With_Value__With_Valid_Casting() {
            var genderResult = GetGender(1);
            Assert.True(genderResult.HasValue, "Result should have value");
            Assert.False(genderResult.HasError, "Result should not have error");
            Assert.Equal(Gender.Female, genderResult.Value);
            Assert.Equal(default, genderResult.Error);

            var castResult = genderResult.Cast<int>();

            Assert.True(castResult.HasValue, "Casted Result should have value.");
            Assert.False(castResult.HasError, "Casted Result should not have error.");
            Assert.Equal(1, castResult.Value);
            Assert.Equal(default, castResult.Error);
        }
    }
}