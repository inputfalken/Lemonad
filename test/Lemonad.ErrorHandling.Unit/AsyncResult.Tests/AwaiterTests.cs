using System;
using System.Threading.Tasks;
using Lemonad.ErrorHandling.Extensions.AsyncResult;
using Xunit;

namespace Lemonad.ErrorHandling.Unit.AsyncResult.Tests {
    public class AwaiterTests {
        [Fact]
        public Task Awaiting_Null_Throws() {
            IAsyncResult<string, int> result = null;
            return Assert.ThrowsAsync<ArgumentNullException>(async () => { await result; });
        }
    }
}