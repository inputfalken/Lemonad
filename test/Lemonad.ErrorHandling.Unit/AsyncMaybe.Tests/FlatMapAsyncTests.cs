using System;
using System.Threading.Tasks;
using Assertion;
using Lemonad.ErrorHandling;
using Lemonad.ErrorHandling.Extensions.AsyncMaybe;
using Xunit;

namespace Lemonad.ErrorHandling.Unit.AsyncMaybe.Tests {
    public class FlatMapAsyncTests {
        [Fact]
        public async Task
            Flattening_From_String_Maybe_With_value_To_Nullable_Int_With_Value__Expects_String_Maybe_With_Value() {
            const string input = "hello";
            int? nullable = 2;

            await ErrorHandling.AsyncMaybe.Value(input)
                .FlatMapAsync(async x => {
                    await AssertionUtilities.Delay;
                    return nullable;
                })
                .AssertValue(nullable.Value);
        }

        [Fact]
        public async Task
            Flattening_From_String_Maybe_With_value_To_Nullable_Int_Without_Value__Expects_String_Maybe_Without_Value() {
            const string input = "hello";
            int? nullable = null;

            await ErrorHandling.AsyncMaybe.Value(input)
                .FlatMapAsync(async x => {
                    await AssertionUtilities.Delay;
                    return nullable;
                })
                .AssertNone();
        }

        [Fact]
        public async Task
            Flattening_From_String_Maybe_With_value_To_String_Maybe_With_Value__Expects_String_Maybe_With_Value() {
            const string input = "hello";
            await ErrorHandling.AsyncMaybe.Value(input)
                .FlatMapAsync(x => ErrorHandling.AsyncMaybe.Value("hello"))
                .AssertValue("hello");
        }

        [Fact]
        public async Task
            Flattening_From_String_Maybe_With_value_To_String_Maybe_Without_Value__Expects_String_Maybe_Without_Value() {
            const string input = "hello";
            await ErrorHandling.AsyncMaybe.Value(input)
                .FlatMapAsync(x => ErrorHandling.AsyncMaybe.None<string>())
                .AssertNone();
        }

        [Fact]
        public void
            Maybe_String_Whose_Property_HasValue_Is_True__Pasing_Null_Selector__ArgumentNullReferenceException_Thrown() {
            Assert.Throws<ArgumentNullException>(() => {
                Func<string, IAsyncMaybe<string>> selector = null;
                ErrorHandling.AsyncMaybe.Value("foo").FlatMapAsync(selector);
            });
        }

        [Fact]
        public void
            Maybe_String_Whose_Property_HasValue_Is_True__Pasing_Null_Selector_And_ResultSelector_ArgumentNullReferenceException_Thrown() {
            Assert.Throws<ArgumentNullException>(() => {
                Func<string, IAsyncMaybe<string>> selector = null;
                Func<string, string, string> resultSelector = null;
                ErrorHandling.AsyncMaybe.Value("foo").FlatMapAsync(selector, resultSelector);
            });
        }

        [Fact]
        public void
            Maybe_String_Whose_Property_HasValue_Is_True_Flatmapping_Some__Pasing_Null_ResultSelector__ArgumentNullReferenceException_Thrown() {
            Assert.Throws<ArgumentNullException>(() => {
                Func<string, string, string> resultSelector = null;
                ErrorHandling.AsyncMaybe.Value("foo").FlatMapAsync(ErrorHandling.AsyncMaybe.Value, resultSelector);
            });
        }

        [Fact]
        public void Passing_Null_Function__Throws_ArgumentNullException() {
            Assert.Throws<ArgumentNullException>(() => {
                Func<string, IAsyncMaybe<bool>> function = null;
                ErrorHandling.AsyncMaybe.Value("foo").FlatMapAsync(function);
            });
        }

        [Fact]
        public async Task
            ResultSelector_Overload__Flattening_From_String_Maybe_With_value_To_Nullable_Int_Maybe_With_Value__Expects_String_Maybe_With_Value() {
            const string input = "hello";

            int? nullabelInt = 2;

            await ErrorHandling.AsyncMaybe.Value(input)
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

            await ErrorHandling.AsyncMaybe.Value(input)
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

            await ErrorHandling.AsyncMaybe.Value(input)
                .FlatMapAsync(x => ErrorHandling.AsyncMaybe.Value("hello"), (x, y) => x.Length + y.Length)
                .AssertValue(input.Length * 2);
        }

        [Fact]
        public async Task
            ResultSelector_Overload__Flattening_From_String_Maybe_With_value_To_String_Maybe_Without_Value__Expects_String_Maybe_Without_Value() {
            const string input = "hello";

            await ErrorHandling.AsyncMaybe.Value(input)
                .FlatMapAsync(x => ErrorHandling.AsyncMaybe.None<string>(), (x, y) => x.Length + y.Length);
        }

        [Fact]
        public async Task
            ResultSelector_Overload__Flattening_From_String_Maybe_Without_value_To_Nullable_Int_With_Value__Expects_String_Maybe_Without_Value() {
            int? nullableInt = 2;
            await ErrorHandling.AsyncMaybe.None<string>()
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

            await ErrorHandling.AsyncMaybe.None<string>()
                .FlatMapAsync(async x => {
                    await AssertionUtilities.Delay;
                    return nullableInt;
                })
                .AssertNone();
        }

