using System.Linq;
using Xunit;

namespace Lemonad.ErrorHandling.Test.Maybe.Tests {
    public class SelectManyTests {
        [Fact]
        public void Enumerable_ResultSelector_SelectMany_FlatMap_Maybe_With_Value() {
            const string input = "hello";
            var list = Enumerable
                .Range(0, 3)
                .SelectMany(_ => input.NoneWhenStringIsNullOrEmpty(), (x, y) => x + y.Length)
                .ToList();

            Assert.Equal(5, list[0]);
            Assert.Equal(6, list[1]);
            Assert.Equal(7, list[2]);
        }
        
        [Fact]
        public void Enumerable_ResultSelector_SelectMany_FlatMap_Maybe_Without_Value() {
            const string input = "hello";
            var list = Enumerable
                .Range(0, 3)
                .SelectMany(_ => input.SomeWhen(y => y.Length > 10), (x, y) => x + y.Length)
                .ToList();

            Assert.False(list.Any());
        }
    }
}