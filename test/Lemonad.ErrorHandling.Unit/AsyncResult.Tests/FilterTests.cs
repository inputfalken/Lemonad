using System.Threading.Tasks;
using Assertion;
using Lemonad.ErrorHandling.Extensions.AsyncResult;
using Xunit;

namespace Lemonad.ErrorHandling.Unit.AsyncResult.Tests {
    public class OkWhenTests {
        [Fact]
        public async Task
            Result_With_Error__Expects_Predicate_Never_To_Be_Executed_And_ErrorSelector_Never_To_Be_Invoked_With_Parameter_ErrorSelector() {
            var predicateExectued = false;
            var errorSelectorExectued = false;
            var result = await AssertionUtilities.DivisionAsync(10, 0).Filter(d => {
                predicateExectued = true;
                return d == 2;
            }, x => {
                errorSelectorExectued = true;
                return "This should never happen!";
            });

            Assert.False(predicateExectued,
                "Should not get exectued since there's an error before the predicate was applied.");
            Assert.False(errorSelectorExectued,
                "Should not get exectued since there's an error before the predicate was applied.");
            Assert.Equal(default, result.Either.Value);
            Assert.Equal("Can not divide '10' with '0'.", result.Either.Error);
            Assert.True(result.Either.HasError, "Result should have error.");
            Assert.False(result.Either.HasValue, "Result should not have value.");
        }

        [Fact]
        public async Task
            Result_With_Value_With_Falsy_Predicate__Expects_Predicate_To_Be_Executed_And_ErrorSelector_To_Be_Invoked_With_Parameter_ErrorSelector() {
            var predicateExectued = false;
            var errorSelectorExectued = false;
            var result = await AssertionUtilities.DivisionAsync(10, 2).Filter(d => {
                predicateExectued = true;
                return false;
            }, x => {
                // Since the division operation was ok, but the predicate false, it makes sense that this value is positive.
                errorSelectorExectued = true;
                return "Good";
            });

            Assert.True(predicateExectued, "Should get exectued since there's a value from the result.");
            Assert.True(errorSelectorExectued, "Should get exectued since the predicate was truthy.");
            Assert.Equal(default, result.Either.Value);
            Assert.Equal("Good", result.Either.Error);
            Assert.True(result.Either.HasError, "Result should have error.");
            Assert.False(result.Either.HasValue, "Result should not have value.");
        }

        [Fact]
        public async Task
            Result_With_Value_With_Truthy_Predicate__Expects_Predicate_To_Be_Executed_And_ErrorSelector_To_Never_Be_Invoked_With_Parameter_ErrorSelector() {
            var predicateExectued = false;
            var errorSelectorExectued = false;
            var result = await AssertionUtilities.DivisionAsync(10, 2).Filter(d => {
                predicateExectued = true;
                return true;
            }, x => {
                errorSelectorExectued = true;
                return "This should never happen.";
            });

            Assert.True(predicateExectued,
                "Should get exectued since there's a value from the result.");
            Assert.False(errorSelectorExectued,
                "Should not get exectued since the predicate was falsy.");
            Assert.Equal(5, result.Either.Value);
            Assert.Equal(default, result.Either.Error);
            Assert.False(result.Either.HasError, "Result should not have error.");
            Assert.True(result.Either.HasValue, "Result should have value.");
        }
    }
}