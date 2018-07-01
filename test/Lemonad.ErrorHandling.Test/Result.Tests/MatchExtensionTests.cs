using Xunit;

namespace Lemonad.ErrorHandling.Test.Result.Tests {
    public class MatchExtensionTests {
        private static Result<double, double> Division(double left, double right) {
            return right == 0
                ? ErrorHandling.Result.Error<double, double>(-1)
                : ErrorHandling.Result.Ok<double, double>(left / right);
        }

        [Fact]
        public void Result_With_Error() {
            var result = Division(10, 0).Match();
            Assert.Equal(-1, result);
        }
        
        [Fact]
        public void Result_With_Value() {
            var result = Division(10, 2).Match();
            Assert.Equal(5, result);
        }
    }
}