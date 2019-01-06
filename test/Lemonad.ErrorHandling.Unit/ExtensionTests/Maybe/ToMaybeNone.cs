using System;
using Assertion;
using Lemonad.ErrorHandling.Extensions;
using Xunit;

namespace Lemonad.ErrorHandling.Unit.ExtensionTests.Maybe {
    public class ToMaybeNone {
        [Fact]
        public void Null_Predicate__Throws_ArgumentNullException() =>
            Assert.Throws<ArgumentNullException>(() => { "foo".ToMaybeNone(null); });

        [Fact]
        public void When_Predicate_Returns_False__Maybe_Is_Expected_To_HaveValue() =>
            "".ToMaybeNone(s => false).AssertValue("");

        [Fact]
        public void When_Predicate_Returns_True__Maybe_Is_Expected_To_HaveValue() =>
            "".ToMaybeNone(s => true).AssertNone();
    }
}