using System;
using System.Threading.Tasks;
using Assertion;
using Lemonad.ErrorHandling.Extensions.AsyncMaybe;
using Xunit;

namespace Lemonad.ErrorHandling.Unit.AsyncMaybe.Tests {
    public class NoneWhenTests {
        [Fact]
        public void Passing_Null_Predicate_Throws() =>
            Assert.Throws<ArgumentNullException>(
                AssertionUtilities.PredicateName,
                () => ErrorHandling.AsyncMaybe.Value("foo").IsNoneWhen(null)
            );

        [Fact]
        public async Task With_Falsy_Predicate__Expects_Value() =>
            await ErrorHandling.AsyncMaybe.Value("foo").IsNoneWhen(s => s.Length > 20).AssertValue("foo");

        [Fact]
        public async Task TaskWith_Truthy_Predicate__Expects_None() =>
            await ErrorHandling.AsyncMaybe.Value("foo").IsNoneWhen(s => s.Length > 0).AssertNone();
    }
}