using System;
using System.Threading.Tasks;
using Assertion;
using Lemonad.ErrorHandling.Extensions.AsyncResult;
using Xunit;

namespace Lemonad.ErrorHandling.Unit.Result.Tests {
    public class MapErrorAsyncTests {
        [Fact]
        public void Passing_Null_Selector_Throws() {
            Assert.Throws<ArgumentNullException>(
                AssertionUtilities.SelectorName,
                () => AssertionUtilities
                    .Division(2, 0)
                    .MapErrorAsync<string>(null)
            );
        }

        [Fact]
        public async Task Result_With_Error__Expects_Error_To_Be_Mapped() {
            var errorSelectorInvoked = false;
            await AssertionUtilities
                .Division(10, 0)
                .MapErrorAsync(async s => {
                    await AssertionUtilities.Delay;
                    errorSelectorInvoked = true;
                    return s.ToUpper();
                }).AssertError("CAN NOT DIVIDE '10' WITH '0'.");

            Assert.True(errorSelectorInvoked,
                "Errorselector should get exeuted since there is an error in the result.");
        }

        [Fact]
        public async Task Result_With_Value__Expects_Error_To_Not_Be_Mapped() {
            var errorSelectorInvoked = false;
            await AssertionUtilities
                .Division(10, 2)
                .MapErrorAsync(async s => {
                    await AssertionUtilities.Delay;
                    errorSelectorInvoked = true;
                    return s.ToUpper();
                })
                .AssertValue(5d);

            Assert.False(errorSelectorInvoked,
                "Errorselector not should get exeuted since there is an value in the result.");
        }
    }
}