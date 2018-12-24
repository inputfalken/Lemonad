using System;
using Assertion;
using Xunit;

namespace Lemonad.ErrorHandling.Unit.Maybe.Tests {
    public class DoWithTests {
        [Fact]
        public void Maybe_With_No_Value__Expects_Action_Executed() {
            var actionExecuted = false;
            ErrorHandling.Maybe
                .None<string>()
                .DoWith(s => { actionExecuted = true; })
                .AssertNone();

            Assert.False(actionExecuted);
        }

        [Fact]
        public void Maybe_With_No_Value_Null_Action__Expects_Exception() {
            Action<string> argument = null;
            Assert.Throws<ArgumentNullException>(
                "someAction",
                () => ErrorHandling.Maybe.Value("foobar").DoWith(argument)
            );
        }

        [Fact]
        public void Maybe_With_Value__Expects_Action_Executed() {
            var actionExecuted = false;
            ErrorHandling.Maybe
                .Value("foobar")
                .DoWith(s => {
                    actionExecuted = true;
                    Assert.Equal("foobar", s);
                })
                .AssertValue("foobar");

            Assert.True(actionExecuted);
        }

        [Fact]
        public void Maybe_With_Value_Null_Action__Expects_Exception() {
            Action<string> argument = null;
            Assert.Throws<ArgumentNullException>(
                "someAction",
                () => ErrorHandling.Maybe.Value("foobar").DoWith(argument)
            );
        }
    }
}