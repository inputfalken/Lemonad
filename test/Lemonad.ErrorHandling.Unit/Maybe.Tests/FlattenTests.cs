﻿using System;
using Assertion;
using Lemonad.ErrorHandling.Extensions;
using Xunit;

namespace Lemonad.ErrorHandling.Unit.Maybe.Tests {
    public class FlattenTests {
        [Fact]
        public void
            Flattening_From_String_Maybe_With_No_Value_To_Maybe_Int_With_No_Value__Expects_String_Maybe_With_Value() {
            ErrorHandling.Maybe.None<string>().AssertNone().Flatten(x => 2.ToMaybeNone().AssertNone()).AssertNone();
        }

        [Fact]
        public void
            Flattening_From_String_Maybe_With_No_Value_To_Maybe_Int_With_Value__Expects_String_Maybe_With_Value() {
            ErrorHandling.Maybe.None<string>().Flatten(x => ErrorHandling.Maybe.Value(2).AssertValue(2)).AssertNone();
        }

        [Fact]
        public void
            Flattening_From_String_Maybe_With_Value_To_Maybe_Int_With_No_Value__Expects_String_Maybe_With_Value() {
            ErrorHandling.Maybe.Value("hello")
                .Flatten(x => 2.ToMaybeNone().AssertNone())
                .AssertNone();
        }

        [Fact]
        public void
            Flattening_From_String_Maybe_With_Value_To_Maybe_Int_With_Value__Expects_String_Maybe_With_Value() {
            const string input = "hello";
            ErrorHandling.Maybe.Value(input)
                .Flatten(x => ErrorHandling.Maybe.Value(2).AssertValue(2))
                .AssertValue(input);
        }

        [Fact]
        public void Passing_Null_Selector_Throws() =>
            Assert.Throws<ArgumentNullException>(AssertionUtilities.SelectorName,
                () => ErrorHandling.Maybe.Value(2).Flatten((Func<int, IMaybe<int>>) null));
    }
}