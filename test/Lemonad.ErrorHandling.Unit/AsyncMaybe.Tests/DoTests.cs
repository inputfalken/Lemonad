using System;
using System.Threading.Tasks;
using Assertion;
using Lemonad.ErrorHandling.Extensions.AsyncMaybe;
using Xunit;

namespace Lemonad.ErrorHandling.Unit.AsyncMaybe.Tests {
    public class DoTests {
        [Fact]
        public async Task Maybe_With_No_Value__Expects_Action_Executed() {
            var actionExecuted = false;
            await ErrorHandling.AsyncMaybe
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
                () => ErrorHandling.AsyncMaybe.Value("foobar").Do(argument)
            );
        }

        [Fact]
        public async Task Maybe_With_Value__Expects_Action_Executed() {
            var actionExecuted = false;
            await ErrorHandling.AsyncMaybe.Value("foobar")
                .Do(() => actionExecuted = true)
                .AssertValue("foobar");

            Assert.True(actionExecuted);
        }

        [Fact]
        public void Maybe_With_Value_Null_Action__Expects_Exception() {
            Action argument = null;
            Assert.Throws<ArgumentNullException>(
                "action",
                () => ErrorHandling.AsyncMaybe.Value("foobar").Do(argument)
            );
        }
    }
}