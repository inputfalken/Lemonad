using System.Collections.Generic;
using System.Linq;
using Xunit;

namespace Lemonad.ErrorHandling.Test.Result.Tests {
    public class ResultEnumerableExtensionsTests {
        [Fact]
        public void Getting_Oks_And_errors_From_IEnumerable_Of_Results() {
            Result<int, string> Divide(int error, int right) {
                if (right != 0)
                    return error / right;
                else
                    return "Cannot divide with zero";
            }

            var eithers = new List<Result<int, string>> {
                Divide(4, 2),
                Divide(3, 0),
                Divide(3, 3),
                Divide(4, 0),
                Divide(5, 0),
                Divide(6, 0),
                Divide(7, 0),
                Divide(8, 0),
                Divide(10, 2)
            };
            var list = eithers.Values().ToList();

            Assert.All(eithers.ErrorValues(), s => { Assert.Equal("Cannot divide with zero", s); });
            Assert.Equal(2, list[0]);
            Assert.Equal(1, list[1]);
            Assert.Equal(5, list[2]);
        }
    }
}