using System;
using Lemonad.ErrorHandling.Extensions;
using Lemonad.ErrorHandling.Extensions.Maybe;
using Xunit;

namespace Lemonad.ErrorHandling.Unit.Maybe.Tests {
    public class FlatMapTests {
        [Fact]
        public void
            Flattening_From_String_Maybe_With_value_To_Nullable_Int_With_Value__Expects_String_Maybe_With_Value() {
            const string input = "hello";
            var lengthMaybe = input.ToMaybe(s => s.Length > 4);
            int? nullabelInt = 2;

            var flatMappedMaybe = lengthMaybe.FlatMap(x => nullabelInt);

            Assert.True(lengthMaybe.HasValue, "Maybe should have value.");
            Assert.True(nullabelInt.HasValue, "Maybe should have value.");
            Assert.True(flatMappedMaybe.HasValue,
                "Maybe should have a value since both maybes will pass with the input supplied.");
            Assert.Equal(nullabelInt.Value, flatMappedMaybe.Value);
        }

        [Fact]
        public void
            Flattening_From_String_Maybe_With_value_To_Nullable_Int_Without_Value__Expects_String_Maybe_Without_Value() {
            const string input = "hello";
            int? nullableInt = null;
            var stringMaybe = input.ToMaybeNone(string.IsNullOrEmpty);

            var flatMappedMaybe = stringMaybe.FlatMap(x => nullableInt);

            Assert.True(stringMaybe.HasValue, "Maybe should have value.");
            Assert.False(nullableInt.HasValue, "Maybe should not have value");
            Assert.False(flatMappedMaybe.HasValue,
                "Maybe should not have a value since one of the maybes will not pass.");
            Assert.Equal(default, flatMappedMaybe.Value);
        }

        [Fact]
        public void
            Flattening_From_String_Maybe_With_value_To_String_Maybe_With_Value__Expects_String_Maybe_With_Value() {
            const string input = "hello";
            var lengthMaybe = input.ToMaybe(s => s.Length > 4);
            var stringMaybe = input.ToMaybeNone(string.IsNullOrEmpty);

            var flatMappedMaybe = lengthMaybe.FlatMap(_ => stringMaybe);

            Assert.True(lengthMaybe.HasValue, "Maybe should have value.");
            Assert.True(stringMaybe.HasValue, "Maybe should have value.");
            Assert.True(flatMappedMaybe.HasValue,
                "Maybe should have a value since both maybes will pass with the input supplied.");
            Assert.Equal(input, flatMappedMaybe.Value);
        }

        [Fact]
        public void
            Flattening_From_String_Maybe_With_value_To_String_Maybe_Without_Value__Expects_String_Maybe_Without_Value() {
            const string input = "hello";
            var lengthMaybe = input.ToMaybe(s => s.Length > 5);
            var stringMaybe = input.ToMaybeNone(string.IsNullOrEmpty);

            var flatMappedMaybe = stringMaybe.FlatMap(_ => lengthMaybe);

            Assert.True(stringMaybe.HasValue, "Maybe should have value.");
            Assert.False(lengthMaybe.HasValue, "Maybe should not have value");
            Assert.False(flatMappedMaybe.HasValue,
                "Maybe should not have a value since one of the maybes will not pass.");
            Assert.Equal(default, flatMappedMaybe.Value);
        }

        [Fact]
        public void
            Maybe_String_Whose_Property_HasValue_Is_True__Pasing_Null_Selector__ArgumentNullReferenceException_Thrown() {
            Assert.Throws<ArgumentNullException>(() => {
                Func<string, IMaybe<string>> selector = null;
                ErrorHandling.Maybe.Value("foo").FlatMap(selector);
            });
        }

        [Fact]
        public void
            Maybe_String_Whose_Property_HasValue_Is_True__Pasing_Null_Selector_And_ResultSelector_ArgumentNullReferenceException_Thrown() {
            Assert.Throws<ArgumentNullException>(() => {
                Func<string, IMaybe<string>> selector = null;
                Func<string, string, string> resultSelector = null;
                ErrorHandling.Maybe.Value("foo").FlatMap(selector, resultSelector);
            });
        }

        [Fact]
        public void
            Maybe_String_Whose_Property_HasValue_Is_True_Flatmapping_Some__Pasing_Null_ResultSelector__ArgumentNullReferenceException_Thrown() {
            Assert.Throws<ArgumentNullException>(() => {
                Func<string, string, string> resultSelector = null;
                ErrorHandling.Maybe.Value("foo").FlatMap(ErrorHandling.Maybe.Value, resultSelector);
            });
        }

        [Fact]
        public void Passing_Null_Function__Throws_ArgumentNullException() {
            Assert.Throws<ArgumentNullException>(() => {
                Func<string, IMaybe<bool>> function = null;
                ErrorHandling.Maybe.Value("foo").FlatMap(function);
            });
        }

        [Fact]
        public void
            ResultSelector_Overload__Flattening_From_String_Maybe_With_value_To_Nullable_Int_Maybe_With_Value__Expects_String_Maybe_With_Value() {
            const string input = "hello";
            var lengthMaybe = input.ToMaybe(s => s.Length > 4);
            int? nullabelInt = 2;

            var flatMappedMaybe = lengthMaybe.FlatMap(x => nullabelInt, (s, s1) => s.Length + s1);

            Assert.True(lengthMaybe.HasValue, "Maybe should have value.");
            Assert.True(nullabelInt.HasValue, "Maybe should have value.");
            Assert.True(flatMappedMaybe.HasValue,
                "Maybe should have a value since both maybes will pass with the input supplied.");
            Assert.Equal(input.Length + nullabelInt, flatMappedMaybe.Value);
        }

