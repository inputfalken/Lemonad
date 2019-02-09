using System;
using System.Threading.Tasks;
using Assertion;
using Lemonad.ErrorHandling.Extensions;
using Lemonad.ErrorHandling.Extensions.AsyncMaybe;
using Xunit;

namespace Lemonad.ErrorHandling.Unit.Maybe.Tests {
    public class FlattenAsyncTests {
        [Fact]
        public async Task
            Flattening_From_String_Maybe_With_No_Value_To_Maybe_Int_With_No_Value__Expects_String_Maybe_With_Value() {
            const string input = "hello";
            await input.ToMaybeNone().AssertNone().FlattenAsync(x => Lemonad.ErrorHandling.AsyncMaybe.None<int>().AssertNone()).AssertNone();
        }

        [Fact]
        public async Task
            Flattening_From_String_Maybe_With_No_Value_To_Maybe_Int_With_Value__Expects_String_Maybe_With_Value() {
            const string input = "hello";

            await input.ToMaybeNone().AssertNone().FlattenAsync(x => Lemonad.ErrorHandling.AsyncMaybe.Value(2).AssertValue(2)).AssertNone();
        }

        [Fact]
        public async Task
            Flattening_From_String_Maybe_With_Value_To_Maybe_Int_With_No_Value__Expects_String_Maybe_With_Value() {
            const string input = "hello";

            await input.ToMaybe(s => s.Length > 4)
                .AssertValue("hello")
                .FlattenAsync(x => Lemonad.ErrorHandling.AsyncMaybe.None<int>().AssertNone())
                .AssertNone();
        }

        [Fact]
        public async Task
            Flattening_From_String_Maybe_With_Value_To_Maybe_Int_With_Value__Expects_String_Maybe_With_Value() {
            const string input = "hello";
            await input.ToMaybe(s => s.Length > 4)
                .AssertValue("hello")
                .FlattenAsync(x => Lemonad.ErrorHandling.AsyncMaybe.Value(2).AssertValue(2))
                .AssertValue("hello");
        }

        [Fact]
        public void Passing_Null_Selector_Throws() =>
            Assert.Throws<ArgumentNullException>(AssertionUtilities.SelectorName,
                () => ErrorHandling.Maybe.Value(2).FlattenAsync((Func<int, IAsyncMaybe<int>>) null));
    }
}