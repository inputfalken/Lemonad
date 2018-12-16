using Xunit;

namespace Lemonad.ErrorHandling.Unit.Maybe.Tests {
    public class ComparisonTests {
        [Fact]
        public void Comparing_Some_With_Not_Same_Int_Value__Expects_Equality_To_be_False() {
            var intFirst = ErrorHandling.Maybe.Value(2);
            var intSecond = ErrorHandling.Maybe.Value(3);

            Assert.NotEqual(intFirst, intSecond);
            Assert.Equal(2, intFirst.Value);
            Assert.Equal(3, intSecond.Value);
        }

        [Fact]
        public void Comparing_Some_With_Not_Same_String_Value__Expects_Equality_To_be_False() {
            var stringFirst = ErrorHandling.Maybe.Value("p");
            var stringSecond = ErrorHandling.Maybe.Value("f");

            Assert.NotEqual(stringFirst, stringSecond);
            Assert.Equal("p", stringFirst.Value);
            Assert.Equal("f", stringSecond.Value);
        }

        [Fact]
        public void Comparing_Some_With_Same_Int_Value__Expects_Equality_To_be_False() {
            var intFirst = ErrorHandling.Maybe.Value(3);
            var intSecond = ErrorHandling.Maybe.Value(3);

            Assert.Equal(intFirst, intSecond);
            Assert.Equal(3, intFirst.Value);
            Assert.Equal(3, intSecond.Value);
        }

        [Fact]
        public void Comparing_Some_With_Same_String_Value__Expects_Equality_To_be_True() {
            var stringFirst = ErrorHandling.Maybe.Value("f");
            var stringSecond = ErrorHandling.Maybe.Value("f");

            Assert.Equal(stringFirst, stringSecond);
            Assert.Equal("f", stringFirst.Value);
            Assert.Equal("f", stringSecond.Value);
        }

        [Fact]
        public void Noones_Are_Expected_To_Be_Equal() {
            var first = ErrorHandling.Maybe.None<string>();
            var second = ErrorHandling.Maybe.None<string>();

            Assert.Equal(first, second);
            Assert.Equal(first, ErrorHandling.Maybe.None<string>());
            Assert.Equal(second, ErrorHandling.Maybe.None<string>());
        }
    }
}