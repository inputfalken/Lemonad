using Lemonad.ErrorHandling.Extensions;
using Xunit;

namespace Lemonad.ErrorHandling.Test.Result.Tests {
    public class MatchExtensionTests {
        private static Result<double, double> Division(double left, double right) => right == 0
            ? ResultExtensions.Error<double, double>(-1)
            : ResultExtensions.Ok<double, double>(left / right);

        [Fact]
        public void Result_With_Error() {
            var result = Division(10, 0).Match();
            Assert.Equal(-1, result);
        }

        [Fact]
        public void Result_With_Error_With_Selector() {
            var result = Division(10, 0).Match(d => d * 2);
            Assert.Equal(-2, result);
        }

        [Fact]
        public void Result_With_Value() {
            var result = Division(10, 2).Match();
            Assert.Equal(5, result);
        }

        [Fact]
        public void Result_With_Value_With_Selector() {
            var result = Division(10, 2).Match(d => d * 2);
            Assert.Equal(10, result);
        }
    }
}
