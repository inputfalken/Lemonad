using System;
using Assertion;
using Lemonad.ErrorHandling.Extensions.AsyncResult;
using Xunit;

namespace Lemonad.ErrorHandling.Unit.ExtensionTests.IAsyncResult {
    public class SwapTests {
        [Fact]
        public void Passing_Null_Source_Throws() =>
            Assert.Throws<ArgumentNullException>(
                AssertionUtilities.ExtensionParameterName,
                () => ((IAsyncResult<int, string>) null).Swap()
            );

        [Fact]
        public async System.Threading.Tasks.Task Result_With_Error() => await AssertionUtilities.DivisionAsync(10, 0)
            .Swap().AssertValue("Can not divide '10' with '0'.");

        [Fact]
        public async System.Threading.Tasks.Task Result_With_Value() =>
            await AssertionUtilities.DivisionAsync(10, 2).Swap().AssertError(5);
    }
}