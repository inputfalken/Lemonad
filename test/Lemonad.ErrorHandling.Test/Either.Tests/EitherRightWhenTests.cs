using System;
using System.ComponentModel;
using Xunit;

namespace Lemonad.ErrorHandling.Test.Either.Tests {
    public class EitherRightWhenTests {
        [Fact]
        [Description(
            "Since int32? is a nullable type and the predicate is false; but is excplicitly set an exception will not be thrown.")]
        public void Explicitly_Set_Nullable_Int_String_Either_With_Falsy_Predicate__Expects__No_Exception() {
            var exception = Record.Exception(() => "Hello".ToEitherRight<int?, string>().RightWhen(s => false, 1));
            Assert.Null(exception);
        }

        [Fact]
        [Description(
            "Since int32? is a nullable type and the predicate is false; but is excplicitly set an exception will not be thrown.")]
        public void
            Explicitly_Set_Nullable_Int_String_Either_With_Falsy_Predicate_With_Chained_RightWhen__Expects__No_Exception() {
            var exception = Record.Exception(() => {
                var either = "Hello".ToEitherRight<int?, string>()
                    .RightWhen(s => false, 1)
                    .RightWhen(s => false);
                Assert.True(either.IsLeft, "Should have a right value.");
                Assert.Equal(1, either.Left);
            });
            Assert.Null(exception);
        }

        [Fact]
        [Description("Since the predicate is true and we have a right value. No exception will be thrown.")]
        public void Explicitly_Set_Nullable_Int_String_Either_With_Truthy_Predicate__Expects__No_Exception() {
            var exception = Record.Exception(() => {
                var either = "Hello".ToEitherRight<int?, string>().RightWhen(s => true, 1);
                Assert.True(either.IsRight, "Should have a right value.");
                Assert.Equal("Hello", either.Right);
            });
            Assert.Null(exception);
        }

        [Fact]
        [Description(
            "Since int32 is not a nullable type; it will not throw an exception even if the predicate is false.")]
        public void Int_String_Either_With_Falsy_Predicate__Expects__Expects_Exception() {
            var exception = Record.Exception(() => "Hello".ToEitherRight<int, string>().RightWhen(s => false));
            Assert.Null(exception);
        }

        [Fact]
        [Description("Since the predicate is true and we have a right value. No exception will be thrown.")]
        public void Int_String_Either_With_Truthy_Predicate__Expects__Expects_No_Exception() {
            var exception = Record.Exception(() => {
                var either = "Hello".ToEitherRight<int, string>().RightWhen(s => true);
                Assert.True(either.IsRight, "Should have a right value.");
                Assert.Equal("Hello", either.Right);
            });
            Assert.Null(exception);
        }

        [Fact]
        [Description("Since int32? is a nullable type and the predicate is false; it will throw an exception.")]
        public void Nullable_Int_String_Either_With_Falsy_Predicate__Expects__Exception() {
            Assert.Throws<ArgumentException>(() => "Hello".ToEitherRight<int?, string>().RightWhen(s => false));
        }

        [Fact]
        [Description("Since the predicate is true and we have a right value. No exception will be thrown.")]
        public void Nullable_Int_String_Either_With_Truthy_Predicate__Expects__Exception() {
            var exception = Record.Exception(() => {
                var either = "Hello".ToEitherRight<int?, string>().RightWhen(s => true);
                Assert.True(either.IsRight, "Should have a right value.");
                Assert.Equal("Hello", either.Right);
            });
            Assert.Null(exception);
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
        public void Provide_Left_Overload__String_String_Either_With_Truthy_Predicate__Expects_Right_Either() {
            var either = "Hello".ToEitherRight<string, string>()
                .RightWhen(s => true, "ERROR");

            Assert.True(either.IsRight, "Either should have a right value since the predicate is true.");
            Assert.Equal("Hello", either.Right);
            Assert.Equal(default(string), either.Left);
        }

        [Fact]
        public void String_String_Either_With_Falsy_Predicate__Expects_Exception() {
            Assert.Throws<ArgumentException>(() => {
                "Hello".ToEitherRight<string, string>()
                    .RightWhen(s => false);
            });
        }

        [Fact]
        public void String_String_Either_With_Truthy_Predicate__Expects_No_Exception() {
            var exception = Record.Exception(() => {
                var either = "Hello".ToEitherRight<string, string>()
                    .RightWhen(s => true);
                Assert.True(either.IsRight, "Should have a right value.");
                Assert.Equal("Hello", either.Right);
            });
            Assert.Null(exception);
        }
    }
}