using System;
using System.Threading.Tasks;
using Lemonad.ErrorHandling.Extensions.AsyncMaybe;
using Xunit;

namespace Lemonad.ErrorHandling.Unit.AsyncMaybe.Tests {
    public class AwaiterTests {
        [Fact]
        public Task Awaiting_Null_Throws() {
            IAsyncMaybe<string> maybe = null;
            return Assert.ThrowsAsync<ArgumentNullException>(async () => { await maybe; });
        }
    }
}