using System.Threading.Tasks;
using Lemonad.ErrorHandling.Extensions;
using static Lemonad.ErrorHandling.Test.AssertionUtilities;
using Xunit;

namespace Lemonad.ErrorHandling.Test.Outcome.Tests {
    public class FilterWithTaskPredicate {
        private static Task Delay => Task.Delay(200);

        [Fact]
        public async Task
            Result_With_Error__Expects_Predicate_Never_To_Be_Executed_And_ErrorSelector_Never_To_Be_Invoked() {
            var predicateExectued = false;
            var errorSelectorExectued = false;

            var result = await DivisionAsync(10, 0)
                .AsOutcome()
                .Filter(async x => {
                    await Delay;
                    predicateExectued = true;
                    return x == 2;
                }, () => {
                    errorSelectorExectued = true;
                    return "Bad";
                }).Match(x => x, s => -1);

            Assert.False(predicateExectued,
                "Should not get exectued since there's an error before the predicate was applied.");
            Assert.False(errorSelectorExectued,
                "Should not get exectued since there's an error before the predicate was applied.");
            Assert.Equal(-1, result);
        }

        [Fact]
        public async Task
            Result_With_Value_With_Falsy_Predicate__Expects_Predicate_To_Be_Executed_And_ErrorSelector_To_Be_Invoked() {
            var predicateExectued = false;
            var errorSelectorExectued = false;

            var result = await DivisionAsync(10, 2)
                .AsOutcome()
                .Filter(async x => {
                    await Delay;
                    predicateExectued = true;
                    return false;
                }, () => {
                    errorSelectorExectued = true;
                    return "Bad";
                }).Match(x => x, s => -1);

            Assert.True(predicateExectued, "Should get exectued since there's a value from the result.");
            Assert.True(errorSelectorExectued, "Should get exectued since the predicate was truthy.");
            Assert.Equal(-1, result);
        }

        [Fact]
        public async Task
            Result_With_Value_With_Truthy_Predicate__Expects_Predicate_To_Be_Executed_And_ErrorSelector_To_Never_Be_Invoked() {
            var predicateExectued = false;
            var errorSelectorExectued = false;
            var result = await DivisionAsync(10, 2)
                .AsOutcome()
                .Filter(async x => {
                    await Delay;
                    predicateExectued = true;
                    return true;
                }, () => {
                    errorSelectorExectued = true;
                    return "Bad";
                }).Match(x => x, s => -1);

            Assert.True(predicateExectued,
                "Should get exectued since there's a value from the result.");
            Assert.False(errorSelectorExectued,
                "Should not get exectued since the predicate was falsy.");
            Assert.Equal(5, result);
        }
    }
}