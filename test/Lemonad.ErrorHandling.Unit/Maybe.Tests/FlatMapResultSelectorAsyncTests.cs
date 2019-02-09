﻿using System;
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
            int? nullabelInt = 2;
            await input
                .ToMaybe(s => s.Length > 4)
                .AssertValue("hello")
                .FlatMapAsync(async x => {
                    await AssertionUtilities.Delay;
                    return nullabelInt;
                })
                .AssertValue(nullabelInt.Value);
        }

        [Fact]
        public async Task
            Flattening_From_String_Maybe_With_value_To_Nullable_Int_Without_Value__Expects_String_Maybe_Without_Value() {
            const string input = "hello";
            int? nullableInt = null;
            await input
                .ToMaybeNone(string.IsNullOrEmpty)
                .AssertValue("hello")
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
            await input
                .ToMaybeNone(string.IsNullOrEmpty)
                .AssertValue("hello")
                .FlatMapAsync(x => { return input.ToMaybe(s => s.Length > 4).ToMaybeAsync(); })
                .AssertValue(input);
        }

        [Fact]
        public async Task
            Flattening_From_String_Maybe_With_value_To_String_Maybe_Without_Value__Expects_String_Maybe_Without_Value() {
            const string input = "hello";
            await input
                .ToMaybeNone(string.IsNullOrEmpty)
                .AssertValue("hello")
                .FlatMapAsync(x => input.ToMaybe(s => s.Length > 5).ToMaybeAsync())
                .AssertNone();
        }

        [Fact]
        public void
            Passing_Both_Null_ResultSelector_Function_And_SelectorFunction__Throws_ArgumentNullException() {
            Assert.Throws<ArgumentNullException>(() => {
                Func<string, string, string> function = null;
                ErrorHandling.Maybe.Value("foo").FlatMapAsync(AsyncMaybe.Value, function);
            });
        }

        [Fact]
        public void Passing_Null_ResultSelector_Function__Throws_ArgumentNullException() {
            Assert.Throws<ArgumentNullException>(() => {
                Func<string, string, string> function = null;
                ErrorHandling.Maybe.Value("foo").FlatMapAsync(AsyncMaybe.Value, function);
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
                    input
                        .ToMaybe(s => s.Length > 4)
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
                    input
                        .ToMaybe(s => s.Length > 4)
                        .FlatMapAsync((Func<string, Task<int?>>) null, (s, i) => s)
            );
        }

        [Fact]
        public async Task
            ResultSelector_Overload__Flattening_From_String_Maybe_With_value_To_Nullable_Int_Maybe_With_Value__Expects_String_Maybe_With_Value() {
            const string input = "hello";
            int? nullabelInt = 2;
            await input
                .ToMaybe(s => s.Length > 4)
                .AssertValue("hello")
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
            await input
                .ToMaybeNone(string.IsNullOrEmpty)
                .AssertValue("hello")
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
            await input
                .ToMaybe(s => s.Length > 4)
                .AssertValue("hello")
                .FlatMapAsync(x => input.ToMaybeNone(string.IsNullOrEmpty).ToMaybeAsync(),
                    (x, y) => x.Length + y.Length)
                .AssertValue(input.Length * 2);
        }

        [Fact]
        public async Task
            ResultSelector_Overload__Flattening_From_String_Maybe_With_value_To_String_Maybe_Without_Value__Expects_String_Maybe_Without_Value() {
            const string input = "hello";
            await input
                .ToMaybeNone(string.IsNullOrEmpty)
                .AssertValue("hello")
                .FlatMapAsync(x => input.ToMaybe(s => s.Length > 5).ToMaybeAsync(), (x, y) => x.Length + y.Length)
                .AssertNone();
        }

        [Fact]
        public async Task
            ResultSelector_Overload__Flattening_From_String_Maybe_Without_value_To_Nullable_Int_With_Value__Expects_String_Maybe_Without_Value() {
            const string input = "hello";
            int? nullableInt = 2;
            await input.ToMaybe(s => s.Length > 5)
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
            const string input = "hello";
            int? nullableInt = null;
            await input
                .ToMaybe(string.IsNullOrEmpty)
                .AssertNone()
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
            await input.ToMaybe(s => s.Length > 5)
                .AssertNone()
                .FlatMapAsync(x => input.ToMaybeNone(string.IsNullOrEmpty).ToMaybeAsync().AssertValue("hello"),
                    (x, y) => x.Length + y.Length)
                .AssertNone();
        }

        [Fact]
        public async Task
            ResultSelector_Overload__Flattening_From_String_Maybe_Without_value_To_String_Maybe_Without_Value__Expects_String_Maybe_Without_Value() {
            const string input = "hello";

            await input.ToMaybe(string.IsNullOrEmpty)
                .AssertNone()
                .FlatMapAsync(x => input.ToMaybe(s => s.Length > 5).ToMaybeAsync().AssertNone(),
                    (x, y) => x.Length + y.Length)
                .AssertNone();
        }
    }
}