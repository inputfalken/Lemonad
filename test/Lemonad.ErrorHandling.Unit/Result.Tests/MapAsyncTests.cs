using System;
using System.Threading.Tasks;
using Assertion;
using Lemonad.ErrorHandling.Extensions.AsyncResult;
using Xunit;

namespace Lemonad.ErrorHandling.Unit.Result.Tests {
    public class MapAsyncTests {
        [Fact]
        public void Passing_Null_Selector_Throws() {
            Assert.Throws<ArgumentNullException>(
                AssertionUtilities.SelectorName,
                () => AssertionUtilities
                    .Division(2, 0)
                    .MapAsync<string>(null)
            );
        }

        [Fact]
        public async Task Result_With_Error_Maps__Expects_Selector_Never_Be_Executed() {
            var selectorExectued = false;
            await AssertionUtilities
                .Division(2, 0)
                .MapAsync(async x => {
                    await AssertionUtilities.Delay;
                    selectorExectued = true;
                    return x * 8;
                })
                .AssertError("Can not divide '2' with '0'.");

            Assert.False(selectorExectued,
                "The selector function should never get executed if there's no value in the Result<T, TError>.");
        }

        [Fact]
        public async Task Result_With_Value_Maps__Expects_Selector_Be_Executed_And_Value_To_Be_Mapped() {
            var selectorExectued = false;
            await AssertionUtilities
                .Division(10, 2)
                .MapAsync(async x => {
                    await AssertionUtilities.Delay;
                    selectorExectued = true;
                    return x * 4;
                })
                .AssertValue(20d);

            Assert.True(selectorExectued, "The selector function should get executed since the result has value.");
        }
    }
}