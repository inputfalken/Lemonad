using System.Threading.Tasks;
using Lemonad.ErrorHandling.Extensions.Internal;
using Xunit;

namespace Lemonad.ErrorHandling.Test.Result.Tests.Internal.Asynchronous.Result.Tests {
    public class DoWithTests {
        [Fact]
        public async Task
            Result_With_Error__Expects_Action__Not_To_Be_Invoked() {
            var actionExectued = false;
            var doWith = TaskResultFunctions.DoWith(AssertionUtilities.DivisionAsync(10, 0), d => actionExectued = true);
            var result = await doWith;

            Assert.False(actionExectued, "Should not get exectued since there's an error.");
            Assert.Equal(default, result.Value);
            Assert.Equal("Can not divide '10' with '0'.", result.Error);
            Assert.True(result.HasError, "Result should have error.");
            Assert.False(result.HasValue, "Result should not have value.");
        }

        [Fact]
        public async Task
            Result_With_Value__Expects_Action_To_Be_Invoked() {
            var actionExectued = false;
            var doWith = TaskResultFunctions.DoWith(AssertionUtilities.DivisionAsync(10, 2), d => actionExectued = true);
            var result = await doWith;

            Assert.True(actionExectued, "Should get exectued since there's no error.");
            Assert.Equal(5, result.Value);
            Assert.Equal(default, result.Error);
            Assert.False(result.HasError, "Result should not have error.");
            Assert.True(result.HasValue, "Result should have value.");
        }
    }
}
