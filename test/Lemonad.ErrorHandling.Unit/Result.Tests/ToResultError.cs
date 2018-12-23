using Lemonad.ErrorHandling.Extensions;
using Xunit;

namespace Lemonad.ErrorHandling.Unit.Result.Tests {
    public class ToResultError {
        [Fact]
        public void Create_Result_From_Value_With_False_Predicate() {
            var predicateExecuted = false;
            var valueSelectorExecuted = false;
            "ERROR".ToResultError(x => {
                predicateExecuted = true;
                return string.IsNullOrWhiteSpace(x);
            }, x => {
                valueSelectorExecuted = true;
                return "value";
            }).AssertValue("value");
            Assert.True(predicateExecuted);
            Assert.True(valueSelectorExecuted);
        }

        [Fact]
        public void Create_Result_From_Value_With_True_Predicate() {
            var predicateExecuted = false;
            var valueSelectorExecuted = false;
            "ERROR".ToResultError(x => {
                predicateExecuted = true;
                return string.IsNullOrWhiteSpace(x) == false;
            }, x => {
                valueSelectorExecuted = true;
                return "value";
            }).AssertError("ERROR");
            Assert.True(predicateExecuted);
            Assert.False(valueSelectorExecuted);
        }
    }
}