using System.Threading.Tasks;
using Lemonad.ErrorHandling.Extensions;
using Xunit;
using static Lemonad.ErrorHandling.Test.Asynchronous.Result.Tests.AssertionUtilitiesAsync;

namespace Lemonad.ErrorHandling.Test.Asynchronous.Result.Tests {
    public class MatchTests {
        [Fact]
        public async Task Result_With_Error__Expect_ErrorAction() {
            var selectorExectued = false;
            var errorSelectorExectued = false;
            var match = Division(10, 0)
                .Match(d => { selectorExectued = true; }, s => { errorSelectorExectued = true; });
            Assert.False(errorSelectorExectued, "Error should not get exectued until it's awaited.");
            await match;

            Assert.False(selectorExectued, "Selector should not get executed.");
            Assert.True(errorSelectorExectued, "Error selector should get exectued.");
        }

        [Fact]
        public async Task Result_With_Error__Expect_ErrorSelector() {
            var selectorExectued = false;
            var errorSelectorExectued = false;
            var match = Division(10, 0).Match(d => {
                selectorExectued = true;
                return d;
            }, s => {
                errorSelectorExectued = true;
                return -1;
            });
            Assert.False(errorSelectorExectued, "Error should not get exectued until it's awaited.");
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
            var match = Division(10, 2)
                .Match(d => { selectorExectued = true; }, s => { errorSelectorExectued = true; });
            Assert.False(selectorExectued, "Selector should not get executed until it's awaited.");
            await match;
            Assert.True(selectorExectued, "Selector should get executed.");
            Assert.False(errorSelectorExectued, "Error selector not should get exectued.");
        }

        [Fact]
        public async Task
            Result_With_Value__Expect_Selector() {
            var selectorExectued = false;
            var errorSelectorExectued = false;
            var match = Division(10, 2).Match(d => {
                selectorExectued = true;
                return d;
            }, s => {
                errorSelectorExectued = true;
                return -1;
            });
            Assert.False(selectorExectued, "Selector should not get executed until it's awaited.");
            var result = await match;

            Assert.True(selectorExectued, "Selector should get executed.");
            Assert.False(errorSelectorExectued, "Error selector not should get exectued.");
            Assert.Equal(5, result);
        }
    }
}