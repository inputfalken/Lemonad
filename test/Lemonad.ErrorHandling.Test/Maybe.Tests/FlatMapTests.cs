using Xunit;

namespace Lemonad.ErrorHandling.Test.Maybe.Tests {
    public class FlatMapTests {
        [Fact]
        public void
            Flattening_From_String_Maybe_With_value_To_String_Maybe_With_Value__Expects_String_Maybe_With_Value() {
            const string input = "hello";
            var lengthMaybe = input.SomeWhen(s => s.Length > 4);
            var stringMaybe = input.NoneWhen(string.IsNullOrEmpty);

            var flatMappedMaybe = lengthMaybe.FlatMap(stringMaybe);

            Assert.True(lengthMaybe.HasValue, "Maybe should have value.");
            Assert.True(stringMaybe.HasValue, "Maybe should have value.");
            Assert.True(flatMappedMaybe.HasValue,
                "Maybe should have a value since both maybes will pass with the input supplied.");
            Assert.Equal(input, flatMappedMaybe.Value);
        }

        [Fact]
        public void
            ResultSelector_Overload__Flattening_From_String_Maybe_With_value_To_String_Maybe_With_Value__Expects_String_Maybe_With_Value() {
            const string input = "hello";
            var lengthMaybe = input.SomeWhen(s => s.Length > 4);
            var stringMaybe = input.NoneWhen(string.IsNullOrEmpty);

            var flatMappedMaybe = lengthMaybe.FlatMap(stringMaybe, (s, s1) => s.Length + s1.Length);

            Assert.True(lengthMaybe.HasValue, "Maybe should have value.");
            Assert.True(stringMaybe.HasValue, "Maybe should have value.");
            Assert.True(flatMappedMaybe.HasValue,
                "Maybe should have a value since both maybes will pass with the input supplied.");
            Assert.Equal(input.Length * 2, flatMappedMaybe.Value);
        }

        [Fact]
        public void
            Flattening_From_String_Maybe_With_value_To_String_Maybe_Without_Value__Expects_String_Maybe_Without_Value() {
            const string input = "hello";
            var lengthMaybe = input.SomeWhen(s => s.Length > 5);
            var stringMaybe = input.NoneWhen(string.IsNullOrEmpty);

            var flatMappedMaybe = stringMaybe.FlatMap(lengthMaybe);

            Assert.True(stringMaybe.HasValue, "Maybe should have value.");
            Assert.False(lengthMaybe.HasValue, "Maybe should not have value");
            Assert.False(flatMappedMaybe.HasValue,
                "Maybe should not have a value since one of the maybes will not pass.");
            Assert.Equal(default(string), flatMappedMaybe.Value);
        }

        [Fact]
        public void
            ResultSelector_Overload__Flattening_From_String_Maybe_With_value_To_String_Maybe_Without_Value__Expects_String_Maybe_Without_Value() {
            const string input = "hello";
            var lengthMaybe = input.SomeWhen(s => s.Length > 5);
            var stringMaybe = input.NoneWhen(string.IsNullOrEmpty);

            var flatMappedMaybe = stringMaybe.FlatMap(lengthMaybe, (x, y) => x.Length + y.Length);

            Assert.True(stringMaybe.HasValue, "Maybe should have value.");
            Assert.False(lengthMaybe.HasValue, "Maybe should not have value");
            Assert.False(flatMappedMaybe.HasValue,
                "Maybe should not have a value since one of the maybes will not pass.");
            Assert.Equal(default(int), flatMappedMaybe.Value);
        }

        [Fact]
        public void
            ResultSelector_Overload__Flattening_From_String_Maybe_Without_value_To_String_Maybe_With_Value__Expects_String_Maybe_Without_Value() {
            const string input = "hello";
            var lengthMaybe = input.SomeWhen(s => s.Length > 5);
            var stringMaybe = input.NoneWhen(string.IsNullOrEmpty);
            var flatMappedMaybe = lengthMaybe.FlatMap(stringMaybe, (x, y) => x.Length + y.Length);

            Assert.True(stringMaybe.HasValue, "Maybe should have value");
            Assert.False(lengthMaybe.HasValue, "Maybe should not have value");
            Assert.False(flatMappedMaybe.HasValue,
                "Maybe should not have a value since one of the maybes will not pass.");
            Assert.Equal(default(int), flatMappedMaybe.Value);
        }

        [Fact]
        public void
            ResultSelector_Overload__Flattening_From_String_Maybe_Without_value_To_String_Maybe_Without_Value__Expects_String_Maybe_Without_Value() {
            const string input = "hello";
            var lengthMaybe = input.SomeWhen(s => s.Length > 5);
            var stringMaybe = input.SomeWhen(string.IsNullOrEmpty);

            var flatMappedMaybe = stringMaybe.FlatMap(lengthMaybe, (s, s1) => s.Length + s1.Length);

            Assert.False(stringMaybe.HasValue, "Maybe should have value");
            Assert.False(lengthMaybe.HasValue, "Maybe should not have value");
            Assert.False(flatMappedMaybe.HasValue,
                "Maybe should not have a value since both of the maybes will not pass.");
            Assert.Equal(default(int), flatMappedMaybe.Value);
        }

