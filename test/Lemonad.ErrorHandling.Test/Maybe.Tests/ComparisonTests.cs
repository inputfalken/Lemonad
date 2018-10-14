using Lemonad.ErrorHandling.Internal;
using Xunit;

namespace Lemonad.ErrorHandling.Test.Maybe.Tests {
    public class ComparisonTests {
        [Fact]
        public void Comparing_Some_With_Not_Same_Int_Value__Expects_Equality_To_be_False() {
            var intFirst = 2.ToMaybe();
            var intSecond = 3.ToMaybe();

            Assert.NotEqual(intFirst, intSecond);
            Assert.Equal(2, intFirst.Value);
            Assert.Equal(3, intSecond.Value);
        }

        [Fact]
        public void Comparing_Some_With_Not_Same_String_Value__Expects_Equality_To_be_False() {
            var stringFirst = "p".ToMaybe();
            var stringSecond = "f".ToMaybe();

            Assert.NotEqual(stringFirst, stringSecond);
            Assert.Equal("p", stringFirst.Value);
            Assert.Equal("f", stringSecond.Value);
        }

        [Fact]
        public void Comparing_Some_With_Same_Int_Value__Expects_Equality_To_be_False() {
            var intFirst = 3.ToMaybe();
            var intSecond = 3.ToMaybe();

            Assert.Equal(intFirst, intSecond);
            Assert.Equal(3, intFirst.Value);
            Assert.Equal(3, intSecond.Value);
        }

        [Fact]
        public void Comparing_Some_With_Same_String_Value__Expects_Equality_To_be_True() {
            var stringFirst = "f".ToMaybe();
            var stringSecond = "f".ToMaybe();

            Assert.Equal(stringFirst, stringSecond);
            Assert.Equal("f", stringFirst.Value);
            Assert.Equal("f", stringSecond.Value);
        }

        [Fact]
        public void Noones_Are_Expected_To_Be_Equal() {
            var first = ErrorHandling.Maybe.ToMaybeNone<string>();
            var second = ErrorHandling.Maybe.ToMaybeNone<string>();

            Assert.Equal(first, second);
            Assert.Equal(first, Maybe<string>.None);
            Assert.Equal(second, Maybe<string>.None);
        }
    }
}