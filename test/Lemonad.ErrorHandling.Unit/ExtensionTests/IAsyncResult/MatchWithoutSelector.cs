using System;
using Assertion;
using Lemonad.ErrorHandling.Extensions.AsyncResult;
using Xunit;

namespace Lemonad.ErrorHandling.Unit.ExtensionTests.IAsyncResult {
    public class MatchWithoutSelector {
        [Fact]
        public async System.Threading.Tasks.Task Null_Source_Throws()
            => await Assert.ThrowsAsync<ArgumentNullException>(
                AssertionUtilities.ExtensionParameterName,
                () => ((IAsyncResult<int, int>) null).Match()
            );

        [Fact]
        public async System.Threading.Tasks.Task Result_With_Error() {
            var result = await AssertionUtilities.DivisionAsync(10, 0).MapError(x => -1d).Match();
            Assert.Equal(-1, result);
        }

        [Fact]
        public async System.Threading.Tasks.Task Result_With_Error_With_Selector() {
            var result = await AssertionUtilities.DivisionAsync(10, 0).MapError(x => -1d).Match(d => d * 2);
            Assert.Equal(-2, result);
        }

        [Fact]
        public async System.Threading.Tasks.Task Result_With_Value() {
            var result = await AssertionUtilities.DivisionAsync(10, 2).MapError(x => -1d).Match();
            Assert.Equal(5, result);
        }

        [Fact]
        public async System.Threading.Tasks.Task Result_With_Value_With_Selector() {
            var result = await AssertionUtilities.DivisionAsync(10, 2).MapError(x => -1d).Match(d => d * 2);
            Assert.Equal(10, result);
        }
    }
}