using System;
using System.Threading.Tasks;
using Assertion;
using Lemonad.ErrorHandling.Extensions.AsyncResult;
using Xunit;

namespace Lemonad.ErrorHandling.Unit.Result.Tests {
    public class DoWithErrorAsync {
        [Fact]
        public void Passing_Null_Action_Throws() {
            Assert.Throws<ArgumentNullException>(
                AssertionUtilities.ActionParamName,
                () => AssertionUtilities
                    .Division(10, 0)
                    .DoWithErrorAsync(null)
            );
        }

        [Fact]
        public async Task Result_With_Error__Expects_Action_To_Be_Invoked() {
            var actionExectued = false;
            await AssertionUtilities
                .Division(10, 0)
                .DoWithErrorAsync(async d => {
                    await Task.Delay(200);
                    actionExectued = true;
                })
                .AssertError("Can not divide '10' with '0'.");

            Assert.True(actionExectued, "Should not get exectued since there's an error.");
        }

        [Fact]
        public async Task Result_With_Value__Expects_Action_Not_To_Be_Invoked() {
            var actionExectued = false;
            await AssertionUtilities
                .Division(10, 2)
                .DoWithErrorAsync(async d => {
                    await Task.Delay(200);
                    actionExectued = true;
                }).AssertValue(5);

            Assert.False(actionExectued, "Should not get exectued since there's an error.");
        }
    }
}