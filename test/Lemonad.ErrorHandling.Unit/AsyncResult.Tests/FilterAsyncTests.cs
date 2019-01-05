using System;
using System.Threading.Tasks;
using Assertion;
using Lemonad.ErrorHandling.Extensions.AsyncResult;
using Xunit;

namespace Lemonad.ErrorHandling.Unit.AsyncResult.Tests {
    public class FilterAsyncTests {
        [Fact]
        public void Passing_Null_ErrorSelector_Throws() {
            Assert.Throws<ArgumentNullException>(
                AssertionUtilities.ErrorSelectorName, () => AssertionUtilities
                    .DivisionAsync(10, 0).FilterAsync(async d => {
                        await Task.Delay(50);
                        return d == 2;
                    }, null)
            );
        }

        [Fact]
        public void Passing_Null_Predicate_Throws() {
            Assert.Throws<ArgumentNullException>(
                AssertionUtilities.PredicateName,
                () => AssertionUtilities.DivisionAsync(10, 0).FilterAsync(null, x => "This should never happen!")
            );
        }

        [Fact]
        public async Task
            Result_With_Error__Expects_Predicate_Never_To_Be_Executed_And_ErrorSelector_Never_To_Be_Invoked_With_Parameter_ErrorSelector() {
            var predicateExectued = false;
            var errorSelectorExectued = false;
            await AssertionUtilities.DivisionAsync(10, 0).FilterAsync(async d => {
                predicateExectued = true;
                await Task.Delay(50);
                return d == 2;
            }, x => {
                errorSelectorExectued = true;
                return "This should never happen!";
            }).AssertError("Can not divide '10' with '0'.");

            Assert.False(predicateExectued,
                "Should not get exectued since there's an error before the predicate was applied.");
            Assert.False(errorSelectorExectued,
                "Should not get exectued since there's an error before the predicate was applied.");
        }

        [Fact]
        public async Task
            Result_With_Value_With_Falsy_Predicate__Expects_Predicate_To_Be_Executed_And_ErrorSelector_To_Be_Invoked_With_Parameter_ErrorSelector() {
            var predicateExectued = false;
            var errorSelectorExectued = false;
            await AssertionUtilities.DivisionAsync(10, 2).FilterAsync(async d => {
                await Task.Delay(50);
                predicateExectued = true;
                return false;
            }, x => {
                // Since the division operation was ok, but the predicate false, it makes sense that this value is positive.
                errorSelectorExectued = true;
                return "Good";
            }).AssertError("Good");

            Assert.True(predicateExectued, "Should get exectued since there's a value from the result.");
            Assert.True(errorSelectorExectued, "Should get exectued since the predicate was truthy.");
        }

        [Fact]
        public async Task
            Result_With_Value_With_Truthy_Predicate__Expects_Predicate_To_Be_Executed_And_ErrorSelector_To_Never_Be_Invoked_With_Parameter_ErrorSelector() {
            var predicateExectued = false;
            var errorSelectorExectued = false;
            await AssertionUtilities
                .DivisionAsync(10, 2)
                .FilterAsync(async d => {
                    await Task.Delay(50);
                    predicateExectued = true;
                    return true;
                }, x => {
                    errorSelectorExectued = true;
                    return "This should never happen.";
                })
                .AssertValue(5);

            Assert.True(predicateExectued,
                "Should get exectued since there's a value from the result.");
            Assert.False(errorSelectorExectued,
                "Should not get exectued since the predicate was falsy.");
        }
    }
}