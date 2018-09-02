using Lemonad.ErrorHandling.Extensions;
using Xunit;

namespace Lemonad.ErrorHandling.Test.Maybe.Tests {
    public class FlattenTests {
        [Fact]
        public void
            Flattening_From_String_Maybe_With_Value_To_Maybe_Int_With_Value__Expects_String_Maybe_With_Value() {
            const string input = "hello";
            var lengthMaybe = input.ToMaybe(s => s.Length > 4);
            var maybe2 = 2.ToMaybe();

            var flatMappedMaybe = lengthMaybe.Flatten(x => maybe2);

            Assert.True(lengthMaybe.HasValue, "Maybe should have value.");
            Assert.True(maybe2.HasValue, "Maybe should have value.");
            Assert.True(flatMappedMaybe.HasValue,
                "Maybe should have a value since both maybes will pass with the input supplied.");
            Assert.Equal("hello", flatMappedMaybe.Value);
        }

        [Fact]
        public void
            Flattening_From_String_Maybe_With_No_Value_To_Maybe_Int_With_Value__Expects_String_Maybe_With_Value() {
            const string input = "hello";
            var lengthMaybe = input.ToMaybeNone();
            var maybe2 = 2.ToMaybe();

            var flatMappedMaybe = lengthMaybe.Flatten(x => maybe2);

            Assert.False(lengthMaybe.HasValue, "Maybe should not have value.");
            Assert.True(maybe2.HasValue, "Maybe should have value.");
            Assert.False(flatMappedMaybe.HasValue, "Flattened maybe should not have a value.");
            Assert.Equal(default, flatMappedMaybe.Value);
        }

        [Fact]
        public void
            Flattening_From_String_Maybe_With_Value_To_Maybe_Int_With_No_Value__Expects_String_Maybe_With_Value() {
            const string input = "hello";
            var lengthMaybe = input.ToMaybe(s => s.Length > 4);
            var maybe2 = 2.ToMaybeNone();

            var flatMappedMaybe = lengthMaybe.Flatten(x => maybe2);

            Assert.True(lengthMaybe.HasValue, "Maybe should have value.");
            Assert.False(maybe2.HasValue, "Maybe should not have value.");
            Assert.False(flatMappedMaybe.HasValue, "Flattened maybe should not have a value.");
            Assert.Equal(default, flatMappedMaybe.Value);
        }

        [Fact]
        public void
            Flattening_From_String_Maybe_With_No_Value_To_Maybe_Int_With_No_Value__Expects_String_Maybe_With_Value() {
            const string input = "hello";
            var lengthMaybe = input.ToMaybeNone();
            var maybe2 = 2.ToMaybeNone();

            var flatMappedMaybe = lengthMaybe.Flatten(x => maybe2);

            Assert.False(lengthMaybe.HasValue, "Maybe should not have value.");
            Assert.False(maybe2.HasValue, "Maybe should not have value.");
            Assert.False(flatMappedMaybe.HasValue, "Flattened maybe should not have a value.");
            Assert.Equal(default, flatMappedMaybe.Value);
        }
    }
}