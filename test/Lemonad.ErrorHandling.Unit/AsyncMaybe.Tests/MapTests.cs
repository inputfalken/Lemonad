using System;
using System.Threading.Tasks;
using Assertion;
using Lemonad.ErrorHandling.Extensions.AsyncMaybe;
using Xunit;

namespace Lemonad.ErrorHandling.Unit.AsyncMaybe.Tests {
    public class MapTests {
        [Fact]
        public async Task Mapping_Integer_With_Multiplication() {
            await ErrorHandling.AsyncMaybe.Value(20).Map(s => s * 2).AssertValue(40);
        }

        [Fact]
        public async Task Mapping_String_Length() {
            await ErrorHandling.AsyncMaybe.Value("hello").Map(s => s.Length).AssertValue(5);
        }

        [Fact]
        public void
            Maybe_String_Whose_Property_HasValue_Is_True__Pasing_Null_Predicate__ArgumentNullReferenceException_Thrown() {
            Assert.Throws<ArgumentNullException>(() => {
                Func<string, bool> function = null;
                ErrorHandling.AsyncMaybe.Value("foo").Map(function);
            });
        }
    }
}