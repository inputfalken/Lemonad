using Lemonad.ErrorHandling.DataTypes.Maybe;
using Lemonad.ErrorHandling.DataTypes.Maybe.Extensions;
using Xunit;

namespace Lemonad.ErrorHandling.Test.Maybe.Tests {
    public class ComparisonTests {
        [Fact]
        public void Comparing_Some_With_Not_Same_Int_Value__Expects_Equality_To_be_False() {
            var intFirst = 2.Some();
            var intSecond = 3.Some();

            Assert.NotEqual(intFirst, intSecond);
            Assert.Equal(2, intFirst.Value);
            Assert.Equal(3, intSecond.Value);
        }

        [Fact]
        public void Comparing_Some_With_Not_Same_String_Value__Expects_Equality_To_be_False() {
            var stringFirst = "p".Some();
            var stringSecond = "f".Some();

            Assert.NotEqual(stringFirst, stringSecond);
            Assert.Equal("p", stringFirst.Value);
            Assert.Equal("f", stringSecond.Value);
        }

        [Fact]
        public void Comparing_Some_With_Same_Int_Value__Expects_Equality_To_be_False() {
            var intFirst = 3.Some();
            var intSecond = 3.Some();

            Assert.Equal(intFirst, intSecond);
            Assert.Equal(3, intFirst.Value);
            Assert.Equal(3, intSecond.Value);
        }

        [Fact]
        public void Comparing_Some_With_Same_String_Value__Expects_Equality_To_be_True() {
            var stringFirst = "f".Some();
            var stringSecond = "f".Some();

            Assert.Equal(stringFirst, stringSecond);
            Assert.Equal("f", stringFirst.Value);
            Assert.Equal("f", stringSecond.Value);
        }

        [Fact]
        public void Noones_Are_Expected_To_Be_Equal() {
            var first = DataTypes.Maybe.Extensions.Maybe.None<string>();
            var second = DataTypes.Maybe.Extensions.Maybe.None<string>();

            Assert.Equal(first, second);
            Assert.Equal(first, Maybe<string>.None);
            Assert.Equal(second, Maybe<string>.None);
        }
    }
}