using System;
using Assertion;
using Lemonad.ErrorHandling.Extensions.AsyncResult;
using Xunit;

namespace Lemonad.ErrorHandling.Unit.ExtensionTests.IAsyncResult {
    public class MatchSingleSelector {
        [Fact]
        public async System.Threading.Tasks.Task Passing_Null_Result_Throws() =>
            await Assert.ThrowsAsync<ArgumentNullException>(
                AssertionUtilities.ExtensionParameterName,
                () => ((IAsyncResult<int, int>) null).Match(d => d * 2)
            );

        [Fact]
        public async System.Threading.Tasks.Task Passing_Null_Selector_Throws() {
            await Assert.ThrowsAsync<ArgumentNullException>(
                AssertionUtilities.SelectorName,
                () => AssertionUtilities.DivisionAsync(10, 0).MapError(x => -1d).Match((Func<double, double>) null)
            );
        }

        [Fact]
        public async System.Threading.Tasks.Task Result_With_Error() {
            Assert.Equal(-2, await AssertionUtilities.DivisionAsync(10, 0).MapError(x => -1d).Match(d => d * 2));
        }

        [Fact]
        public async System.Threading.Tasks.Task Result_With_Error_With_Selector()
            => Assert.Equal(-2, await AssertionUtilities.DivisionAsync(10, 0).MapError(x => -1d).Match(d => d * 2));

        [Fact]
        public async System.Threading.Tasks.Task Result_With_Value()
            => Assert.Equal(10, await AssertionUtilities.DivisionAsync(10, 2).MapError(x => -1d).Match(d => d * 2));

        [Fact]
        public async System.Threading.Tasks.Task Result_With_Value_With_Selector()
            => Assert.Equal(10, await AssertionUtilities.DivisionAsync(10, 2).MapError(x => -1d).Match(d => d * 2));
    }
}