        [Fact]
        public void
            ResultSelector_Overload__Flattening_From_String_Maybe_With_value_To_Nullable_Int_Without_Value__Expects_String_Maybe_Without_Value() {
            const string input = "hello";
            int? nullableInt = null;
            var stringMaybe = input.ToMaybeNone(string.IsNullOrEmpty);

            var flatMappedMaybe = stringMaybe.FlatMap(x => nullableInt, (s, s1) => s.Length + s1);

            Assert.True(stringMaybe.HasValue, "Maybe should have value.");
            Assert.False(nullableInt.HasValue, "Maybe should not have value");
            Assert.False(flatMappedMaybe.HasValue,
                "Maybe should not have a value since one of the maybes will not pass.");
            Assert.Equal(default, flatMappedMaybe.Value);
        }

        [Fact]
        public void
            ResultSelector_Overload__Flattening_From_String_Maybe_With_value_To_String_Maybe_With_Value__Expects_String_Maybe_With_Value() {
            const string input = "hello";
            var lengthMaybe = input.ToMaybe(s => s.Length > 4);
            var stringMaybe = input.ToMaybeNone(string.IsNullOrEmpty);

            var flatMappedMaybe = lengthMaybe.FlatMap(x => stringMaybe, (s, s1) => s.Length + s1.Length);

            Assert.True(lengthMaybe.HasValue, "Maybe should have value.");
            Assert.True(stringMaybe.HasValue, "Maybe should have value.");
            Assert.True(flatMappedMaybe.HasValue,
                "Maybe should have a value since both maybes will pass with the input supplied.");
            Assert.Equal(input.Length * 2, flatMappedMaybe.Value);
        }

        [Fact]
        public void
            ResultSelector_Overload__Flattening_From_String_Maybe_With_value_To_String_Maybe_Without_Value__Expects_String_Maybe_Without_Value() {
            const string input = "hello";
            var lengthMaybe = input.ToMaybe(s => s.Length > 5);
            var stringMaybe = input.ToMaybeNone(string.IsNullOrEmpty);

            var flatMappedMaybe = stringMaybe.FlatMap(x => lengthMaybe, (x, y) => x.Length + y.Length);

            Assert.True(stringMaybe.HasValue, "Maybe should have value.");
            Assert.False(lengthMaybe.HasValue, "Maybe should not have value");
            Assert.False(flatMappedMaybe.HasValue,
                "Maybe should not have a value since one of the maybes will not pass.");
            Assert.Equal(default, flatMappedMaybe.Value);
        }

        [Fact]
        public void
            ResultSelector_Overload__Flattening_From_String_Maybe_Without_value_To_Nullable_Int_With_Value__Expects_String_Maybe_Without_Value() {
            const string input = "hello";
            var lengthMaybe = input.ToMaybe(s => s.Length > 5);
            int? nullableInt = 2;
            var flatMappedMaybe = lengthMaybe.FlatMap(x => nullableInt, (s, s1) => s.Length + s1);

            Assert.True(nullableInt.HasValue, "Maybe should have value");
            Assert.False(lengthMaybe.HasValue, "Maybe should not have value");
            Assert.False(flatMappedMaybe.HasValue,
                "Maybe should not have a value since one of the maybes will not pass.");
            Assert.Equal(default, flatMappedMaybe.Value);
        }

        [Fact]
        public void
            ResultSelector_Overload__Flattening_From_String_Maybe_Without_value_To_Nullable_int_Without_Value__Expects_String_Maybe_Without_Value() {
            const string input = "hello";
            int? nullableInt = null;
            var stringMaybe = input.ToMaybe(string.IsNullOrEmpty);

            var flatMappedMaybe = stringMaybe.FlatMap(x => nullableInt, (s, s1) => s.Length + s1);

            Assert.False(stringMaybe.HasValue, "Maybe should have value");
            Assert.False(nullableInt.HasValue, "Maybe should not have value");
            Assert.False(flatMappedMaybe.HasValue,
                "Maybe should not have a value since both of the maybes will not pass.");
            Assert.Equal(default, flatMappedMaybe.Value);
        }

        [Fact]
        public void
            ResultSelector_Overload__Flattening_From_String_Maybe_Without_value_To_String_Maybe_With_Value__Expects_String_Maybe_Without_Value() {
            const string input = "hello";
            var lengthMaybe = input.ToMaybe(s => s.Length > 5);
            var stringMaybe = input.ToMaybeNone(string.IsNullOrEmpty);
            var flatMappedMaybe = lengthMaybe.FlatMap(x => stringMaybe, (x, y) => x.Length + y.Length);

            Assert.True(stringMaybe.HasValue, "Maybe should have value");
            Assert.False(lengthMaybe.HasValue, "Maybe should not have value");
            Assert.False(flatMappedMaybe.HasValue,
                "Maybe should not have a value since one of the maybes will not pass.");
            Assert.Equal(default, flatMappedMaybe.Value);
        }

        [Fact]
        public void
            ResultSelector_Overload__Flattening_From_String_Maybe_Without_value_To_String_Maybe_Without_Value__Expects_String_Maybe_Without_Value() {
            const string input = "hello";
            var lengthMaybe = input.ToMaybe(s => s.Length > 5);
            var stringMaybe = input.ToMaybe(string.IsNullOrEmpty);

            var flatMappedMaybe = stringMaybe.FlatMap(x => lengthMaybe, (s, s1) => s.Length + s1.Length);

            Assert.False(stringMaybe.HasValue, "Maybe should have value");
            Assert.False(lengthMaybe.HasValue, "Maybe should not have value");
            Assert.False(flatMappedMaybe.HasValue,
                "Maybe should not have a value since both of the maybes will not pass.");
            Assert.Equal(default, flatMappedMaybe.Value);
        }
    }
}