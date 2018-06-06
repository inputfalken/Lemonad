using System.Collections.Generic;
using System.Linq;
using Xunit;

namespace Lemonad.ErrorHandling.Test.Maybe.Tests {
    public class MaybeEnumerableExtensionTests {
        [Fact]
        public void Getting_ValueIEnumerable_Of_Maybes() {
            Maybe<int> Divide(int left, int right) {
                if (right != 0)
                    return left / right;
                return ErrorHandling.Maybe.None<int>();
            }

            var maybes = new List<Maybe<int>> {
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

            var list = maybes.GetMaybeValues().ToList();
            Assert.Equal(2, list[0]);
            Assert.Equal(1, list[1]);
            Assert.Equal(5, list[2]);
        }
    }
}