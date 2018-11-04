using System;
using Xunit;

namespace Lemonad.ErrorHandling.Test.Result.Tests {
    public class CastTests {
        [Fact]
        public void Result_With_Value_Using_Valid_Cast_Expects_Value_As_Int() {
            const int identity = 0;
            var result = AssertionUtilities
                .GetGender(identity)
                .Cast<int>();

            Assert.True(result.Either.HasValue);
            Assert.False(result.Either.HasError);
            Assert.Equal(default, result.Either.Error);
            Assert.Equal(identity, result.Either.Value);
        }

        [Fact]
        public void Result_With_Error_Using_Valid_Cast_Expects_Error() {
            const int identity = 3;
            var result = AssertionUtilities
                .GetGender(identity)
                .Cast<int>();

            Assert.False(result.Either.HasValue);
            Assert.True(result.Either.HasError);
            Assert.Equal("Could not determine gender.", result.Either.Error);
            Assert.Equal(default, result.Either.Value);
        }

        [Fact]
        public void Result_With_Error_Using_Invalid_Cast_Expects_No_Exception() {
            const int identity = 3;
            var result = AssertionUtilities
                .GetGender(identity)
                .Cast<string>();

            Assert.False(result.Either.HasValue);
            Assert.True(result.Either.HasError);
            Assert.Equal("Could not determine gender.", result.Either.Error);
            Assert.Equal(default, result.Either.Value);
        }

        [Fact]
        public void Result_With_Value_Using_Invalid_Cast_Expects_Cast_Exception() {
            const int identity = 0;
            Assert.Throws<InvalidCastException>(() => AssertionUtilities.GetGender(identity).Cast<string>());
        }
    }
}