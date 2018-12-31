using System;
using Assertion;
using Lemonad.ErrorHandling.Extensions.Result;
using Xunit;

namespace Lemonad.ErrorHandling.Unit.ExtensionTests.Result {
    public class MatchWithSingleSelector {
        [Fact]
        public void Passing_Null_Result_Throws() =>
            Assert.Throws<ArgumentNullException>(
                AssertionUtilities.ExtensionParameterName,
                () => ((IResult<int, int>) null).Match(d => d * 2)
            );

        [Fact]
        public void Passing_Null_Selector_Throws() {
            Assert.Throws<ArgumentNullException>(
                AssertionUtilities.SelectorName,
                () => AssertionUtilities.Division(10, 0).MapError(x => -1d).Match((Func<double, double>) null)
            );
        }

        [Fact]
        public void Result_With_Error()
            => Assert.Equal(-2, AssertionUtilities.Division(10, 0).MapError(x => -1d).Match(d => d * 2));

        [Fact]
        public void Result_With_Error_With_Selector()
            => Assert.Equal(-2, AssertionUtilities.Division(10, 0).MapError(x => -1d).Match(d => d * 2));

        [Fact]
        public void Result_With_Value()
            => Assert.Equal(10, AssertionUtilities.Division(10, 2).MapError(x => -1d).Match(d => d * 2));

        [Fact]
        public void Result_With_Value_With_Selector()
            => Assert.Equal(10, AssertionUtilities.Division(10, 2).MapError(x => -1d).Match(d => d * 2));
    }
}