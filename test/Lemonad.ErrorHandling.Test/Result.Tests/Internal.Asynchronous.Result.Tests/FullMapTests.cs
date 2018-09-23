using System.Threading.Tasks;
using Lemonad.ErrorHandling.Internal;
using Xunit;

namespace Lemonad.ErrorHandling.Test.Result.Tests.Internal.Asynchronous.Result.Tests {
    public class FullMapTests {
        [Fact]
        public async Task
            Result_With_Error_Expects__Selector_Never__To_Be_Executed_And_ErrorSelector_To_Be_Invoked() {
            var selectorExectued = false;
            var errorSelectorExectued = false;
            var task = TaskResultFunctions.FullMap(AssertionUtilities.DivisionAsync(10, 0), d => {
                selectorExectued = true;
                return d * 2;
            }, s => {
                errorSelectorExectued = true;
                return s.ToUpper();
            });
            var result = await task;

            Assert.False(selectorExectued, "Should not get exectued since there's an error from the result.");
            Assert.Equal(default, result.Either.Value);
            Assert.Equal("CAN NOT DIVIDE '10' WITH '0'.", result.Either.Error);
            Assert.True(result.Either.HasError, "Result should have error.");
            Assert.False(result.Either.HasValue, "Result should not have value.");
        }

        [Fact]
        public async Task
            Result_With_Value_Expects__Selector_To_Be_Executed_And_ErrorSelector_To_Never_Be_Invoked() {
            var selectorExectued = false;
            var errorSelectorExectued = false;
            var task = TaskResultFunctions.FullMap(AssertionUtilities.DivisionAsync(10, 2), d => {
                selectorExectued = true;
                return d * 10;
            }, s => {
                errorSelectorExectued = true;
                return s.ToUpper();
            });
            var result = await task;

            Assert.True(selectorExectued, "Should get exectued since there's an value from the result.");
            Assert.False(errorSelectorExectued, "Should not get exectued since there's an value from the result.");
            Assert.Equal(50, result.Either.Value);
            Assert.Equal(default, result.Either.Error);
            Assert.False(result.Either.HasError, "Result should not have error.");
            Assert.True(result.Either.HasValue, "Result should have value.");
        }
    }
}