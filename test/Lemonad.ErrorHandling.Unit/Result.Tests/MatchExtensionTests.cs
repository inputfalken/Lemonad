﻿using Assertion;
using Lemonad.ErrorHandling.Extensions.Result;
using Xunit;

namespace Lemonad.ErrorHandling.Unit.Result.Tests {
    public class MatchExtensionTests {
        private static double Division(double left, double second) =>
            AssertionUtilities.Division(left, second).MapError(x => -1d).Match();

        [Fact]
        public void Result_With_Error() {
            var result = Division(10, 0);
            Assert.Equal(-1, result);
        }

        [Fact]
        public void Result_With_Error_With_Selector() {
            var result = Division(10, 0);
            Assert.Equal(-1, result);
        }

        [Fact]
        public void Result_With_Value() {
            var result = Division(10, 2);
            Assert.Equal(5, result);
        }

        [Fact]
        public void Result_With_Value_With_Selector() {
            var result = Division(10, 2);
            Assert.Equal(5, result);
        }
    }
}