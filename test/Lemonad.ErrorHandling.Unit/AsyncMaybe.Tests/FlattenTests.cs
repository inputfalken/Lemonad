using System;
using System.Threading.Tasks;
using Assertion;
using Lemonad.ErrorHandling.Extensions;
using Lemonad.ErrorHandling.Extensions.AsyncMaybe;
using Xunit;

namespace Lemonad.ErrorHandling.Unit.AsyncMaybe.Tests {
    public class FlattenTests {
        [Fact]
        public async Task
            Flattening_From_String_Maybe_With_No_Value_To_Maybe_Int_With_No_Value__Expects_String_Maybe_With_Value() {
            await ErrorHandling.AsyncMaybe.None<string>().AssertNone().Flatten(x => ErrorHandling.Maybe.None<int>())
                .AssertNone();
        }

        [Fact]
        public async Task
            Flattening_From_String_Maybe_With_No_Value_To_Maybe_Int_With_Value__Expects_String_Maybe_With_Value() {
            await ErrorHandling.AsyncMaybe.None<string>().Flatten(x => ErrorHandling.Maybe.Value(2))
                .AssertNone();
        }

        [Fact]
        public async Task
            Flattening_From_String_Maybe_With_Value_To_Maybe_Int_With_No_Value__Expects_String_Maybe_With_Value() {
            await ErrorHandling.AsyncMaybe.Value("hello")
                .Flatten(x => ErrorHandling.Maybe.None<int>())
                .AssertNone();
        }

        [Fact]
        public async Task
            Flattening_From_String_Maybe_With_Value_To_Maybe_Int_With_Value__Expects_String_Maybe_With_Value() {
            const string input = "hello";
            await ErrorHandling.AsyncMaybe.Value(input)
                .Flatten(x => ErrorHandling.Maybe.Value(2))
                .AssertValue(input);
        }

        [Fact]
        public void Passing_Null_Selector_Throws() =>
            Assert.Throws<ArgumentNullException>(AssertionUtilities.SelectorName,
                () => ErrorHandling.AsyncMaybe.Value(2).Flatten((Func<int, IMaybe<int>>) null));
    }
}