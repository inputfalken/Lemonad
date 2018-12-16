using System.Linq;
using System.Threading.Tasks;
using Xunit;

namespace Lemonad.ErrorHandling.Unit.AsyncResult.Tests {
    public class ToErrorEnumerableTests {
        [Fact]
        public async Task Result_With_Error__Expects_Enumerable_With_Element_Element() {
            var result = (await AssertionUtilities.DivisionAsync(20, 0).ToErrorEnumerable()).ToArray();
            Assert.Single(result);
            Assert.Collection(result, x => Assert.Equal("Can not divide '20' with '0'.", x));
        }

        [Fact]
        public async Task Result_With_Value__Expects_Enumerable_With_No_Element() {
            var result = (await AssertionUtilities.DivisionAsync(20, 2).ToErrorEnumerable()).ToArray();
            Assert.Empty(result);
        }
    }
}