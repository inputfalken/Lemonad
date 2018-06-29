using Xunit;

namespace Lemonad.ErrorHandling.Test.Result.Tests {
    public class MapErrorTests {
        private static Result<double, string> Division(double left, double right) {
            if (right == 0)
                return $"Can not divide '{left}' with '{right}'.";

            return left / right;
        }

        [Fact]
        public void Result_With_Error__Expects_Error_To_Be_Mapped() {
            var errorSelectorInvoked = false;
            var result = Division(10, 0).MapError(s => {
                errorSelectorInvoked = true;
                return s.ToUpper();
            });

            Assert.True(errorSelectorInvoked,
                "Errorselector should get exeuted since there is an error in the result.");
            Assert.False(result.HasValue, "Result should not have a value.");
            Assert.True(result.HasError, "Result should have a error.");
            Assert.Equal(default(double), result.Value);
            Assert.Equal("CAN NOT DIVIDE '10' WITH '0'.", result.Error);
        }

        [Fact]
        public void Result_With_Value__Expects_Error_To_Not_Be_Mapped() {
            var errorSelectorInvoked = false;
            var result = Division(10, 2).MapError(s => {
                errorSelectorInvoked = true;
                return s.ToUpper();
            });

            Assert.False(errorSelectorInvoked,
                "Errorselector not should get exeuted since there is an value in the result.");
            Assert.True(result.HasValue, "Result should have a value.");
            Assert.False(result.HasError, "Result should not have a error.");
            Assert.Equal(5d, result.Value);
            Assert.Equal(default(string), result.Error);
        }
    }
}