using System.Threading.Tasks;
using Lemonad.ErrorHandling.Exceptions;
using Xunit;

namespace Lemonad.ErrorHandling.Unit.AsyncMaybe.Tests {
    public class Value {
        [Fact]
        public void Accessing_Value_Before_Await_Throws_InvalidEitherStateException() {
            Assert.Throws<InvalidMaybeStateException>(() => ErrorHandling.AsyncMaybe.Value("foo").Value);
        }
        
        [Fact]
        public async Task Await_Then_Accessing_Value_Does_Not_Throw() {
            var asyncMaybe = ErrorHandling.AsyncMaybe.Value("foo");
            await asyncMaybe.HasValue;
            Assert.Null(Record.Exception(() => asyncMaybe.Value));
        }
    }
}