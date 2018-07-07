using System.Threading.Tasks;
using Xunit;

namespace Lemonad.ErrorHandling.Test.Outcome.Tests {
    public class MapTests {
        private static Outcome<double, string> Division(double left, double right) {
            if (right == 0)
                return Task.Run(() => $"Can not divide '{left}' with '{right}'.");

            return Task.Run(() => left / right);
        }

        [Fact]
        public async Task Result_With_Error_Maps__Expects_Selector_Never_Be_Executed() {
            var selectorExectued = false;
            var division = await Division(2, 0).Map(x => {
                selectorExectued = true;
                return x * 8;
            }).Result;

            Assert.False(selectorExectued,
                "The selector function should never get executed if there's no value in the Result<T, TError>.");
            Assert.True(division.HasError, "Result should have error.");
            Assert.False(division.HasValue, "Result should not have a value.");
            Assert.Equal(default, division.Value);
            Assert.Equal("Can not divide '2' with '0'.", division.Error);
        }

        [Fact]
        public async Task Result_With_Value_Maps__Expects_Selector_Be_Executed_And_Value_To_Be_Mapped() {
            var selectorExectued = false;
            var division = await Division(10, 2).Map(x => {
                selectorExectued = true;
                return x * 4;
            }).Result;

            Assert.True(selectorExectued, "The selector function should get executed since the result has value.");
            Assert.False(division.HasError, "Result not should have error.");
            Assert.True(division.HasValue, "Result should have a value.");
            Assert.Equal(20d, division.Value);
            Assert.Equal(default, division.Error);
        }
    }
}