using Xunit;

namespace Lemonad.ErrorHandling.Test.Either.Tests {
    public class FlatMapResultSelectorTests {
        [Fact]
        public void
            Either_String_Int_Whose_Property_IsRight_Is_True__FLatmaps_Either_String_Double_Whose_Property_IsRight_Is_True__Expects_String_Double_Whose_Property_IsRight_Is_True() {
            var intParse = ErrorHandling.Either.Parse.Int("2");
            var doubleParse = ErrorHandling.Either.Parse.Double("2.2");

            var result = intParse.FlatMap(x => doubleParse, (i, d) => i + d);
            Assert.True(result.IsRight, "Either should have a right value.");
            Assert.False(result.IsLeft, "Either should not have a left value.");
            Assert.Equal(default(string), result.Left);
            Assert.Equal(4.2, result.Right);
        }
        
        [Fact]
        public void
            Either_String_Int_Whose_Property_IsRight_Is_False__FLatmaps_Either_String_Double_Whose_Property_IsRight_Is_True__Expects_String_Double_Whose_Property_IsRight_Is_True() {
            var intParse = ErrorHandling.Either.Parse.Int("foo");
            var doubleParse = ErrorHandling.Either.Parse.Double("2.2");

            var result = intParse.FlatMap(x => doubleParse, (i, d) => i + d);
            Assert.True(result.IsRight, "Either should have a right value.");
            Assert.False(result.IsLeft, "Either should not have a left value.");
            Assert.Equal(default(string), result.Left);
            Assert.Equal(4.2, result.Right);
        }
        
        [Fact]
        public void
            Either_String_Int_Whose_Property_IsRight_Is_False__FLatmaps_Either_String_Double_Whose_Property_IsRight_Is_False__Expects_String_Double_Whose_Property_IsRight_Is_True() {
            var intParse = ErrorHandling.Either.Parse.Int("foo");
            var doubleParse = ErrorHandling.Either.Parse.Double("2.2");

            var result = intParse.FlatMap(x => doubleParse, (i, d) => i + d);
            Assert.True(result.IsRight, "Either should have a right value.");
            Assert.False(result.IsLeft, "Either should not have a left value.");
            Assert.Equal(default(string), result.Left);
            Assert.Equal(4.2, result.Right);
        }
        
        [Fact]
        public void
            Either_String_Int_Whose_Property_IsRight_Is_True__FLatmaps_Either_String_Double_Whose_Property_IsRight_Is_False__Expects_String_Double_Whose_Property_IsRight_Is_True() {
            var intParse = ErrorHandling.Either.Parse.Int("foo");
            var doubleParse = ErrorHandling.Either.Parse.Double("2.2");

            var result = intParse.FlatMap(x => doubleParse, (i, d) => i + d);
            Assert.True(result.IsRight, "Either should have a right value.");
            Assert.False(result.IsLeft, "Either should not have a left value.");
            Assert.Equal(default(string), result.Left);
            Assert.Equal(4.2, result.Right);
        }
    }
}