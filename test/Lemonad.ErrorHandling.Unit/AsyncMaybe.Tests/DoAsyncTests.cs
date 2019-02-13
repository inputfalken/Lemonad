using System;
using System.Threading.Tasks;
using Assertion;
using Lemonad.ErrorHandling.Extensions.AsyncMaybe;
using Xunit;

namespace Lemonad.ErrorHandling.Unit.AsyncMaybe.Tests {
    public class DoAsyncTests {
        [Fact]
        public void Maybe_With_No_Value_Null_Action__Expects_Exception() {
            Func<Task> argument = null;
            Assert.Throws<ArgumentNullException>(
                "action",
                () => ErrorHandling.AsyncMaybe.Value("foobar").DoAsync(argument)
            );
        }

        [Fact]
        public void Maybe_With_Value_Null_Action__Expects_Exception() {
            Func<Task> argument = null;
            Assert.Throws<ArgumentNullException>(
                "action",
                () => ErrorHandling.AsyncMaybe.Value("foobar").DoAsync(argument)
            );
        }

        [Fact]
        public async Task Maybe_With_No_Value__Expects_Action_Executed() {
            var actionExecuted = false;
            await ErrorHandling.AsyncMaybe
                .None<string>()
                .DoAsync(async () => {
                    await AssertionUtilities.Delay;
                    actionExecuted = true;
                })
                .AssertNone();

            Assert.True(actionExecuted);
        }

        [Fact]
        public async Task Maybe_With_Value__Expects_Action_Executed() {
            var actionExecuted = false;
            await ErrorHandling.AsyncMaybe.Value("foobar")
                .DoAsync(async () => {
                    await AssertionUtilities.Delay;
                    actionExecuted = true;
                })
                .AssertValue("foobar");

            Assert.True(actionExecuted);
        }
    }
}