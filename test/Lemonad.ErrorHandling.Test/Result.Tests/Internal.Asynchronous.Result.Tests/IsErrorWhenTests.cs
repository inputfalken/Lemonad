using System.Threading.Tasks;
using Lemonad.ErrorHandling.Internal;
using Xunit;

namespace Lemonad.ErrorHandling.Test.Result.Tests.Internal.Asynchronous.Result.Tests {
    public class IsErrorWhenTests {
        [Fact]
        public async Task
            Result_With_Error__Expects_Predicate_Never_To_Be_Executed_And_ErrorSelector_Never_To_Be_Invoked() {
            var predicateExectued = false;
            var errorSelectorExectued = false;
            var isErrorWhen = TaskResultFunctions.IsErrorWhen(AssertionUtilities.DivisionAsync(10, 0), d => {
                predicateExectued = true;
                return d == 2;
            }, () => {
                errorSelectorExectued = true;
                return "Bad";
            });

            var result = await isErrorWhen;

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
            Result_With_Value_With_Falsy_Predicate__Expects_Predicate_To_Be_Executed_And_ErrorSelector_To_Never_Be_Invoked() {
            var predicateExectued = false;
            var errorSelectorExectued = false;
            var isErrorWhen = TaskResultFunctions.IsErrorWhen(AssertionUtilities.DivisionAsync(10, 2), d => {
                predicateExectued = true;
                return false;
            }, () => {
                errorSelectorExectued = true;
                return "Bad";
            });

            var result = await isErrorWhen;

            Assert.True(predicateExectued,
                "Should get exectued since there's a value from the result.");
            Assert.False(errorSelectorExectued,
                "Should not get exectued since the predicate was falsy.");
            Assert.Equal(5, result.Either.Value);
            Assert.Equal(default, result.Either.Error);
            Assert.False(result.Either.HasError, "Result should not have error.");
            Assert.True(result.Either.HasValue, "Result should have value.");
        }

        [Fact]
        public async Task
            Result_With_Value_With_Truthy_Predicate__Expects_Predicate_To_Be_Executed_And_ErrorSelector_To_Be_Invoked() {
            var predicateExectued = false;
            var errorSelectorExectued = false;
            var isErrorWhen = TaskResultFunctions.IsErrorWhen(AssertionUtilities.DivisionAsync(10, 2), d => {
                predicateExectued = true;
                return true;
            }, () => {
                errorSelectorExectued = true;
                return "Bad";
            });

            Assert.False(predicateExectued, "Should not get exectued before the value is awaited.");
            Assert.False(errorSelectorExectued, "Should not get exectued before the value is awaited.");

            var result = await isErrorWhen;

            Assert.True(predicateExectued, "Should get exectued since there's a value from the result.");
            Assert.True(errorSelectorExectued, "Should get exectued since the predicate was truthy.");
            Assert.Equal(default, result.Either.Value);
            Assert.Equal("Bad", result.Either.Error);
            Assert.True(result.Either.HasError, "Result should have error.");
            Assert.False(result.Either.HasValue, "Result should not have value.");
        }
    }
}