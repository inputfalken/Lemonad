using System.Threading.Tasks;
using Xunit;

namespace Lemonad.ErrorHandling.Test.AsyncResult.Tests {
    public class MapTests {
        [Fact]
        public async Task Result_With_Error_Maps__Expects_Selector_Never_Be_Executed() {
            var selectorExectued = false;
            var division = await AssertionUtilities
                .DivisionAsync(2, 0)
                .ToAsyncResult()
                .Map(x => {
                    selectorExectued = true;
                    return x * 8;
                }).Match(d => d, s => -1);

            Assert.False(selectorExectued,
                "The selector function should never get executed if there's no value in the Result<T, TError>.");
            Assert.Equal(-1, division);
        }

        [Fact]
        public async Task Result_With_Value_Maps__Expects_Selector_Be_Executed_And_Value_To_Be_Mapped() {
            var selectorExectued = false;
            var division = await AssertionUtilities
                .DivisionAsync(10, 2)
                .ToAsyncResult()
                .Map(x => {
                        selectorExectued = true;
                        return x * 4;
                    }
                ).Match(x => x, x => -1);
            Assert.True(selectorExectued, "The selector function should get executed since the result has value.");
            Assert.Equal(20d, division);
        }
    }
}