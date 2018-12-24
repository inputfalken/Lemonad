using System.Threading.Tasks;
using Assertion;
using Lemonad.ErrorHandling.Extensions.AsyncResult;
using Xunit;

namespace Lemonad.ErrorHandling.Unit.AsyncResult.Tests {
    public class MapErrorTests {
        [Fact]
        public async Task Result_With_Error__Expects_Error_To_Be_Mapped() {
            var errorSelectorInvoked = false;
            await AssertionUtilities
                .DivisionAsync(10, 0)
                .MapError(s => {
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
                .DivisionAsync(10, 2)
                .MapError(s => {
                    errorSelectorInvoked = true;
                    return s.ToUpper();
                })
                .AssertValue(5d);

            Assert.False(errorSelectorInvoked,
                "Errorselector not should get exeuted since there is an value in the result.");
        }
    }
}