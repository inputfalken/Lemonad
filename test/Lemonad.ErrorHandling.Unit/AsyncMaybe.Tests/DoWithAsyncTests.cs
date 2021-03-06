using System;
using System.Threading.Tasks;
using Assertion;
using Lemonad.ErrorHandling.Extensions.AsyncMaybe;
using Xunit;

namespace Lemonad.ErrorHandling.Unit.AsyncMaybe.Tests {
    public class DoWithAsyncTests {
        [Fact]
        public async Task Maybe_With_No_Value__Expects_Action_Executed() {
            var actionExecuted = false;
            await ErrorHandling.AsyncMaybe
                .None<string>()
                .DoWithAsync(async s => {
                    await AssertionUtilities.Delay;
                    actionExecuted = true;
                })
                .AssertNone();

            Assert.False(actionExecuted);
        }

        [Fact]
        public void Maybe_With_No_Value_Null_Action__Expects_Exception() {
            Func<string, Task> argument = null;
            Assert.Throws<ArgumentNullException>(
                () => ErrorHandling.AsyncMaybe.Value("foobar").DoWithAsync(argument)
            );
        }

        [Fact]
        public async Task Maybe_With_Value__Expects_Action_Executed() {
            var actionExecuted = false;
            await ErrorHandling.AsyncMaybe
                .Value("foobar")
                .DoWithAsync(async s => {
                    await AssertionUtilities.Delay;
                    actionExecuted = true;
                    Assert.Equal("foobar", s);
                })
                .AssertValue("foobar");

            Assert.True(actionExecuted);
        }

        [Fact]
        public void Maybe_With_Value_Null_Action__Expects_Exception() {
            Func<string, Task> argument = null;
            Assert.Throws<ArgumentNullException>(
                () => ErrorHandling.AsyncMaybe.Value("foobar").DoWithAsync(argument)
            );
        }
    }
}