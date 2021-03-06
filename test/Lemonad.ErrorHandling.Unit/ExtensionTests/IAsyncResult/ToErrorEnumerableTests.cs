using System;
using System.Linq;
using Assertion;
using Lemonad.ErrorHandling.Extensions.AsyncResult;
using Xunit;

namespace Lemonad.ErrorHandling.Unit.ExtensionTests.IAsyncResult {
    public class ToErrorEnumerableTests {
        [Fact]
        public async System.Threading.Tasks.Task Passing_null_Source_Throws() {
            await Assert.ThrowsAsync<ArgumentNullException>(AssertionUtilities.ExtensionParameterName,
                () => ((IAsyncResult<string, int>) null).ToErrorEnumerable()
            );
        }

        [Fact]
        public async System.Threading.Tasks.Task Result_With_Error__Expects_Enumerable_With_Element_Element() {
            var result = (await AssertionUtilities.DivisionAsync(20, 0).ToErrorEnumerable()).ToArray();
            Assert.Single(result);
            Assert.Collection(result, x => Assert.Equal("Can not divide '20' with '0'.", x));
        }

        [Fact]
        public async System.Threading.Tasks.Task Result_With_Value__Expects_Enumerable_With_No_Element() {
            var result = (await AssertionUtilities.DivisionAsync(20, 2).ToErrorEnumerable()).ToArray();
            Assert.Empty(result);
        }
    }
}