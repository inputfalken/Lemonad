using Xunit;

namespace Lemonad.ErrorHandling.Test.Result.Tests {
    public class ToResultError {
        [Fact]
        public void Create_Result_From_Value_With_False_Predicate() {
            var predicateExecuted = false;
            var valueSelectorExecuted = false;
            var result = "ERROR".ToResultError(x => {
                predicateExecuted = true;
                return string.IsNullOrWhiteSpace(x);
            }, x => {
                valueSelectorExecuted = true;
                return "value";
            });
            Assert.Equal(default, result.Either.Error);
            Assert.Equal("value", result.Either.Value);
            Assert.True(result.Either.HasValue);
            Assert.False(result.Either.HasError);
            Assert.True(predicateExecuted);
            Assert.True(valueSelectorExecuted);
        }

        [Fact]
        public void Create_Result_From_Value_With_True_Predicate() {
            var predicateExecuted = false;
            var valueSelectorExecuted = false;
            var result = "ERROR".ToResultError(x => {
                predicateExecuted = true;
                return string.IsNullOrWhiteSpace(x) == false;
            }, x => {
                valueSelectorExecuted = true;
                return "value";
            });
            Assert.Equal("ERROR", result.Either.Error);
            Assert.Equal(default, result.Either.Value);
            Assert.False(result.Either.HasValue);
            Assert.True(result.Either.HasError);
            Assert.True(predicateExecuted);
            Assert.False(valueSelectorExecuted);
        }
    }
}