using System;
using System.Threading.Tasks;
using Assertion;
using Lemonad.ErrorHandling.Extensions.AsyncResult;
using Xunit;

namespace Lemonad.ErrorHandling.Unit.AsyncResult.Tests {
    public class CastErrorTests {
        [Fact]
        public void Result_With_Value_Using_Invalid_Cast_Expects_No_Exception() {
            const int identity = 0;
            AssertionUtilities
                .GetProgramAsync(identity)
                .CastError<string>()
                .AssertValue(0);
        }

        [Fact]
        public void Result_With_Error_Using_Valid_Cast_Expects_Error() {
            const int identity = 0;
            AssertionUtilities
                .GetProgramAsync(identity)
                .CastError<int>()
                .AssertValue(0);
        }

        [Fact]
        public Task Result_With_Error_Using_Invalid_Cast_Expects_Cast_Exception() {
            const int identity = 1;
            return Assert.ThrowsAsync<InvalidCastException>(async () => await AssertionUtilities.GetProgramAsync(identity).CastError<string>());
        }

        [Fact]
        public void Result_With_Error_Using_Valid_Cast_Expects_Value_As_Int() {
            const int identity = 1;
            AssertionUtilities
                .GetProgramAsync(identity)
                .CastError<int>()
                .AssertError(identity);
        }
    }
}