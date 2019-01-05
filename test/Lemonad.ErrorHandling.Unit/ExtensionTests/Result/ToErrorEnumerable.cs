using System;
using Assertion;
using Lemonad.ErrorHandling.Extensions.Result;
using Xunit;

namespace Lemonad.ErrorHandling.Unit.ExtensionTests.Result {
    public class ToErrorEnumerable {
        [Fact]
        public void Null_Source_Throws()
            => Assert.Throws<ArgumentNullException>(
                AssertionUtilities.ExtensionParameterName,
                () => ((IResult<int, string>) null).ToErrorEnumerable()
            );

        [Fact]
        public void With_Error_Expects_Enumerable_With_Element() {
            Assert.Equal(
                AssertionUtilities.Division(10, 0).ToErrorEnumerable(),
                new[] {"Can not divide '10' with '0'."}
            );
        }

        [Fact]
        public void With_Value_Expects_Empty_Enumerable() {
            Assert.Empty(AssertionUtilities.Division(10, 2).ToErrorEnumerable());
        }
    }
}