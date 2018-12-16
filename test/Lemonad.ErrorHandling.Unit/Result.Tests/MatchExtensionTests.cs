using Lemonad.ErrorHandling.Extensions.Result;
using Xunit;

namespace Lemonad.ErrorHandling.Unit.Result.Tests {
    public class MatchExtensionTests {
        private static IResult<double, double> Division(double left, double right) => right == 0
            ? ErrorHandling.Result.Error<double, double>(-1)
            : ErrorHandling.Result.Value<double, double>(left / right);

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