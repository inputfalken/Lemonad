using System;
using Assertion;
using Xunit;

namespace Lemonad.ErrorHandling.Unit.Result.Tests {
    public class MapTests {
        [Fact]
        public void Passing_Null_Selector_Throws() {
            Assert.Throws<ArgumentNullException>(
                AssertionUtilities.SelectorName,
                () => AssertionUtilities
                    .Division(2, 0)
                    .Map<string>(null)
            );
        }

        [Fact]
        public void Result_With_Error_Maps__Expects_Selector_Never_Be_Executed() {
            var selectorExectued = false;
            AssertionUtilities
                .Division(2, 0)
                .Map(x => {
                    selectorExectued = true;
                    return x * 8;
                })
                .AssertError("Can not divide '2' with '0'.");

            Assert.False(selectorExectued,
                "The selector function should never get executed if there's no value in the Result<T, TError>.");
        }

        [Fact]
        public void Result_With_Value_Maps__Expects_Selector_Be_Executed_And_Value_To_Be_Mapped() {
            var selectorExectued = false;
            AssertionUtilities
                .Division(10, 2)
                .Map(x => {
                    selectorExectued = true;
                    return x * 4;
                })
                .AssertValue(20d);

            Assert.True(selectorExectued, "The selector function should get executed since the result has value.");
        }
    }
}