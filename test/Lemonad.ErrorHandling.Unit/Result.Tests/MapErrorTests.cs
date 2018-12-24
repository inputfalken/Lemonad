using Assertion;
using Xunit;

namespace Lemonad.ErrorHandling.Unit.Result.Tests {
    public class MapErrorTests {
        [Fact]
        public void Result_With_Error__Expects_Error_To_Be_Mapped() {
            var errorSelectorInvoked = false;
            AssertionUtilities.Division(10, 0)
                .MapError(s => {
                    errorSelectorInvoked = true;
                    return s.ToUpper();
                })
                .AssertError("CAN NOT DIVIDE '10' WITH '0'.");

            Assert.True(errorSelectorInvoked,
                "Errorselector should get exeuted since there is an error in the result.");
        }

        [Fact]
        public void Result_With_Value__Expects_Error_To_Not_Be_Mapped() {
            var errorSelectorInvoked = false;
            AssertionUtilities.Division(10, 2).MapError(s => {
                errorSelectorInvoked = true;
                return s.ToUpper();
            }).AssertValue(5d);

            Assert.False(errorSelectorInvoked,
                "Errorselector not should get exeuted since there is an value in the result.");
        }
    }
}