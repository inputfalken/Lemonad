using Assertion;
using Lemonad.ErrorHandling.Extensions;
using Xunit;

namespace Lemonad.ErrorHandling.Unit.Maybe.Tests {
    public class FlattenTests {
        [Fact]
        public void
            Flattening_From_String_Maybe_With_No_Value_To_Maybe_Int_With_No_Value__Expects_String_Maybe_With_Value() {
            const string input = "hello";
            input.ToMaybeNone().AssertNone().Flatten(x => 2.ToMaybeNone().AssertNone()).AssertNone();
        }

        [Fact]
        public void
            Flattening_From_String_Maybe_With_No_Value_To_Maybe_Int_With_Value__Expects_String_Maybe_With_Value() {
            const string input = "hello";

            input.ToMaybeNone().AssertNone().Flatten(x => ErrorHandling.Maybe.Value(2).AssertValue(2)).AssertNone();
        }

        [Fact]
        public void
            Flattening_From_String_Maybe_With_Value_To_Maybe_Int_With_No_Value__Expects_String_Maybe_With_Value() {
            const string input = "hello";

            input.ToMaybe(s => s.Length > 4)
                .AssertValue("hello")
                .FlatMap(x => 2.ToMaybeNone().AssertNone())
                .AssertNone();
        }

        [Fact]
        public void
            Flattening_From_String_Maybe_With_Value_To_Maybe_Int_With_Value__Expects_String_Maybe_With_Value() {
            const string input = "hello";
            input.ToMaybe(s => s.Length > 4)
                .AssertValue("hello")
                .Flatten(x => ErrorHandling.Maybe.Value(2).AssertValue(2))
                .AssertValue("hello");
        }
    }
}