using System;
using Assertion;
using Xunit;

namespace Lemonad.ErrorHandling.Unit.Maybe.Tests {
    public class DoTests {
        [Fact]
        public void Maybe_With_No_Value__Expects_Action_Executed() {
            var actionExecuted = false;
            ErrorHandling.Maybe
                .None<string>()
                .Do(() => actionExecuted = true)
                .AssertNone();

            Assert.True(actionExecuted);
        }

        [Fact]
        public void Maybe_With_No_Value_Null_Action__Expects_Exception() {
            Action argument = null;
            Assert.Throws<ArgumentNullException>(
                "action",
                () => ErrorHandling.Maybe.Value("foobar").Do(argument)
            );
        }

        [Fact]
        public void Maybe_With_Value__Expects_Action_Executed() {
            var actionExecuted = false;
            ErrorHandling.Maybe.Value("foobar")
                .Do(() => actionExecuted = true)
                .AssertValue("foobar");

            Assert.True(actionExecuted);
        }

        [Fact]
        public void Maybe_With_Value_Null_Action__Expects_Exception() {
            Action argument = null;
            Assert.Throws<ArgumentNullException>(
                "action",
                () => ErrorHandling.Maybe.Value("foobar").Do(argument)
            );
        }
    }
}