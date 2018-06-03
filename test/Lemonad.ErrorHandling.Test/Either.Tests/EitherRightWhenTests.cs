using System;
using Xunit;

namespace Lemonad.ErrorHandling.Test.Either.Tests {
    public class EitherRightWhenTests {
        [Fact]
        public void Provide_Left_Overload__String_String_Either_With_Truthy_Predicate__Expects_Right_Either() {
            var either = "Hello".ToEitherRight<string, string>()
                .RightWhen(s => true, "ERROR");

            Assert.True(either.IsRight, "Either should have a right value since the predicate is true.");
            Assert.Equal("Hello", either.Right);
            Assert.Equal(default(string), either.Left);
        }

        [Fact]
        public void Provide_Left_Overload__String_String_Either_With_Falsy_Predicate__Expects_Left_Either() {
            var either = "Hello".ToEitherRight<string, string>()
                .RightWhen(s => false, "ERROR");

            Assert.True(either.IsLeft, "Either should have a left value since the predicate is false.");
            Assert.Equal("ERROR", either.Left);
            Assert.Equal(default(string), either.Right);
        }

        [Fact]
        public void String_String_Either_With_Falsy_Predicate__Expects_Left_Either() {
            Assert.Throws<ArgumentException>(() => {
                "Hello".ToEitherRight<string, string>()
                    .RightWhen(s => false);
            });
        }

        [Fact]
        public void Int_String_Either_With_Falsy_Predicate__Expects_Left_Either() {
            Assert.Throws<ArgumentException>(() => {
                "Hello".ToEitherRight<int, string>()
                    .RightWhen(s => false);
            });
        }
    }
}