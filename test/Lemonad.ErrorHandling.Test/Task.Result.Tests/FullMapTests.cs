using System.Threading.Tasks;
using Lemonad.ErrorHandling.DataTypes.Result;
using Lemonad.ErrorHandling.DataTypes.Result.Extensions;
using Xunit;

namespace Lemonad.ErrorHandling.Test.Task.Result.Tests {
    public class FullMapTests {
        private static async Task<Result<double, string>> Division(double left, double right) {
            await System.Threading.Tasks.Task.Delay(50);

            if (right == 0)
                return await System.Threading.Tasks.Task.Run(() => $"Can not divide '{left}' with '{right}'.");
            return left / right;
        }

        [Fact]
        public async System.Threading.Tasks.Task
            Result_With_Error_Expects__Selector_Never__To_Be_Executed_And_ErrorSelector_To_Be_Invoked() {
            var selectorExectued = false;
            var errorSelectorExectued = false;
            var task = Division(10, 0).FullMap(d => {
                selectorExectued = true;
                return d * 2;
            }, s => {
                errorSelectorExectued = true;
                return s.ToUpper();
            });
            Assert.False(errorSelectorExectued, "Should not get executed until value is awaited.");

            var result = await task;

            Assert.False(selectorExectued, "Should not get exectued since there's an error from the result.");
            Assert.Equal(default, result.Value);
            Assert.Equal("CAN NOT DIVIDE '10' WITH '0'.", result.Error);
            Assert.True(result.HasError, "Result should have error.");
            Assert.False(result.HasValue, "Result should not have value.");
        }

        [Fact]
        public async System.Threading.Tasks.Task
            Result_With_Value_Expects__Selector_To_Be_Executed_And_ErrorSelector_To_Never_Be_Invoked() {
            var selectorExectued = false;
            var errorSelectorExectued = false;
            var task = Division(10, 2).FullMap(d => {
                selectorExectued = true;
                return d * 10;
            }, s => {
                errorSelectorExectued = true;
                return s.ToUpper();
            });

            Assert.False(selectorExectued, "Should not get exectued until the value is awaited.");

            var result = await task;

            Assert.True(selectorExectued, "Should get exectued since there's an value from the result.");
            Assert.False(errorSelectorExectued, "Should not get exectued since there's an value from the result.");
            Assert.Equal(50, result.Value);
            Assert.Equal(default, result.Error);
            Assert.False(result.HasError, "Result should not have error.");
            Assert.True(result.HasValue, "Result should have value.");
        }
    }
}