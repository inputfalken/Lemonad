using System;
using Xunit;

namespace Lemonad.ErrorHandling.Test.Maybe.Tests {
    public class DoTests {
        [Fact]
        public void Maybe_With_No_Value__Expects_Action_Executed() {
            var actionExecuted = false;
            var value = ErrorHandling.Maybe.None<string>().Do(() => actionExecuted = true);

            Assert.True(actionExecuted);
            Assert.False(value.HasValue);
            Assert.Equal(default, value.Value);
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
            var value = ErrorHandling.Maybe.Value("foobar").Do(() => actionExecuted = true);

            Assert.True(actionExecuted);
            Assert.True(value.HasValue);
            Assert.Equal("foobar", value.Value);
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