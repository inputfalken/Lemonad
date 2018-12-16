using Xunit;

namespace Lemonad.ErrorHandling.Unit.Result.Tests {
    public class DoWithErrorTests {
        [Fact]
        public void
            Result_With_Error__Expects_Action_To_Be_Invoked() {
            var actionExectued = false;
            var result = AssertionUtilities.Division(10, 0).DoWithError(d => actionExectued = true);

            Assert.True(actionExectued, "Should not get exectued since there's an error.");
            Assert.Equal(default, result.Either.Value);
            Assert.Equal("Can not divide '10' with '0'.", result.Either.Error);
            Assert.True(result.Either.HasError, "Result should have error.");
            Assert.False(result.Either.HasValue, "Result should not have value.");
        }

        [Fact]
        public void
            Result_With_Value__Expects_Action_Not_To_Be_Invoked() {
            var actionExectued = false;
            var result = AssertionUtilities.Division(10, 2).DoWithError(d => actionExectued = true);

            Assert.False(actionExectued, "Should not get exectued since there's an error.");
            Assert.Equal(5, result.Either.Value);
            Assert.Equal(default, result.Either.Error);
            Assert.False(result.Either.HasError, "Result should not have error.");
            Assert.True(result.Either.HasValue, "Result should have value.");
        }
    }
}