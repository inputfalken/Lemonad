using System.Threading.Tasks;
using Lemonad.ErrorHandling.Extensions.Internal;
using Xunit;

namespace Lemonad.ErrorHandling.Test.Result.Tests.Internal.Asynchronous.Result.Tests {
    public class MapTests {
        [Fact]
        public async Task Result_With_Error_Maps__Expects_Selector_Never_Be_Executed() {
            var selectorExectued = false;
            var division = await TaskResultFunctions.Map(AssertionUtilities.DivisionAsync(2, 0), x => {
                selectorExectued = true;
                return x * 8;
            });

            Assert.False(selectorExectued,
                "The selector function should never get executed if there's no value in the Result<T, TError>.");
            Assert.True(division.HasError, "Result should have error.");
            Assert.False(division.HasValue, "Result should not have a value.");
            Assert.Equal(default, division.Value);
            Assert.Equal("Can not divide '2' with '0'.", division.Error);
        }

        [Fact]
        public async Task
            Result_With_Value_Maps__Expects_Selector_Be_Executed_And_Value_To_Be_Mapped() {
            var selectorExectued = false;
            var outcome = TaskResultFunctions.Map(AssertionUtilities.DivisionAsync(10, 2), x => {
                selectorExectued = true;
                return x * 4;
            });
            var division = await outcome;

            Assert.True(selectorExectued, "The selector function should get executed since the result has value.");
            Assert.False(division.HasError, "Result not should have error.");
            Assert.True(division.HasValue, "Result should have a value.");
            Assert.Equal(20d, division.Value);
            Assert.Equal(default, division.Error);
        }
    }
}
