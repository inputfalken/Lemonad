using System;
using System.Threading.Tasks;
using Assertion;
using Lemonad.ErrorHandling.Extensions;
using Lemonad.ErrorHandling.Extensions.AsyncMaybe;
using Xunit;

namespace Lemonad.ErrorHandling.Unit.AsyncMaybe.Tests {
    public class FlatMapTests {
        [Fact]
        public async Task
            Flattening_From_String_Maybe_With_value_To_Nullable_Int_With_Value__Expects_String_Maybe_With_Value() {
            const string input = "hello";
            int? nullabelInt = 2;

            await ErrorHandling.AsyncMaybe.Value(input)
                .FlatMap(x => nullabelInt)
                .AssertValue(nullabelInt.Value);
        }

        [Fact]
        public async Task
            Flattening_From_String_Maybe_With_value_To_Nullable_Int_Without_Value__Expects_String_Maybe_Without_Value() {
            const string input = "hello";
            int? nullableInt = null;

            await ErrorHandling.AsyncMaybe.Value(input)
                .FlatMap(x => nullableInt)
                .AssertNone();
        }

        [Fact]
        public async Task
            Flattening_From_String_Maybe_With_value_To_String_Maybe_With_Value__Expects_String_Maybe_With_Value() {
            const string input = "hello";
            await ErrorHandling.AsyncMaybe.Value(input)
                .FlatMap(x => input.ToMaybeNone(string.IsNullOrEmpty).AssertValue("hello"))
                .AssertValue("hello");
        }

        [Fact]
        public async Task
            Flattening_From_String_Maybe_With_value_To_String_Maybe_Without_Value__Expects_String_Maybe_Without_Value() {
            const string input = "hello";
            await ErrorHandling.AsyncMaybe.Value(input)
                .FlatMap(x => input.ToMaybe(s => s.Length > 5).AssertNone())
                .AssertNone();
        }

        [Fact]
        public void
            Maybe_String_Whose_Property_HasValue_Is_True__Pasing_Null_Selector__ArgumentNullReferenceException_Thrown() {
            Assert.Throws<ArgumentNullException>(() => {
                Func<string, IMaybe<string>> selector = null;
                ErrorHandling.AsyncMaybe.Value("foo").FlatMap(selector);
            });
        }

        [Fact]
        public void
            Maybe_String_Whose_Property_HasValue_Is_True__Pasing_Null_Selector_And_ResultSelector_ArgumentNullReferenceException_Thrown() {
            Assert.Throws<ArgumentNullException>(() => {
                Func<string, IMaybe<string>> selector = null;
                Func<string, string, string> resultSelector = null;
                ErrorHandling.AsyncMaybe.Value("foo").FlatMap(selector, resultSelector);
            });
        }

        [Fact]
        public void
            Maybe_String_Whose_Property_HasValue_Is_True_Flatmapping_Some__Pasing_Null_ResultSelector__ArgumentNullReferenceException_Thrown() {
            Assert.Throws<ArgumentNullException>(() => {
                Func<string, string, string> resultSelector = null;
                ErrorHandling.AsyncMaybe.Value("foo").FlatMap(ErrorHandling.Maybe.Value, resultSelector);
            });
        }

        [Fact]
        public void Passing_Null_Function__Throws_ArgumentNullException() {
            Assert.Throws<ArgumentNullException>(() => {
                Func<string, IMaybe<bool>> function = null;
                ErrorHandling.AsyncMaybe.Value("foo").FlatMap(function);
            });
        }

        [Fact]
        public async Task
            ResultSelector_Overload__Flattening_From_String_Maybe_With_value_To_Nullable_Int_Maybe_With_Value__Expects_String_Maybe_With_Value() {
            const string input = "hello";

            int? nullabelInt = 2;

            await ErrorHandling.AsyncMaybe.Value(input)
                .FlatMap(x => nullabelInt, (x, y) => x.Length + y)
                .AssertValue(input.Length + nullabelInt.Value);
        }

        [Fact]
        public async Task
            ResultSelector_Overload__Flattening_From_String_Maybe_With_value_To_Nullable_Int_Without_Value__Expects_String_Maybe_Without_Value() {
            const string input = "hello";
            int? nullableInt = null;

            await ErrorHandling.AsyncMaybe.Value(input)
                .FlatMap(x => nullableInt, (x, y) => x.Length + y)
                .AssertNone();
        }

        [Fact]
        public async Task
            ResultSelector_Overload__Flattening_From_String_Maybe_With_value_To_String_Maybe_With_Value__Expects_String_Maybe_With_Value() {
            const string input = "hello";
            var stringMaybe = input.ToMaybeNone(string.IsNullOrEmpty);

            await ErrorHandling.AsyncMaybe.Value(input)
                .FlatMap(x => stringMaybe.AssertValue("hello"), (x, y) => x.Length + y.Length)
                .AssertValue(input.Length * 2);
        }

        [Fact]
        public async Task
            ResultSelector_Overload__Flattening_From_String_Maybe_With_value_To_String_Maybe_Without_Value__Expects_String_Maybe_Without_Value() {
            const string input = "hello";

            await ErrorHandling.AsyncMaybe.Value(input)
                .FlatMap(x => input.ToMaybe(s => s.Length > 5).AssertNone(), (x, y) => x.Length + y.Length);
        }

        [Fact]
        public async Task
            ResultSelector_Overload__Flattening_From_String_Maybe_Without_value_To_Nullable_Int_With_Value__Expects_String_Maybe_Without_Value() {
            int? nullableInt = 2;
            await ErrorHandling.AsyncMaybe.None<string>()
                .FlatMap(x => nullableInt, (x, y) => x.Length + y)
                .AssertNone();
        }

        [Fact]
        public async Task
            ResultSelector_Overload__Flattening_From_String_Maybe_Without_value_To_Nullable_int_Without_Value__Expects_String_Maybe_Without_Value() {
            int? nullableInt = null;

            await ErrorHandling.AsyncMaybe.None<string>()
                .FlatMap(x => nullableInt)
                .AssertNone();
        }

        [Fact]
        public async Task
            ResultSelector_Overload__Flattening_From_String_Maybe_Without_value_To_String_Maybe_With_Value__Expects_String_Maybe_Without_Value() {
            const string input = "hello";
            await ErrorHandling.AsyncMaybe.None<string>()
                .FlatMap(x => input.ToMaybeNone(string.IsNullOrEmpty).AssertValue("hello"),
                    (x, y) => x.Length + y.Length);
        }

        [Fact]
        public async Task
            ResultSelector_Overload__Flattening_From_String_Maybe_Without_value_To_String_Maybe_Without_Value__Expects_String_Maybe_Without_Value() {
            const string input = "hello";
            await ErrorHandling.AsyncMaybe.None<string>()
                .FlatMap(x => input.ToMaybe(s => s.Length > 5).AssertNone())
                .AssertNone();
        }
    }
}