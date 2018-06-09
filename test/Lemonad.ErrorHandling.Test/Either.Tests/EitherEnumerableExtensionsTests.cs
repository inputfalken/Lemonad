using System.Collections.Generic;
using System.Linq;
using Xunit;

namespace Lemonad.ErrorHandling.Test.Either.Tests {
    public class EitherEnumerableExtensionsTests {
        [Fact]
        public void Getting_Rights_And_Lefts_From_IEnumerable_Of_Eithers() {
            Either<string, int> Divide(int left, int right) =>
                right != 0 ? (Either<string, int>) (left / right) : "Cannot divide with zero";

            var eithers = new List<Either<string, int>> {
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
            var list = eithers.EitherRights().ToList();

            Assert.All(eithers.EitherLefts(), s => { Assert.Equal("Cannot divide with zero", s); });
            Assert.Equal(2, list[0]);
            Assert.Equal(1, list[1]);
            Assert.Equal(5, list[2]);
        }
    }
}