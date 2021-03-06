using System;
using System.Threading.Tasks;
using Assertion;
using Lemonad.ErrorHandling.Extensions.AsyncMaybe;
using Xunit;

namespace Lemonad.ErrorHandling.Unit.AsyncMaybe.Tests {
    public class MapAsyncTests {
        [Fact]
        public async Task Mapping_Integer_With_Multiplication() {
            await ErrorHandling.AsyncMaybe.Value(20)
                .MapAsync(async x => {
                    await AssertionUtilities.Delay;
                    return x * 2;
                })
                .AssertValue(40);
        }

        [Fact]
        public async Task Mapping_String_Length() {
            await ErrorHandling.AsyncMaybe.Value("hello")
                .MapAsync(async s => {
                    await AssertionUtilities.Delay;
                    return s.Length;
                })
                .AssertValue(5);
        }

        [Fact]
        public async Task
            Maybe_String_Whose_Property_HasValue_Is_True__Pasing_Null_Predicate__ArgumentNullReferenceException_Thrown() {
            await Assert.ThrowsAsync<ArgumentNullException>(async () => {
                Func<string, Task<bool>> function = null;
                await ErrorHandling.AsyncMaybe.Value("foo").MapAsync(function);
            });
        }
    }
}