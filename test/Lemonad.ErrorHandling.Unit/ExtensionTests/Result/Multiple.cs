using System;
using Assertion;
using Lemonad.ErrorHandling.Extensions.Result;
using Xunit;

namespace Lemonad.ErrorHandling.Unit.ExtensionTests.Result {
    public class Multiple {
        [Fact]
        public void One_Successful_Predicate_And_One_Failed_Predicate() {
            AssertionUtilities.Division(10, 2).Multiple(
                x => x.Filter(y => y == 5, _ => "Value is not equal to 5."),
                x => x.Filter(y => y == 0, _ => "Value is not equal to 0.")
            ).AssertError(new[] {"Value is not equal to 0."});
        }

        [Fact]
        public void Passing_Null_Result_Throws()
            => Assert.Throws<ArgumentNullException>(
                AssertionUtilities.ExtensionParameterName,
                () => ((IResult<string, int>) null).Multiple()
            );

        [Fact]
        public void Two_Failed_Predicates() {
            AssertionUtilities.Division(10, 2).Multiple(
                x => x.Filter(y => y == 4, _ => "Value is not equal to 4."),
                x => x.Filter(y => y == 0, _ => "Value is not equal to 0.")
            ).AssertError(new[] {"Value is not equal to 4.", "Value is not equal to 0."});
        }

        [Fact]
        public void Two_Successful_Predicates() {
            AssertionUtilities.Division(10, 2).Multiple(
                x => x.Filter(y => y == 5, _ => "Value is not equal to 4."),
                x => x.Filter(y => true, _ => "This should never happen!")
            ).AssertValue(5);
        }
    }
}