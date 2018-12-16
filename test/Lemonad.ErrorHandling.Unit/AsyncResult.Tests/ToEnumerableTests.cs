using System.Linq;
using System.Threading.Tasks;
using Xunit;

namespace Lemonad.ErrorHandling.Unit.AsyncResult.Tests {
    public class ToEnumerableTests {
        [Fact]
        public async Task Result_With_Error__Expects_Enumerable_With_No_Element() {
            var result = (await AssertionUtilities.DivisionAsync(20, 0).ToEnumerable()).ToArray();
            Assert.Empty(result);
        }

        [Fact]
        public async Task Result_With_Value__Expects_Enumerable_With_One_Element() {
            var result = (await AssertionUtilities.DivisionAsync(20, 2).ToEnumerable()).ToArray();
            Assert.Single(result);
            Assert.Collection(result, x => Assert.Equal(10, x));
        }
    }
}