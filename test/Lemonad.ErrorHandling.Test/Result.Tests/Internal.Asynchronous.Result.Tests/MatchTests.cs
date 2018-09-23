using System.Threading.Tasks;
using Lemonad.ErrorHandling.Internal;
using Xunit;

namespace Lemonad.ErrorHandling.Test.Result.Tests.Internal.Asynchronous.Result.Tests {
    public class MatchTests {
        [Fact]
        public async Task Result_With_Error__Expect_ErrorAction() {
            var selectorExectued = false;
            var errorSelectorExectued = false;
            var match = TaskResultFunctions.Match(AssertionUtilities.DivisionAsync(10, 0),
                d => { selectorExectued = true; }, s => { errorSelectorExectued = true; });
            await match;

            Assert.False(selectorExectued, "Selector should not get executed.");
            Assert.True(errorSelectorExectued, "Error selector should get exectued.");
        }

        [Fact]
        public async Task Result_With_Error__Expect_ErrorSelector() {
            var selectorExectued = false;
            var errorSelectorExectued = false;
            var match = TaskResultFunctions.Match(AssertionUtilities.DivisionAsync(10, 0), d => {
                selectorExectued = true;
                return d;
            }, s => {
                errorSelectorExectued = true;
                return -1;
            });
            var result = await match;

            Assert.False(selectorExectued, "Selector should not get executed.");
            Assert.True(errorSelectorExectued, "Error selector should get exectued.");
            Assert.Equal(-1, result);
        }

        [Fact]
        public async Task
            Result_With_Value__Expect_Action() {
            var selectorExectued = false;
            var errorSelectorExectued = false;
            var match = TaskResultFunctions.Match(AssertionUtilities.DivisionAsync(10, 2),
                d => { selectorExectued = true; }, s => { errorSelectorExectued = true; });
            await match;
            Assert.True(selectorExectued, "Selector should get executed.");
            Assert.False(errorSelectorExectued, "Error selector not should get exectued.");
        }

        [Fact]
        public async Task
            Result_With_Value__Expect_Selector() {
            var selectorExectued = false;
            var errorSelectorExectued = false;
            var match = TaskResultFunctions.Match(AssertionUtilities.DivisionAsync(10, 2), d => {
                selectorExectued = true;
                return d;
            }, s => {
                errorSelectorExectued = true;
                return -1;
            });
            var result = await match;

            Assert.True(selectorExectued, "Selector should get executed.");
            Assert.False(errorSelectorExectued, "Error selector not should get exectued.");
            Assert.Equal(5, result);
        }
    }
}