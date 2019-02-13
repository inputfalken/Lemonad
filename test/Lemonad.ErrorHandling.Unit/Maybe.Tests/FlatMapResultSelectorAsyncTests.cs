using System;
using System.Threading.Tasks;
using Assertion;
using Lemonad.ErrorHandling.Extensions;
using Lemonad.ErrorHandling.Extensions.AsyncMaybe;
using Lemonad.ErrorHandling.Extensions.Maybe;
using Xunit;

namespace Lemonad.ErrorHandling.Unit.Maybe.Tests {
    public class FlatMapAsyncResultSelectorAsyncTests {
        [Fact]
        public async Task
            Flattening_From_String_Maybe_With_value_To_Nullable_Int_With_Value__Expects_String_Maybe_With_Value() {
            const string input = "hello";
            int? nullableInt = 2;
            await ErrorHandling.Maybe.Value(input)
                .FlatMapAsync(async x => {
                    await AssertionUtilities.Delay;
                    return nullableInt;
                })
                .AssertValue(nullableInt.Value);
        }

        [Fact]
        public async Task
            Flattening_From_String_Maybe_With_value_To_Nullable_Int_Without_Value__Expects_String_Maybe_Without_Value() {
            const string input = "hello";
            int? nullableInt = null;
            await ErrorHandling.Maybe.Value(input)
                .FlatMapAsync(async x => {
                    await AssertionUtilities.Delay;
                    return nullableInt;
                })
                .AssertNone();
        }

        [Fact]
        public async Task
            Flattening_From_String_Maybe_With_value_To_String_Maybe_With_Value__Expects_String_Maybe_With_Value() {
            const string input = "hello";
            await ErrorHandling.Maybe.Value(input)
                .FlatMapAsync(x => { return input.ToMaybe(s => s.Length > 4).ToMaybeAsync(); })
                .AssertValue(input);
        }

        [Fact]
        public async Task
            Flattening_From_String_Maybe_With_value_To_String_Maybe_Without_Value__Expects_String_Maybe_Without_Value() {
            const string input = "hello";
            await ErrorHandling.Maybe.Value(input)
                .FlatMapAsync(x => input.ToMaybe(s => s.Length > 5).ToMaybeAsync())
                .AssertNone();
        }

        [Fact]
        public void
            Passing_Both_Null_ResultSelector_Function_And_SelectorFunction__Throws_ArgumentNullException() {
            Assert.Throws<ArgumentNullException>(() => {
                Func<string, string, string> function = null;
                ErrorHandling.Maybe.Value("foo").FlatMapAsync(Lemonad.ErrorHandling.AsyncMaybe.Value, function);
            });
        }

        [Fact]
        public void Passing_Null_ResultSelector_Function__Throws_ArgumentNullException() {
            Assert.Throws<ArgumentNullException>(() => {
                Func<string, string, string> function = null;
                ErrorHandling.Maybe.Value("foo").FlatMapAsync(Lemonad.ErrorHandling.AsyncMaybe.Value, function);
            });
        }

        [Fact]
        public void Passing_Null_Selector_Function__Throws_ArgumentNullException() {
            Assert.Throws<ArgumentNullException>(() => {
                Func<string, IAsyncMaybe<bool>> function = null;
                ErrorHandling.Maybe.Value("foo").FlatMapAsync(function, (s, b) => s);
            });
        }

        [Fact]
        public void Passing_Null_To_Nullable_ResultSelector_Throws() {
            const string input = "hello";
            Assert.Throws<ArgumentNullException>(
                AssertionUtilities.ResultSelector,
                () =>
                    Lemonad.ErrorHandling.Maybe.Value(input)
                        .FlatMapAsync(async x => {
                            await AssertionUtilities.Delay;
                            return (int?) 2;
                        }, (Func<string, int, string>) null)
            );
        }

        [Fact]
        public void Passing_Null_To_Nullable_Selector_Throws() {
            const string input = "hello";
            Assert.Throws<ArgumentNullException>(
                AssertionUtilities.SelectorName,
                () =>
                    Lemonad.ErrorHandling.Maybe.Value(input)
                        .FlatMapAsync((Func<string, Task<int?>>) null, (s, i) => s)
            );
        }

