using System;
using System.Threading.Tasks;
using Assertion;
using Lemonad.ErrorHandling.Extensions.AsyncMaybe;
using Xunit;

namespace Lemonad.ErrorHandling.Unit.AsyncMaybe.Tests {
    public class DoWithTests {
        [Fact]
        public async Task Maybe_With_No_Value__Expects_Action_Executed() {
            var actionExecuted = false;
            await ErrorHandling.AsyncMaybe
                .None<string>()
                .DoWith(s => { actionExecuted = true; })
                .AssertNone();

            Assert.False(actionExecuted);
        }

        [Fact]
        public void Maybe_With_No_Value_Null_Action__Expects_Exception() {
            Action<string> argument = null;
            Assert.Throws<ArgumentNullException>(
                () => ErrorHandling.Maybe.Value("foobar").DoWith(argument)
            );
        }

        [Fact]
        public async Task Maybe_With_Value__Expects_Action_Executed() {
            var actionExecuted = false;
            await ErrorHandling.AsyncMaybe
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
                () => ErrorHandling.Maybe.Value("foobar").DoWith(argument)
            );
        }
    }
}