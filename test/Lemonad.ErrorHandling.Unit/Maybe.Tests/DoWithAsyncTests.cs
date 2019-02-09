using System;
using System.Threading.Tasks;
using Assertion;
using Lemonad.ErrorHandling.Extensions.AsyncMaybe;
using Xunit;

namespace Lemonad.ErrorHandling.Unit.Maybe.Tests {
    public class DoWithAsyncTests {
        [Fact]
        public void Maybe_With_No_Value_Null_Action__Expects_Exception() {
            Func<string, Task> argument = null;
            Assert.Throws<ArgumentNullException>(
                () => ErrorHandling.Maybe.Value("foobar").DoWithAsync(argument)
            );
        }

        [Fact]
        public void Maybe_With_Value_Null_Action__Expects_Exception() {
            Func<string, Task> argument = null;
            Assert.Throws<ArgumentNullException>(
                () => ErrorHandling.Maybe.Value("foobar").DoWithAsync(argument)
            );
        }

        [Fact]
        public async Task Maybe_With_No_Value__Expects_Action_Executed() {
            var actionExecuted = false;
            await ErrorHandling.Maybe
                .None<string>()
                .DoWithAsync(async s => {
                    await AssertionUtilities.Delay;
                    actionExecuted = true;
                })
                .AssertNone();

            Assert.False(actionExecuted);
        }

        [Fact]
        public async Task Maybe_With_Value__Expects_Action_Executed() {
            var actionExecuted = false;
            await ErrorHandling.Maybe
                .Value("foobar")
                .DoWithAsync(async s => {
                    await AssertionUtilities.Delay;
                    actionExecuted = true;
                    Assert.Equal("foobar", s);
                })
                .AssertValue("foobar");

            Assert.True(actionExecuted);
        }
    }
}