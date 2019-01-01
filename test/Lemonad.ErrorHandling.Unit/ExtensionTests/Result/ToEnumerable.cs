using System;
using Assertion;
using Lemonad.ErrorHandling.Extensions.Result;
using Xunit;

namespace Lemonad.ErrorHandling.Unit.ExtensionTests.Result {
    public class ToEnumerable {
        [Fact]
        public void With_Value_Expects_Empty_Enumerable() {
            Assert.Equal(AssertionUtilities.Division(10, 2).ToEnumerable(), new[] {5d});
        }

        [Fact]
        public void With_Error_Expects_Enumerable_With_Element() {
            Assert.Empty(AssertionUtilities.Division(10, 0).ToEnumerable());
        }

        [Fact]
        public void Null_Source_Throws()
            => Assert.Throws<ArgumentNullException>(
                AssertionUtilities.ExtensionParameterName,
                () => ((IResult<int, string>) null).ToEnumerable()
            );
    }
}