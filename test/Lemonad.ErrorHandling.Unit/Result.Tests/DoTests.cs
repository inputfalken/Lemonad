using Xunit;

namespace Lemonad.ErrorHandling.Unit.Result.Tests {
    public class DoTests {
        [Fact]
        public void
            Result_With_Error__Expects_Action_To_Be_Invoked() {
            var actionExectued = false;
            AssertionUtilities
                .Division(10, 0)
                .Do(() => actionExectued = true)
                .AssertError("Can not divide '10' with '0'.");
            Assert.True(actionExectued, "Should not get exectued since there's an error.");
        }

        [Fact]
        public void Result_With_Value__Expects_Action_To_be_Invoked() {
            var actionExectued = false;
            AssertionUtilities
                .Division(10, 2)
                .Do(() => actionExectued = true)
                .AssertValue(5);

            Assert.True(actionExectued, "Should not get exectued since there's an error.");
        }
    }
}