        [Fact]
        public async Task
            ResultSelector_Overload__Flattening_From_String_Maybe_Without_value_To_String_Maybe_With_Value__Expects_String_Maybe_Without_Value() {
            await ErrorHandling.AsyncMaybe.None<string>()
                .FlatMapAsync(x => ErrorHandling.AsyncMaybe.Value("hello"), (x, y) => x.Length + y.Length);
        }

        [Fact]
        public async Task
            ResultSelector_Overload__Flattening_From_String_Maybe_Without_value_To_String_Maybe_Without_Value__Expects_String_Maybe_Without_Value() {
            await ErrorHandling.AsyncMaybe.None<string>()
                .FlatMapAsync(x => ErrorHandling.AsyncMaybe.None<string>())
                .AssertNone();
        }
    }
}

public class FlatMapTests {
    [Fact]
    public async Task
        Flattening_From_String_Maybe_With_value_To_Nullable_Int_With_Value__Expects_String_Maybe_With_Value() {
        const string input = "hello";
        int? nullabelInt = 2;

        await AsyncMaybe.Value(input)
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

        await AsyncMaybe.Value(input)
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
        await AsyncMaybe.Value(input)
            .FlatMapAsync(x => AsyncMaybe.Value("hello"))
            .AssertValue("hello");
    }

    [Fact]
    public async Task
        Flattening_From_String_Maybe_With_value_To_String_Maybe_Without_Value__Expects_String_Maybe_Without_Value() {
        const string input = "hello";
        await AsyncMaybe.Value(input)
            .FlatMapAsync(x => AsyncMaybe.None<int>())
            .AssertNone();
    }

    [Fact]
    public void
        Maybe_String_Whose_Property_HasValue_Is_True__Pasing_Null_Selector__ArgumentNullReferenceException_Thrown() {
        Assert.Throws<ArgumentNullException>(() => {
            Func<string, IAsyncMaybe<string>> selector = null;
            AsyncMaybe.Value("foo").FlatMapAsync(selector);
        });
    }

    [Fact]
    public void
        Maybe_String_Whose_Property_HasValue_Is_True__Pasing_Null_Selector_And_ResultSelector_ArgumentNullReferenceException_Thrown() {
        Assert.Throws<ArgumentNullException>(() => {
            Func<string, IAsyncMaybe<string>> selector = null;
            Func<string, string, string> resultSelector = null;
            AsyncMaybe.Value("foo").FlatMapAsync(selector, resultSelector);
        });
    }

    [Fact]
    public void
        Maybe_String_Whose_Property_HasValue_Is_True_Flatmapping_Some__Pasing_Null_ResultSelector__ArgumentNullReferenceException_Thrown() {
        Assert.Throws<ArgumentNullException>(() => {
            Func<string, string, string> resultSelector = null;
            AsyncMaybe.Value("foo").FlatMapAsync(AsyncMaybe.Value, resultSelector);
        });
    }

    [Fact]
    public void Passing_Null_Function__Throws_ArgumentNullException() {
        Assert.Throws<ArgumentNullException>(() => {
            Func<string, IAsyncMaybe<bool>> function = null;
            AsyncMaybe.Value("foo").FlatMapAsync(function);
        });
    }

    [Fact]
    public async Task
        ResultSelector_Overload__Flattening_From_String_Maybe_With_value_To_Nullable_Int_Maybe_With_Value__Expects_String_Maybe_With_Value() {
        const string input = "hello";

        int? nullabelInt = 2;

        await AsyncMaybe.Value(input)
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

        await AsyncMaybe.Value(input)
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

        await AsyncMaybe.Value(input)
            .FlatMapAsync(x => AsyncMaybe.Value(input), (x, y) => x.Length + y.Length)
            .AssertValue(input.Length * 2);
    }

    [Fact]
    public async Task
        ResultSelector_Overload__Flattening_From_String_Maybe_With_value_To_String_Maybe_Without_Value__Expects_String_Maybe_Without_Value() {
        const string input = "hello";

        await AsyncMaybe.Value(input)
            .FlatMapAsync(x => AsyncMaybe.None<string>(), (x, y) => x.Length + y.Length);
    }

    [Fact]
    public async Task
        ResultSelector_Overload__Flattening_From_String_Maybe_Without_value_To_Nullable_Int_With_Value__Expects_String_Maybe_Without_Value() {
        int? nullableInt = 2;
        await AsyncMaybe.None<string>()
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

        await AsyncMaybe.None<string>()
            .FlatMapAsync(async x => {
                await AssertionUtilities.Delay;
                return nullableInt;
            })
            .AssertNone();
    }

    [Fact]
    public async Task
        ResultSelector_Overload__Flattening_From_String_Maybe_Without_value_To_String_Maybe_With_Value__Expects_String_Maybe_Without_Value() {
        await AsyncMaybe.None<string>()
            .FlatMapAsync(x => AsyncMaybe.Value("hello"),
                (x, y) => x.Length + y.Length);
    }

    [Fact]
    public async Task
        ResultSelector_Overload__Flattening_From_String_Maybe_Without_value_To_String_Maybe_Without_Value__Expects_String_Maybe_Without_Value() {
        await AsyncMaybe.None<string>()
            .FlatMapAsync(x => AsyncMaybe.None<string>())
            .AssertNone();
    }
}