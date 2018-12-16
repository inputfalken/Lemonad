using System;
using Xunit;

namespace Lemonad.ErrorHandling.Unit.Maybe.Tests {
    public class DoWithTests {
        [Fact]
        public void Maybe_With_No_Value__Expects_Action_Executed() {
            var actionExecuted = false;
            var value = ErrorHandling.Maybe.None<string>().DoWith(s => { actionExecuted = true; });

            Assert.False(actionExecuted);
            Assert.False(value.HasValue);
            Assert.Equal(default, value.Value);
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
            var value = ErrorHandling.Maybe.Value("foobar").DoWith(s => {
                actionExecuted = true;
                Assert.Equal("foobar", s);
            });

            Assert.True(actionExecuted);
            Assert.True(value.HasValue);
            Assert.Equal("foobar", value.Value);
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