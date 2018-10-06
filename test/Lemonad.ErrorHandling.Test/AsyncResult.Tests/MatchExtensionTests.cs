using System.Threading.Tasks;
using Xunit;
using static Lemonad.ErrorHandling.Test.AssertionUtilities;

namespace Lemonad.ErrorHandling.Test.AsyncResult.Tests {
    public class MatchExtensionTests {
        [Fact]
        public async Task Result_With_Error() {
            var result = await DivisionAsync(10, 0).MapError(x => -1d).Match();
            Assert.Equal(-1, result);
        }

        [Fact]
        public async Task Result_With_Error_With_Selector() {
            var result = await DivisionAsync(10, 0).MapError(x => -1d).Match(d => d * 2);
            Assert.Equal(-2, result);
        }

        [Fact]
        public async Task Result_With_Value() {
            var result = await DivisionAsync(10, 2).MapError(x => -1d).Match();
            Assert.Equal(5, result);
        }

        [Fact]
        public async Task Result_With_Value_With_Selector() {
            var result = await DivisionAsync(10, 2).MapError(x => -1d).Match(d => d * 2);
            Assert.Equal(10, result);
        }
    }
}