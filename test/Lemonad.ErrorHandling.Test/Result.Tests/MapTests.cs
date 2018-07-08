using System;
using Xunit;

namespace Lemonad.ErrorHandling.Test.Result.Tests
{
    public class MapTests
    {
        private static Result<double, string> Division(double left, double right)
        {
            if (right == 0)
                return $"Can not divide '{left}' with '{right}'.";

            return left / right;
        }

        [Fact]
        public void Result_With_Error_Maps__Expects_Selector_Never_Be_Executed()
        {
            var selectorExectued = false;
            var division = Division(2, 0).Map(x =>
            {
                selectorExectued = true;
                return x * 8;
            });
            
            Assert.False(selectorExectued, "The selector function should never get executed if there's no value in the Result<T, TError>.");
            Assert.True(division.HasError, "Result should have error.");
            Assert.False(division.HasValue, "Result should not have a value.");
            Assert.Equal(default(double), division.Value);
            Assert.Equal("Can not divide '2' with '0'.", division.Error);
        }
        
        [Fact]
        public void Result_With_Value_Maps__Expects_Selector_Be_Executed_And_Value_To_Be_Mapped()
        {
            var selectorExectued = false;
            var division = Division(10, 2).Map(x =>
            {
                selectorExectued = true;
                return x * 4;
            });
            
            Assert.True(selectorExectued, "The selector function should get executed since the result has value.");
            Assert.False(division.HasError, "Result not should have error.");
            Assert.True(division.HasValue, "Result should have a value.");
            Assert.Equal(20d, division.Value);
            Assert.Equal(default(string), division.Error);
        }
    }
}