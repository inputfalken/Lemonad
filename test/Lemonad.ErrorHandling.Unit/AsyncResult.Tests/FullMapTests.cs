using System;
using System.Threading.Tasks;
using Assertion;
using Lemonad.ErrorHandling.Extensions.AsyncResult;
using Xunit;

namespace Lemonad.ErrorHandling.Unit.AsyncResult.Tests {
    public class FullMapTests {
        [Fact]
        public void Passing_Null_ErrorSelector_Throws()
            => Assert.Throws<ArgumentNullException>(
                AssertionUtilities.ErrorSelectorName,
                () => AssertionUtilities.DivisionAsync(10, 2).FullMap<string, string>(_ => string.Empty, null)
            );

        [Fact]
        public void Passing_Null_Selector_Throws()
            => Assert.Throws<ArgumentNullException>(
                AssertionUtilities.SelectorName,
                () => AssertionUtilities.DivisionAsync(10, 2).FullMap<string, string>(null, s => s)
            );

        [Fact]
        public async Task Result_With_Error_Expects__Selector_Never__To_Be_Executed_And_ErrorSelector_To_Be_Invoked() {
            var selectorExectued = false;
            var errorSelectorExectued = false;
            await AssertionUtilities
                .DivisionAsync(10, 0)
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
        public async Task Result_With_Value_Expects__Selector_To_Be_Executed_And_ErrorSelector_To_Never_Be_Invoked() {
            var selectorExectued = false;
            var errorSelectorExectued = false;
            await AssertionUtilities
                .DivisionAsync(10, 2)
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