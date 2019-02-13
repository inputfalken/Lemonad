using System;
using System.Threading.Tasks;
using Assertion;
using Lemonad.ErrorHandling.Extensions.AsyncMaybe;
using Xunit;

namespace Lemonad.ErrorHandling.Unit.Maybe.Tests {
    public class FlattenAsyncTests {
        [Fact]
        public async Task
            Flattening_From_String_Maybe_With_No_Value_To_Maybe_Int_With_No_Value__Expects_String_Maybe_With_Value() {
            await ErrorHandling.Maybe.None<string>()
                .FlattenAsync(x => Lemonad.ErrorHandling.AsyncMaybe.None<int>()).AssertNone();
        }

        [Fact]
        public async Task
            Flattening_From_String_Maybe_With_No_Value_To_Maybe_Int_With_Value__Expects_String_Maybe_With_Value() {
            await ErrorHandling.Maybe.None<string>()
                .FlattenAsync(x => Lemonad.ErrorHandling.AsyncMaybe.Value(2)).AssertNone();
        }

        [Fact]
        public async Task
            Flattening_From_String_Maybe_With_Value_To_Maybe_Int_With_No_Value__Expects_String_Maybe_With_Value() {
            await ErrorHandling.Maybe.Value("hello")
                .FlattenAsync(x => Lemonad.ErrorHandling.AsyncMaybe.None<int>())
                .AssertNone();
        }

        [Fact]
        public async Task
            Flattening_From_String_Maybe_With_Value_To_Maybe_Int_With_Value__Expects_String_Maybe_With_Value() {
            const string input = "hello";
            await ErrorHandling.Maybe.Value(input)
                .FlattenAsync(x => Lemonad.ErrorHandling.AsyncMaybe.Value(2))
                .AssertValue(input);
        }

        [Fact]
        public void Passing_Null_Selector_Throws() =>
            Assert.Throws<ArgumentNullException>(AssertionUtilities.SelectorName,
                () => ErrorHandling.Maybe.Value(2).FlattenAsync((Func<int, IAsyncMaybe<int>>) null));
    }
}