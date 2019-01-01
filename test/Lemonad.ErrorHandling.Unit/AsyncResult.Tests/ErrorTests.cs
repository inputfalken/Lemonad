using System;
using Assertion;
using Xunit;

namespace Lemonad.ErrorHandling.Unit.AsyncResult.Tests {
    public class ErrorTests {
        [Fact]
        public void With_Error__Expects_Error() {
            ErrorHandling.AsyncResult.Error<string, int>(2).AssertError(2);
        }

        [Fact]
        public void With_Null_Error_Throws()
            => Assert.Throws<ArgumentNullException>(
                AssertionUtilities.ErrorParamName,
                () => ErrorHandling.AsyncResult.Error<string, int?>(null)
            );
    }
}