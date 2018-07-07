using Lemonad.ErrorHandling.DataTypes.Result;
using Xunit;

namespace Lemonad.ErrorHandling.Test.Result.Tests {
    public class OkWhenTests {
        private static Result<double, string> Division(double left, double right) {
            if (right == 0)
                return $"Can not divide '{left}' with '{right}'.";

            return left / right;
        }

        [Fact]
        public void
            Result_With_Error__Expects_Predicate_Never_To_Be_Executed_And_ErrorSelector_Never_To_Be_Invoked() {
            var predicateExectued = false;
            var errorSelectorExectued = false;
            var result = Division(10, 0).Filter(d => {
                predicateExectued = true;
                return d == 2;
            }, () => {
                errorSelectorExectued = true;
                return "Bad";
            });

            Assert.False(predicateExectued,
                "Should not get exectued since there's an error before the predicate was applied.");
            Assert.False(errorSelectorExectued,
                "Should not get exectued since there's an error before the predicate was applied.");
            Assert.Equal(default, result.Value);
            Assert.Equal("Can not divide '10' with '0'.", result.Error);
            Assert.True(result.HasError, "Result should have error.");
            Assert.False(result.HasValue, "Result should not have value.");
        }

        [Fact]
        public void
            Result_With_Value_With_Falsy_Predicate__Expects_Predicate_To_Be_Executed_And_ErrorSelector_To_Be_Invoked() {
            var predicateExectued = false;
            var errorSelectorExectued = false;
            var result = Division(10, 2).Filter(d => {
                predicateExectued = true;
                return false;
            }, () => {
                errorSelectorExectued = true;
                return "Bad";
            });

            Assert.True(predicateExectued, "Should get exectued since there's a value from the result.");
            Assert.True(errorSelectorExectued, "Should get exectued since the predicate was truthy.");
            Assert.Equal(default, result.Value);
            Assert.Equal("Bad", result.Error);
            Assert.True(result.HasError, "Result should have error.");
            Assert.False(result.HasValue, "Result should not have value.");
        }

        [Fact]
        public void
            Result_With_Value_With_Truthy_Predicate__Expects_Predicate_To_Be_Executed_And_ErrorSelector_To_Never_Be_Invoked() {
            var predicateExectued = false;
            var errorSelectorExectued = false;
            var result = Division(10, 2).Filter(d => {
                predicateExectued = true;
                return true;
            }, () => {
                errorSelectorExectued = true;
                return "Bad";
            });

            Assert.True(predicateExectued,
                "Should get exectued since there's a value from the result.");
            Assert.False(errorSelectorExectued,
                "Should not get exectued since the predicate was falsy.");
            Assert.Equal(5, result.Value);
            Assert.Equal(default, result.Error);
            Assert.False(result.HasError, "Result should not have error.");
            Assert.True(result.HasValue, "Result should have value.");
        }
    }
}