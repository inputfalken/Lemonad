using System;
using System.Linq;
using Assertion;
using Lemonad.ErrorHandling.Extensions.AsyncResult;
using Xunit;

namespace Lemonad.ErrorHandling.Unit.ExtensionTests.IAsyncResult {
    public class ToEnumerableTests {
        [Fact]
        public async System.Threading.Tasks.Task Passing_null_Source_Throws() {
            await Assert.ThrowsAsync<ArgumentNullException>(AssertionUtilities.ExtensionParameterName,
                () => ((IAsyncResult<string, int>) null).ToEnumerable()
            );
        }

        [Fact]
        public async System.Threading.Tasks.Task Result_With_Error__Expects_Enumerable_With_No_Element() {
            var result = (await AssertionUtilities.DivisionAsync(20, 0).ToEnumerable()).ToArray();
            Assert.Empty(result);
        }

        [Fact]
        public async System.Threading.Tasks.Task Result_With_Value__Expects_Enumerable_With_One_Element() {
            var result = (await AssertionUtilities.DivisionAsync(20, 2).ToEnumerable()).ToArray();
            Assert.Single(result);
            Assert.Collection(result, x => Assert.Equal(10, x));
        }
    }
}