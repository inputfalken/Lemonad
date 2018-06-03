using Xunit;

namespace Lemonad.ErrorHandling.Test.Maybe.Tests {
    public class ComparisonTests {
        [Fact]
        public void Noones_Are_Expected_To_Be_Equal() {
            var first = ErrorHandling.Maybe.None<string>();
            var second = ErrorHandling.Maybe.None<string>();

            Assert.Equal(first, second);
        }

        [Fact]
        public void Comparing_Some_With_Same_String_Value__Expects_Equality_To_be_True() {
            var stringFirst = "f".Some();
            var stringSecond = "f".Some();
            Assert.Equal(stringFirst, stringSecond);
        }

        [Fact]
        public void Comparing_Some_With_Not_Same_String_Value__Expects_Equality_To_be_False() {
            var stringFirst = "p".Some();
            var stringSecond = "f".Some();
            Assert.NotEqual(stringFirst, stringSecond);
        }

        [Fact]
        public void Comparing_Some_With_Same_Int_Value__Expects_Equality_To_be_False() {
            var stringFirst = 3.Some();
            var stringSecond = 3.Some();
            Assert.Equal(stringFirst, stringSecond);
        }

        [Fact]
        public void Comparing_Some_With_Not_Same_Int_Value__Expects_Equality_To_be_False() {
            var stringFirst = 2.Some();
            var stringSecond = 3.Some();
            Assert.NotEqual(stringFirst, stringSecond);
        }
    }
}