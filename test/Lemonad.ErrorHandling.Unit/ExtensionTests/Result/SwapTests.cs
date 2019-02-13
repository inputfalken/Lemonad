using System;
using Assertion;
using Lemonad.ErrorHandling.Extensions.Result;
using Xunit;

namespace Lemonad.ErrorHandling.Unit.ExtensionTests.Result {
    public class SwapTests {
        [Fact]
        public void Passing_Null_Source_Throws() =>
            Assert.Throws<ArgumentNullException>(
                AssertionUtilities.ExtensionParameterName,
                () => ((IResult<int, string>) null).Swap()
            );

        [Fact]
        public void Result_With_Error() =>
            AssertionUtilities.Division(10, 0).Swap().AssertValue("Can not divide '10' with '0'.");

        [Fact]
        public void Result_With_Value() => AssertionUtilities.Division(10, 2).Swap().AssertError(5);
    }
}