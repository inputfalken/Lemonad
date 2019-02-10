using System;
using Assertion;
using Lemonad.ErrorHandling.Extensions;
using Xunit;

namespace Lemonad.ErrorHandling.Unit.Maybe.Tests {
    public class FlatMapResultSelectorTests {
        [Fact]
        public void
            Flattening_From_String_Maybe_With_value_To_Nullable_Int_With_Value__Expects_String_Maybe_With_Value() {
            const string input = "hello";
            int? nullabelInt = 2;
            ErrorHandling.Maybe.Value(input)
                .FlatMap(x => nullabelInt)
                .AssertValue(nullabelInt.Value);
        }

        [Fact]
        public void
            Flattening_From_String_Maybe_With_value_To_Nullable_Int_Without_Value__Expects_String_Maybe_Without_Value() {
            const string input = "hello";
            int? nullableInt = null;
            ErrorHandling.Maybe.Value(input)
                .FlatMap(x => nullableInt)
                .AssertNone();
        }

        [Fact]
        public void
            Flattening_From_String_Maybe_With_value_To_String_Maybe_With_Value__Expects_String_Maybe_With_Value() {
            const string input = "hello";
            ErrorHandling.Maybe.Value(input)
                .FlatMap(x => input.ToMaybe(s => s.Length > 4))
                .AssertValue(input);
        }

        [Fact]
        public void
            Flattening_From_String_Maybe_With_value_To_String_Maybe_Without_Value__Expects_String_Maybe_Without_Value() {
            const string input = "hello";
            ErrorHandling.Maybe.Value(input)
                .FlatMap(x => input.ToMaybe(s => s.Length > 5))
                .AssertNone();
        }

        [Fact]
        public void Passing_Both_Null_ResultSelector_Function_And_SelectorFunction__Throws_ArgumentNullException() {
            Assert.Throws<ArgumentNullException>(() => {
                Func<string, string, string> function = null;
                ErrorHandling.Maybe.Value("foo").FlatMap(ErrorHandling.Maybe.Value, function);
            });
        }

        [Fact]
        public void Passing_Null_ResultSelector_Function__Throws_ArgumentNullException() {
            Assert.Throws<ArgumentNullException>(() => {
                Func<string, string, string> function = null;
                ErrorHandling.Maybe.Value("foo").FlatMap(ErrorHandling.Maybe.Value, function);
            });
        }

        [Fact]
        public void Passing_Null_Selector_Function__Throws_ArgumentNullException() {
            Assert.Throws<ArgumentNullException>(() => {
                Func<string, IMaybe<bool>> function = null;
                ErrorHandling.Maybe.Value("foo").FlatMap(function, (s, b) => s);
            });
        }

        [Fact]
        public void Passing_Null_To_Nullable_ResultSelector_Throws() {
            const string input = "hello";
            Assert.Throws<ArgumentNullException>(
                AssertionUtilities.ResultSelector,
                () =>
                    ErrorHandling.Maybe.Value(input)
                        .FlatMap(x => (int?) 2, (Func<string, int, string>) null)
            );
        }

        [Fact]
        public void Passing_Null_To_Nullable_Selector_Throws() {
            const string input = "hello";
            Assert.Throws<ArgumentNullException>(
                AssertionUtilities.SelectorName,
                () =>
                    ErrorHandling.Maybe.Value(input)
                        .FlatMap((Func<string, int?>) null, (s, i) => s)
            );
        }

        [Fact]
        public void
            ResultSelector_Overload__Flattening_From_String_Maybe_With_value_To_Nullable_Int_Maybe_With_Value__Expects_String_Maybe_With_Value() {
            const string input = "hello";
            int? nullabelInt = 2;
            ErrorHandling.Maybe.Value(input)
                .FlatMap(x => nullabelInt, (x, y) => x.Length + y)
                .AssertValue(input.Length + nullabelInt.Value);
        }

        [Fact]
        public void
            ResultSelector_Overload__Flattening_From_String_Maybe_With_value_To_Nullable_Int_Without_Value__Expects_String_Maybe_Without_Value() {
            const string input = "hello";
            int? nullableInt = null;
            ErrorHandling.Maybe.Value(input)
                .FlatMap(x => nullableInt, (x, y) => x.Length + y)
                .AssertNone();
        }

        [Fact]
        public void
            ResultSelector_Overload__Flattening_From_String_Maybe_With_value_To_String_Maybe_With_Value__Expects_String_Maybe_With_Value() {
            const string input = "hello";
            ErrorHandling.Maybe.Value(input)
                .FlatMap(x => input.ToMaybeNone(string.IsNullOrEmpty), (x, y) => x.Length + y.Length)
                .AssertValue(input.Length * 2);
        }

        [Fact]
        public void
            ResultSelector_Overload__Flattening_From_String_Maybe_With_value_To_String_Maybe_Without_Value__Expects_String_Maybe_Without_Value() {
            const string input = "hello";
            ErrorHandling.Maybe.Value(input)
                .FlatMap(x => input.ToMaybe(s => s.Length > 5), (x, y) => x.Length + y.Length)
                .AssertNone();
        }

        [Fact]
        public void
            ResultSelector_Overload__Flattening_From_String_Maybe_Without_value_To_Nullable_Int_With_Value__Expects_String_Maybe_Without_Value() {
            int? nullableInt = 2;
            ErrorHandling.Maybe.None<string>()
                .FlatMap(x => nullableInt, (x, y) => x.Length + y)
                .AssertNone();
        }

        [Fact]
        public void
            ResultSelector_Overload__Flattening_From_String_Maybe_Without_value_To_Nullable_int_Without_Value__Expects_String_Maybe_Without_Value() {
            int? nullableInt = null;
            ErrorHandling.Maybe.None<string>()
                .FlatMap(x => nullableInt, (x, y) => x.Length + y)
                .AssertNone();
        }

        [Fact]
        public void
            ResultSelector_Overload__Flattening_From_String_Maybe_Without_value_To_String_Maybe_With_Value__Expects_String_Maybe_Without_Value() {
            const string input = "hello";
            ErrorHandling.Maybe.None<string>()
                .FlatMap(x => input.ToMaybeNone(string.IsNullOrEmpty).AssertValue("hello"),
                    (x, y) => x.Length + y.Length)
                .AssertNone();
        }

        [Fact]
        public void
            ResultSelector_Overload__Flattening_From_String_Maybe_Without_value_To_String_Maybe_Without_Value__Expects_String_Maybe_Without_Value() {
            const string input = "hello";

            ErrorHandling.Maybe.None<string>()
                .FlatMap(x => input.ToMaybe(s => s.Length > 5).AssertNone(), (x, y) => x.Length + y.Length)
                .AssertNone();
        }
    }
}