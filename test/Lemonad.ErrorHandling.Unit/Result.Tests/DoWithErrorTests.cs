using System;
using Assertion;
using Xunit;

namespace Lemonad.ErrorHandling.Unit.Result.Tests {
    public class DoWithErrorTests {
        [Fact]
        public void Passing_Null_Action_Throws() {
            Assert.Throws<ArgumentNullException>(AssertionUtilities.ActionParamName, () => {
                AssertionUtilities
                    .Division(10, 0)
                    .DoWithError(null)
                    .AssertError("Can not divide '10' with '0'.");
            });
        }

        [Fact]
        public void Result_With_Error__Expects_Action_To_Be_Invoked() {
            var actionExectued = false;
            AssertionUtilities
                .Division(10, 0)
                .DoWithError(_ => actionExectued = true)
                .AssertError("Can not divide '10' with '0'.");

            Assert.True(actionExectued, "Should not get exectued since there's an error.");
        }

        [Fact]
        public void Result_With_Value__Expects_Action_Not_To_Be_Invoked() {
            var actionExectued = false;
            AssertionUtilities
                .Division(10, 2)
                .DoWithError(d => actionExectued = true)
                .AssertValue(5);
            Assert.False(actionExectued, "Should not get exectued since there's an error.");
        }
    }
}