﻿using System;
using Assertion;
using Lemonad.ErrorHandling.Extensions;
using Xunit;

namespace Lemonad.ErrorHandling.Unit.Maybe.Tests {
    public class NoneWhenTests {
        [Fact]
        public void
            Maybe_String_Whose_Property_HasValue_Is_True__Pasing_Null_Predicate__ArgumentNullReferenceException_Thrown() {
            Assert.Throws<ArgumentNullException>(() => {
                Func<string, bool> predicate = null;
                ErrorHandling.Maybe.Value("foo").IsNoneWhen(predicate);
            });
        }

        [Fact]
        public void Null_Predicate__Throws_ArgumentNullException() {
            Assert.Throws<ArgumentNullException>(() => {
                Func<string, bool> predicate = null;
                "foo".ToMaybeNone(predicate);
            });
        }

        [Fact]
        public void When_Predicate_Returns_False__Maybe_Is_Expected_To_HaveValue() {
             "".ToMaybeNone(s => false).AssertValue("");
        }

        [Fact]
        public void When_Predicate_Returns_True__Maybe_Is_Expected_To_HaveValue() {
             "".ToMaybeNone(s => true).AssertNone();
        }
    }
}