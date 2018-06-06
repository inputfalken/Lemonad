using System;
using Xunit;

namespace Lemonad.ErrorHandling.Test.Either.Tests {
    public class ToEitherTests {
        [Fact]
        public void
            Convert_Maybe_Int_Whose_Property_HasValue_Is_False_Pass_Null_Left_Selector__Expects_Argument_Null_Exception() {
            Assert.Throws<ArgumentNullException>(() => {
                Func<int> x = null;
                2.None().ToEither(x);
            });
        }

        [Fact]
        public void
            Convert_Nullable_Int_Whose_Property_HasValue_Is_False_Pass_Null_Left_Selector__Expects_Argument_Null_Exception() {
            Assert.Throws<ArgumentNullException>(() => {
                Func<int> x = null;
                int? nullable = null;
                nullable.ToEither(x);
            });
        }

        [Fact]
        public void
            Convert_Maybe_Int_Whose_Property_HasValue_Is_True_Pass_Null_Left_Selector__Expects_No_Argument_Null_Exception() {
            var exception = Record.Exception(() => {
                Func<int> x = null;
                2.Some().ToEither(x);
            });
            Assert.Null(exception);
        }

        [Fact]
        public void
            Convert_Nullable_Int_Whose_Property_HasValue_Is_True_Pass_Null_Left_Selector__Expects_No_Argument_Null_Exception() {
            var exception = Record.Exception(() => {
                Func<int> x = null;
                int? nullable = 2;
                nullable.ToEither(x);
            });
            Assert.Null(exception);
        }

        [Fact]
        public void Convert_Maybe_Int_Whose_Property_HasValue_Is_False__Expects_Either_With_Left_Value() {
            var either = 2.None().ToEither(() => "ERROR");

            Assert.False(either.IsRight, "Either should not have a right value.");
            Assert.True(either.IsLeft, "Either should have a left value.");
            Assert.Equal(default(int), either.Right);
            Assert.Equal("ERROR", either.Left);
        }

        [Fact]
        public void Convert_Maybe_Int_Whose_Property_HasValue_Is_True__Expects_Either_With_Right_Value() {
            var either = 2.Some().ToEither(() => "ERROR");

            Assert.True(either.IsRight, "Either should have a right value.");
            Assert.False(either.IsLeft, "Either should not have a left value.");
            Assert.Equal(2, either.Right);
            Assert.Equal(default(string), either.Left);
        }

        [Fact]
        public void Convert_Nullable_Int_Whose_Property_HasValue_Is_False__Expects_Either_With_Left_Value() {
            int? number = null;
            var either = number.ToEither(() => "ERROR");

            Assert.False(either.IsRight, "Either should not have a right value.");
            Assert.True(either.IsLeft, "Either should have a left value.");
            Assert.Equal(default(int), either.Right);
            Assert.Equal("ERROR", either.Left);
        }

        [Fact]
        public void Convert_Nullable_Int_Whose_Property_HasValue_Is_True__Expects_Either_With_Right_Value() {
            int? number = 2;
            var either = number.ToEither(() => "ERROR");

            Assert.True(either.IsRight, "Either should have a right value.");
            Assert.False(either.IsLeft, "Either should not have a left value.");
            Assert.Equal(2, either.Right);
            Assert.Equal(default(string), either.Left);
        }

        [Fact]
        public void Convert_Maybe_String_Whose_Property_HasValue_Is_False__Expects_Either_With_Left_Value() {
            var either = 2.None().ToEither(() => "ERROR");

            Assert.False(either.IsRight, "Either should have a right value.");
            Assert.True(either.IsLeft, "Either should have a left value.");
            Assert.Equal("ERROR", either.Left);
            Assert.Equal(default(int), either.Right);
        }

        [Fact]
        public void Convert_Maybe_String_Whose_Property_HasValue_Is_True__Expects_Either_With_Right_Value() {
            var either = 2.Some().ToEither(() => "ERROR");

            Assert.True(either.IsRight, "Either should have a right value.");
            Assert.False(either.IsLeft, "Either should not have a left value.");
            Assert.Equal(default(string), either.Left);
            Assert.Equal(2, either.Right);
        }

        [Fact]
        public void Convert_Maybe_String_With_Null_Whose_Property_HasValue_Is_False__Expects_Either_With_Left_Value() {
            string str = null;
            var either = 2.None().ToEither(() => str);
            Assert.False(either.IsRight, "Either should not have a right value.");
            Assert.True(either.IsLeft, "Either should have a left value.");
            Assert.Null(either.Left);
            Assert.Equal(default(int), either.Right);
        }

        [Fact]
        public void Convert_Maybe_String_With_Null_Whose_Property_HasValue_Is_True__Expects_Either_With_Right_value() {
            string str = null;
            var either = 2.Some().ToEither(() => str);
            Assert.True(either.IsRight, "Either should have a right value.");
            Assert.False(either.IsLeft, "Either should not have a left value.");
            Assert.Equal(2, either.Right);
            Assert.Equal(default(string), either.Left);
        }
    }
}