        [Fact]
        public async Task
            ResultSelector_Overload__Flattening_From_String_Maybe_With_value_To_Nullable_Int_Maybe_With_Value__Expects_String_Maybe_With_Value() {
            const string input = "hello";
            int? nullabelInt = 2;
            await Lemonad.ErrorHandling.Maybe.Value(input)
                .FlatMapAsync(async x => {
                    await AssertionUtilities.Delay;
                    return nullabelInt;
                }, (x, y) => x.Length + y)
                .AssertValue(input.Length + nullabelInt.Value);
        }

        [Fact]
        public async Task
            ResultSelector_Overload__Flattening_From_String_Maybe_With_value_To_Nullable_Int_Without_Value__Expects_String_Maybe_Without_Value() {
            const string input = "hello";
            int? nullableInt = null;
            await Lemonad.ErrorHandling.Maybe.Value(input)
                .FlatMapAsync(async x => {
                    await AssertionUtilities.Delay;
                    return nullableInt;
                }, (x, y) => x.Length + y)
                .AssertNone();
        }

        [Fact]
        public async Task
            ResultSelector_Overload__Flattening_From_String_Maybe_With_value_To_String_Maybe_With_Value__Expects_String_Maybe_With_Value() {
            const string input = "hello";
            await Lemonad.ErrorHandling.Maybe.Value(input)
                .FlatMapAsync(x => input.ToMaybeNone(string.IsNullOrEmpty).ToMaybeAsync(),
                    (x, y) => x.Length + y.Length)
                .AssertValue(input.Length * 2);
        }

        [Fact]
        public async Task
            ResultSelector_Overload__Flattening_From_String_Maybe_With_value_To_String_Maybe_Without_Value__Expects_String_Maybe_Without_Value() {
            const string input = "hello";
            await Lemonad.ErrorHandling.Maybe.Value(input)
                .FlatMapAsync(x => input.ToMaybe(s => s.Length > 5).ToMaybeAsync(), (x, y) => x.Length + y.Length)
                .AssertNone();
        }

        [Fact]
        public async Task
            ResultSelector_Overload__Flattening_From_String_Maybe_Without_value_To_Nullable_Int_With_Value__Expects_String_Maybe_Without_Value() {
            int? nullableInt = 2;
            await ErrorHandling.Maybe.None<string>()
                .AssertNone()
                .FlatMapAsync(async x => {
                    await AssertionUtilities.Delay;
                    return nullableInt;
                }, (x, y) => x.Length + y)
                .AssertNone();
        }

        [Fact]
        public async Task
            ResultSelector_Overload__Flattening_From_String_Maybe_Without_value_To_Nullable_int_Without_Value__Expects_String_Maybe_Without_Value() {
            int? nullableInt = null;
            await ErrorHandling.Maybe.None<string>()
                .FlatMapAsync(async x => {
                    await AssertionUtilities.Delay;
                    return nullableInt;
                }, (x, y) => x.Length + y)
                .AssertNone();
        }

        [Fact]
        public async Task
            ResultSelector_Overload__Flattening_From_String_Maybe_Without_value_To_String_Maybe_With_Value__Expects_String_Maybe_Without_Value() {
            const string input = "hello";
            await ErrorHandling.Maybe.None<string>()
                .FlatMapAsync(x => input.ToMaybeNone(string.IsNullOrEmpty).ToMaybeAsync().AssertValue("hello"),
                    (x, y) => x.Length + y.Length)
                .AssertNone();
        }

        [Fact]
        public async Task
            ResultSelector_Overload__Flattening_From_String_Maybe_Without_value_To_String_Maybe_Without_Value__Expects_String_Maybe_Without_Value() {
            const string input = "hello";

            await ErrorHandling.Maybe.None<string>()
                .FlatMapAsync(x => input.ToMaybe(s => s.Length > 5).ToMaybeAsync().AssertNone(),
                    (x, y) => x.Length + y.Length)
                .AssertNone();
        }
    }
}