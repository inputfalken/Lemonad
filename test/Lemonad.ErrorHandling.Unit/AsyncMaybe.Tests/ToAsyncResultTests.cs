using System;
using System.Threading.Tasks;
using Assertion;
using Lemonad.ErrorHandling.Extensions.AsyncMaybe;
using Lemonad.ErrorHandling.Extensions.AsyncResult;
using Xunit;

namespace Lemonad.ErrorHandling.Unit.AsyncMaybe.Tests {
    public class ToAsyncResultTests {
        [Fact]
        public void Null_Source_Throws() {
            Assert.Throws<ArgumentNullException>(AssertionUtilities.ExtensionParameterName, () => ((IAsyncResult<string, int>) null).ToAsyncMaybe());
        }
        [Fact]
        public async Task Result_With_Value() {
            await ErrorHandling.AsyncResult.Value<string, int>("foobar").ToAsyncMaybe().AssertValue("foobar");
        }
        
        [Fact]
        public async Task Result_With_Error() {
            await ErrorHandling.AsyncResult.Error<int, string>("ERROR").ToAsyncMaybe().AssertNone();
        }
    }
}