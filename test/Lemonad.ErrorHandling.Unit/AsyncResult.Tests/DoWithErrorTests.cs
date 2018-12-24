using System.Threading.Tasks;
using Assertion;
using Lemonad.ErrorHandling.Extensions.AsyncResult;
using Xunit;

namespace Lemonad.ErrorHandling.Unit.AsyncResult.Tests {
    public class DoWithErrorTests {
        [Fact]
        public async Task
            Result_With_Error__Expects_Action_To_Be_Invoked() {
            var actionExectued = false;
            await AssertionUtilities
                .DivisionAsync(10, 0)
                .DoWithError(d => actionExectued = true)
                .AssertError("Can not divide '10' with '0'.");

            Assert.True(actionExectued, "Should not get exectued since there's an error.");
        }

        [Fact]
        public async Task
            Result_With_Value__Expects_Action_Not_To_Be_Invoked() {
            var actionExectued = false;
            await AssertionUtilities
                .DivisionAsync(10, 2)
                .DoWithError(d => actionExectued = true)
                .AssertValue(5);

            Assert.False(actionExectued, "Should not get exectued since there's an error.");
        }
    }
}