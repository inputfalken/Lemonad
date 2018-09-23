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
            Assert.False(genderResult.Either.HasValue, "Result should not have value");
            Assert.True(genderResult.Either.HasError, "Result should have error");
            Assert.Equal(default, genderResult.Either.Value);
            Assert.Equal("Could not determine gender", genderResult.Either.Error);

            var exception = Record.Exception(() => {
                var castResult = genderResult.Cast<string>();
                Assert.False(castResult.Either.HasValue, "Casted Result not should have value.");
                Assert.True(castResult.Either.HasError, "Casted Result should have error.");
                Assert.Equal(default, castResult.Either.Value);
                Assert.Equal("Could not determine gender", castResult.Either.Error);
            });

            Assert.Null(exception);
        }

        [Fact]
        public void Result_With_Error__With_Valid_Casting() {
            var genderResult = GetGender(3);
            Assert.False(genderResult.Either.HasValue, "Result should not have value");
            Assert.True(genderResult.Either.HasError, "Result should have error");
            Assert.Equal(default, genderResult.Either.Value);
            Assert.Equal("Could not determine gender", genderResult.Either.Error);

            var castResult = genderResult.Cast<int>();

            Assert.False(castResult.Either.HasValue, "Casted Result not should have value.");
            Assert.True(castResult.Either.HasError, "Casted Result should have error.");
            Assert.Equal(default, castResult.Either.Value);
            Assert.Equal("Could not determine gender", castResult.Either.Error);
        }

        [Fact]
        public void Result_With_Value__With_Invalid_Casting() {
            var genderResult = GetGender(1);
            Assert.True(genderResult.Either.HasValue, "Result should have value");
            Assert.False(genderResult.Either.HasError, "Result should not have error");
            Assert.Equal(Gender.Female, genderResult.Either.Value);
            Assert.Equal(default, genderResult.Either.Error);

            Assert.Throws<InvalidCastException>(() => genderResult.Cast<string>());
        }

        [Fact]
        public void Result_With_Value__With_Valid_Casting() {
            var genderResult = GetGender(1);
            Assert.True(genderResult.Either.HasValue, "Result should have value");
            Assert.False(genderResult.Either.HasError, "Result should not have error");
            Assert.Equal(Gender.Female, genderResult.Either.Value);
            Assert.Equal(default, genderResult.Either.Error);

            var castResult = genderResult.Cast<int>();

            Assert.True(castResult.Either.HasValue, "Casted Result should have value.");
            Assert.False(castResult.Either.HasError, "Casted Result should not have error.");
            Assert.Equal(1, castResult.Either.Value);
            Assert.Equal(default, castResult.Either.Error);
        }
    }
}