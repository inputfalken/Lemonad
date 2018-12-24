using System.Collections.Generic;
using System.Linq;
using Assertion;
using Lemonad.ErrorHandling.Extensions.Result.Enumerable;
using Xunit;

namespace Lemonad.ErrorHandling.Unit.Result.Tests {
    public class ResultEnumerableExtensionsTests {
        [Fact]
        public void Getting_Oks_And_errors_From_IEnumerable_Of_Results() {
            var results = new List<IResult<double, string>> {
                AssertionUtilities.Division(4, 2),
                AssertionUtilities.Division(3, 0),
                AssertionUtilities.Division(3, 3),
                AssertionUtilities.Division(4, 0),
                AssertionUtilities.Division(5, 0),
                AssertionUtilities.Division(6, 0),
                AssertionUtilities.Division(7, 0),
                AssertionUtilities.Division(8, 0),
                AssertionUtilities.Division(10, 2)
            };
            var list = results.Values().ToList();

            Assert.Equal(6, results.Errors().Count());
            Assert.Equal(2, list[0]);
            Assert.Equal(1, list[1]);
            Assert.Equal(5, list[2]);
        }
    }
}