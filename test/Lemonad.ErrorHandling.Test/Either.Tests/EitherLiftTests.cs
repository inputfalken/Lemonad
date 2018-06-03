using Xunit;

namespace Lemonad.ErrorHandling.Test.Either.Tests {
    public class EitherLiftTests {
        [Fact]
        public void Creating_Either_From_String_With_Content__Expects_Either_To_Be_Right() {
            var either = "hello".ToEither("ERROR");

            Assert.True(either.IsRight, "Either should be right since no filterings is done");
            Assert.Equal("hello", either.Right);
            Assert.Equal(default(string), either.Left);
        }
        
        [Fact]
        public void Creating_Either_From_Null_String_With__Expects_Either_To_Be_Left() {
            string input = null;
            var either = input.ToEither("ERROR");

            Assert.True(either.IsLeft, "Either should be left since the string is null does not have a value,");
            Assert.Equal("ERROR", either.Left);
            Assert.Equal(default(string), either.Right);
        }

        [Fact]
        public void Creating_Either_From_Maybe_String_Without_Value__Expects_Either_To_Be_Left() {
            var either = "hello".SomeWhen(s => false).ToEither("ERROR");
            
            Assert.True(either.IsLeft, "Either should be left since maybe does not have a value,");
            Assert.Equal("ERROR", either.Left);
            Assert.Equal(default(string), either.Right);
        }
    }
}