        [Fact]
        public void
            Flattening_From_String_Maybe_With_value_To_Nullable_Int_With_Value__Expects_String_Maybe_With_Value() {
            const string input = "hello";
            var lengthMaybe = input.SomeWhen(s => s.Length > 4);
            int? nullabelInt = 2;

            var flatMappedMaybe = lengthMaybe.FlatMap(nullabelInt);

            Assert.True(lengthMaybe.HasValue, "Maybe should have value.");
            Assert.True(nullabelInt.HasValue, "Maybe should have value.");
            Assert.True(flatMappedMaybe.HasValue,
                "Maybe should have a value since both maybes will pass with the input supplied.");
            Assert.Equal(nullabelInt.Value, flatMappedMaybe.Value);
        }

        [Fact]
        public void
            ResultSelector_Overload__Flattening_From_String_Maybe_With_value_To_Nullable_Int_Maybe_With_Value__Expects_String_Maybe_With_Value() {
            const string input = "hello";
            var lengthMaybe = input.SomeWhen(s => s.Length > 4);
            int? nullabelInt = 2;

            var flatMappedMaybe = lengthMaybe.FlatMap(nullabelInt, (s, s1) => s.Length + s1);

            Assert.True(lengthMaybe.HasValue, "Maybe should have value.");
            Assert.True(nullabelInt.HasValue, "Maybe should have value.");
            Assert.True(flatMappedMaybe.HasValue,
                "Maybe should have a value since both maybes will pass with the input supplied.");
            Assert.Equal(input.Length + nullabelInt, flatMappedMaybe.Value);
        }

        [Fact]
        public void
            Flattening_From_String_Maybe_With_value_To_Nullable_Int_Without_Value__Expects_String_Maybe_Without_Value() {
            const string input = "hello";
            int? nullableInt = null;
            var stringMaybe = input.NoneWhen(string.IsNullOrEmpty);

            var flatMappedMaybe = stringMaybe.FlatMap(nullableInt);

            Assert.True(stringMaybe.HasValue, "Maybe should have value.");
            Assert.False(nullableInt.HasValue, "Maybe should not have value");
            Assert.False(flatMappedMaybe.HasValue,
                "Maybe should not have a value since one of the maybes will not pass.");
            Assert.Equal(default(int), flatMappedMaybe.Value);
        }

        [Fact]
        public void
            ResultSelector_Overload__Flattening_From_String_Maybe_With_value_To_Nullable_Int_Without_Value__Expects_String_Maybe_Without_Value() {
            const string input = "hello";
            int? nullableInt = null;
            var stringMaybe = input.NoneWhen(string.IsNullOrEmpty);

            var flatMappedMaybe = stringMaybe.FlatMap(nullableInt, (s, s1) => s.Length + s1);

            Assert.True(stringMaybe.HasValue, "Maybe should have value.");
            Assert.False(nullableInt.HasValue, "Maybe should not have value");
            Assert.False(flatMappedMaybe.HasValue,
                "Maybe should not have a value since one of the maybes will not pass.");
            Assert.Equal(default(int), flatMappedMaybe.Value);
        }

        [Fact]
        public void
            ResultSelector_Overload__Flattening_From_String_Maybe_Without_value_To_Nullable_Int_With_Value__Expects_String_Maybe_Without_Value() {
            const string input = "hello";
            var lengthMaybe = input.SomeWhen(s => s.Length > 5);
            int? nullableInt = 2;
            var flatMappedMaybe = lengthMaybe.FlatMap(nullableInt, (s, s1) => s.Length + s1);

            Assert.True(nullableInt.HasValue, "Maybe should have value");
            Assert.False(lengthMaybe.HasValue, "Maybe should not have value");
            Assert.False(flatMappedMaybe.HasValue,
                "Maybe should not have a value since one of the maybes will not pass.");
            Assert.Equal(default(int), flatMappedMaybe.Value);
        }

        [Fact]
        public void
            ResultSelector_Overload__Flattening_From_String_Maybe_Without_value_To_Nullable_int_Without_Value__Expects_String_Maybe_Without_Value() {
            const string input = "hello";
            int? nullableInt = null;
            var stringMaybe = input.SomeWhen(string.IsNullOrEmpty);

            var flatMappedMaybe = stringMaybe.FlatMap(nullableInt, (s, s1) => s.Length + s1);

            Assert.False(stringMaybe.HasValue, "Maybe should have value");
            Assert.False(nullableInt.HasValue, "Maybe should not have value");
            Assert.False(flatMappedMaybe.HasValue,
                "Maybe should not have a value since both of the maybes will not pass.");
            Assert.Equal(default(int), flatMappedMaybe.Value);
        }
    }
}