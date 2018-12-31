using System;
using System.Threading.Tasks;
using Assertion;
using Lemonad.ErrorHandling.Extensions.AsyncResult;
using Lemonad.ErrorHandling.Extensions.Result;
using Xunit;

namespace Lemonad.ErrorHandling.Unit.ExtensionTests.Result {
    public class ToAsyncResult {
        [Fact]
        public async Task Result_With_Value__Expects_AsyncResult_With_Value()
            => await AssertionUtilities.Division(10, 5).ToAsyncResult().AssertValue(2);

        [Fact]
        public async Task Result_With_Error__Expects_AsyncResult_With_Error()
            => await AssertionUtilities.Division(10, 0).ToAsyncResult().AssertError("Can not divide '10' with '0'.");

        [Fact]
        public void Passing_Null_Result_Throws_Exception()
            => Assert.Throws<ArgumentNullException>(
                AssertionUtilities.ExtensionParameterName,
                () => ((IResult<string, int>) null).ToAsyncResult()
            );
    }
}