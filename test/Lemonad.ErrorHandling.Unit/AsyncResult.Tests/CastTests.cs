using System;
using System.Threading.Tasks;
using Assertion;
using Lemonad.ErrorHandling.Extensions.AsyncResult;
using Xunit;

namespace Lemonad.ErrorHandling.Unit.AsyncResult.Tests {
    public class CastTests {
        [Fact]
        public async Task Result_With_Error_Using_Invalid_Cast_Expects_No_Exception() {
            const int identity = 3;
            await AssertionUtilities
                .GetGenderAsync(identity)
                .Cast<string>()
                .AssertError("Could not determine gender.");
        }

        [Fact]
        public async Task Result_With_Error_Using_Valid_Cast_Expects_Error() {
            const int identity = 3;
            await AssertionUtilities
                .GetGenderAsync(identity)
                .Cast<int>()
                .AssertError("Could not determine gender.");
        }

        [Fact]
        public async Task Result_With_Value_Using_Invalid_Cast_Expects_Cast_Exception() {
            const int identity = 0;
            await Assert.ThrowsAsync<InvalidCastException>(async () => await AssertionUtilities.GetGenderAsync(identity).Cast<string>());
        }

        [Fact]
        public async Task Result_With_Value_Using_Valid_Cast_Expects_Value_As_Int() {
            const int identity = 0;
            await AssertionUtilities
                .GetGenderAsync(identity)
                .Cast<int>()
                .AssertValue(identity);
        }
    }
}