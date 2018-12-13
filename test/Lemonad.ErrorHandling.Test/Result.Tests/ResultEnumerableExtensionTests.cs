using System.Collections.Generic;
using System.Linq;
using Lemonad.ErrorHandling.Extensions.Enumerable;
using Xunit;
using static Lemonad.ErrorHandling.Test.AssertionUtilities;

namespace Lemonad.ErrorHandling.Test.Result.Tests {
    public class ResultEnumerableExtensionsTests {
        [Fact]
        public void Getting_Oks_And_errors_From_IEnumerable_Of_Results() {
            var results = new List<IResult<double, string>> {
                Division(4, 2),
                Division(3, 0),
                Division(3, 3),
                Division(4, 0),
                Division(5, 0),
                Division(6, 0),
                Division(7, 0),
                Division(8, 0),
                Division(10, 2)
            };
            var list = results.Values().ToList();

            Assert.Equal(6, results.Errors().Count());
            Assert.Equal(2, list[0]);
            Assert.Equal(1, list[1]);
            Assert.Equal(5, list[2]);
        }
    }
}