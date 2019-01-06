using System;
using System.Threading.Tasks;
using Assertion;
using Lemonad.ErrorHandling.Extensions.AsyncResult;
using Xunit;

namespace Lemonad.ErrorHandling.Unit.AsyncResult.Tests {
    public class IsErrorWhenAsyncTests {
        [Fact]
        public void Passing_Null_ErrorSelector_Throws() {
            Assert.Throws<ArgumentNullException>(
                AssertionUtilities.ErrorSelectorName,
                () => AssertionUtilities.DivisionAsync(10, 2)
                    .IsErrorWhenAsync(async x => {
                        await AssertionUtilities.Delay;
                        return true;
                    }, null)
            );
        }

        [Fact]
        public void Passing_Null_Predicate_Throws() {
            Assert.Throws<ArgumentNullException>(
                AssertionUtilities.PredicateName,
                () => AssertionUtilities.DivisionAsync(10, 2).IsErrorWhenAsync(null, d => "")
            );
        }

        [Fact]
        public async Task
            Result_With_Error__Expects_Predicate_Never_To_Be_Executed_And_ErrorSelector_Never_To_Be_Invoked() {
            var predicateExectued = false;
            var errorSelectorExectued = false;
            await AssertionUtilities
                .DivisionAsync(10, 0)
                .IsErrorWhenAsync(async d => {
                    predicateExectued = true;
                    await AssertionUtilities.Delay;
                    return d == 2;
                }, x => {
                    errorSelectorExectued = true;
                    return "Bad";
                })
                .AssertError("Can not divide '10' with '0'.");

            Assert.False(predicateExectued,
                "Should not get exectued since there's an error before the predicate was applied.");
            Assert.False(errorSelectorExectued,
                "Should not get exectued since there's an error before the predicate was applied.");
        }

        [Fact]
        public async Task
            Result_With_Value_With_Falsy_Predicate__Expects_Predicate_To_Be_Executed_And_ErrorSelector_To_Never_Be_Invoked() {
            var predicateExectued = false;
            var errorSelectorExectued = false;
            await AssertionUtilities
                .DivisionAsync(10, 2)
                .IsErrorWhenAsync(async d => {
                    await AssertionUtilities.Delay;
                    predicateExectued = true;
                    return false;
                }, x => {
                    errorSelectorExectued = true;
                    return "Bad";
                })
                .AssertValue(5);

            Assert.True(predicateExectued,
                "Should get exectued since there's a value from the result.");
            Assert.False(errorSelectorExectued,
                "Should not get exectued since the predicate was falsy.");
        }

        [Fact]
        public async Task
            Result_With_Value_With_Truthy_Predicate__Expects_Predicate_To_Be_Executed_And_ErrorSelector_To_Be_Invoked() {
            var predicateExectued = false;
            var errorSelectorExectued = false;
            await AssertionUtilities
                .DivisionAsync(10, 2)
                .IsErrorWhenAsync(async d => {
                    await AssertionUtilities.Delay;
                    predicateExectued = true;
                    return true;
                }, x => {
                    errorSelectorExectued = true;
                    return "Bad";
                })
                .AssertError("Bad");

            Assert.True(predicateExectued, "Should get exectued since there's a value from the result.");
            Assert.True(errorSelectorExectued, "Should get exectued since the predicate was truthy.");
        }
    }
}