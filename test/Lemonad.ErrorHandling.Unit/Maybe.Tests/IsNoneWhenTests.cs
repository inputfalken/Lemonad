using System;
using Assertion;
using Lemonad.ErrorHandling.Extensions;
using Xunit;

namespace Lemonad.ErrorHandling.Unit.Maybe.Tests {
    public class NoneWhenTests {
        [Fact]
        public void Passing_Null_Predicate_Throws() =>
            Assert.Throws<ArgumentNullException>(
                AssertionUtilities.PredicateName,
                () => ErrorHandling.Maybe.Value("foo").IsNoneWhen(null)
            );

        [Fact]
        public void With_Truthy_Predicate__Expects_None() =>
            ErrorHandling.Maybe.Value("foo").IsNoneWhen(s => s.Length > 0).AssertNone();

        [Fact]
        public void With_Falsy_Predicate__Expects_Value() =>
            ErrorHandling.Maybe.Value("foo").IsNoneWhen(s => s.Length > 20).AssertValue("foo");
    }
}