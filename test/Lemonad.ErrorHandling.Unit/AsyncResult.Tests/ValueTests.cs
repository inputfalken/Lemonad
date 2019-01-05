using System;
using Assertion;
using Xunit;

namespace Lemonad.ErrorHandling.Unit.AsyncResult.Tests {
    public class ValueTests {
        [Fact]
        public void With_Null_Error_Throws()
            => Assert.Throws<ArgumentNullException>(
                AssertionUtilities.ValueParamName,
                () => ErrorHandling.AsyncResult.Value<string, int?>(null)
            );

        [Fact]
        public void With_Value__Expects_Value()
            => ErrorHandling.AsyncResult.Value<int, string>(2).AssertValue(2);
    }
}