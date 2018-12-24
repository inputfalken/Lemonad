using Assertion;
using Xunit;

namespace Lemonad.ErrorHandling.Unit.Result.Tests {
    public class FullMapTests {
        [Fact]
        public void Result_With_Error_Expects__Selector_Never__To_Be_Executed_And_ErrorSelector_To_Be_Invoked() {
            var selectorExectued = false;
            var errorSelectorExectued = false;
            AssertionUtilities
                .Division(10, 0)
                .FullMap(d => {
                    selectorExectued = true;
                    return d * 2;
                }, s => {
                    errorSelectorExectued = true;
                    return s.ToUpper();
                }).AssertError("CAN NOT DIVIDE '10' WITH '0'.");

            Assert.False(selectorExectued, "Should not get exectued since there's an error from the result.");
            Assert.True(errorSelectorExectued, "Should get exectued since there's an error from the result.");
        }

        [Fact]
        public void Result_With_Value_Expects__Selector_To_Be_Executed_And_ErrorSelector_To_Never_Be_Invoked() {
            var selectorExectued = false;
            var errorSelectorExectued = false;
            AssertionUtilities
                .Division(10, 2)
                .FullMap(d => {
                    selectorExectued = true;
                    return d * 10;
                }, s => {
                    errorSelectorExectued = true;
                    return s.ToUpper();
                }).AssertValue(50);

            Assert.True(selectorExectued, "Should get exectued since there's an value from the result.");
            Assert.False(errorSelectorExectued, "Should not get exectued since there's an value from the result.");
        }
    }
}