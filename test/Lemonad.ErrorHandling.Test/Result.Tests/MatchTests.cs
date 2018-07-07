using Lemonad.ErrorHandling.DataTypes.Result;
using Xunit;

namespace Lemonad.ErrorHandling.Test.Result.Tests {
    public class MatchTests {
        private static Result<double, string> Division(double left, double right) {
            if (right == 0)
                return $"Can not divide '{left}' with '{right}'.";

            return left / right;
        }

        [Fact]
        public void Result_With_Error__Expect_ErrorAction() {
            var selectorExectued = false;
            var errorSelectorExectued = false;
            Division(10, 0).Match(d => { selectorExectued = true; }, s => { errorSelectorExectued = true; });

            Assert.False(selectorExectued, "Selector should not get executed.");
            Assert.True(errorSelectorExectued, "Error selector should get exectued.");
        }

        [Fact]
        public void Result_With_Error__Expect_ErrorSelector() {
            var selectorExectued = false;
            var errorSelectorExectued = false;
            var result = Division(10, 0).Match(d => {
                selectorExectued = true;
                return d;
            }, s => {
                errorSelectorExectued = true;
                return -1;
            });

            Assert.False(selectorExectued, "Selector should not get executed.");
            Assert.True(errorSelectorExectued, "Error selector should get exectued.");
            Assert.Equal(-1, result);
        }

        [Fact]
        public void
            Result_With_Value__Expect_Action() {
            var selectorExectued = false;
            var errorSelectorExectued = false;
            Division(10, 2).Match(d => { selectorExectued = true; }, s => { errorSelectorExectued = true; });

            Assert.True(selectorExectued, "Selector should get executed.");
            Assert.False(errorSelectorExectued, "Error selector not should get exectued.");
        }

        [Fact]
        public void
            Result_With_Value__Expect_Selector() {
            var selectorExectued = false;
            var errorSelectorExectued = false;
            var result = Division(10, 2).Match(d => {
                selectorExectued = true;
                return d;
            }, s => {
                errorSelectorExectued = true;
                return -1;
            });

            Assert.True(selectorExectued, "Selector should get executed.");
            Assert.False(errorSelectorExectued, "Error selector not should get exectued.");
            Assert.Equal(5, result);
        }
    }
}