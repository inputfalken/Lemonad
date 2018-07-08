using System.Threading.Tasks;
using Lemonad.ErrorHandling.Extensions;
using Xunit;

namespace Lemonad.ErrorHandling.Test.Task.Result.Tests {
    public class MatchTests {
        private static async Task<Result<double, string>> Division(double left, double right) {
            await System.Threading.Tasks.Task.Delay(50);

            if (right == 0)
                return await System.Threading.Tasks.Task.Run(() => $"Can not divide '{left}' with '{right}'.");
            return left / right;
        }

        [Fact]
        public async System.Threading.Tasks.Task Result_With_Error__Expect_ErrorAction() {
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
        public async System.Threading.Tasks.Task Result_With_Error__Expect_ErrorSelector() {
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
        public async System.Threading.Tasks.Task
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
        public async System.Threading.Tasks.Task
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