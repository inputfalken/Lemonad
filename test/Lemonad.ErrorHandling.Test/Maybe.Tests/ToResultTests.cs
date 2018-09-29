using System;
using Xunit;

namespace Lemonad.ErrorHandling.Test.Maybe.Tests {
    public class ToResultTests {
        [Fact]
        public void Convert_Maybe_Int_Whose_Property_HasValue_Is_False__Expects_Result_With_error_Value() {
            var result = 2.ToMaybeNone().ToResult(x => "ERROR");

            Assert.False(result.Either.HasValue, "Result should have error.");
            Assert.True(result.Either.HasError, "Result should have a error value.");
            Assert.Equal(default, result.Either.Value);
            Assert.Equal("ERROR", result.Either.Error);
        }

        [Fact]
        public void
            Convert_Maybe_Int_Whose_Property_HasValue_Is_False_Pass_Null_error_Selector__Expects_ArgumentNullException_Thrown_Thrown() {
            Assert.Throws<ArgumentNullException>(() => {
                Func<Maybe<int>, int> x = null;
                2.ToMaybeNone().ToResult(x);
            });
        }

        [Fact]
        public void Convert_Maybe_Int_Whose_Property_HasValue_Is_True__Expects_Result_With_Ok_Value() {
            var result = 2.ToMaybe().ToResult(x => "ERROR");

            Assert.True(result.Either.HasValue, "Result should have value.");
            Assert.False(result.Either.HasError, "Result should not have a error value.");
            Assert.Equal(2, result.Either.Value);
            Assert.Equal(default, result.Either.Error);
        }

        [Fact]
        public void
            Convert_Maybe_Int_Whose_Property_HasValue_Is_True_Pass_Null_error_Selector__Expects_No_ArgumentNullException_Thrown() {
            var exception = Record.Exception(() => {
                Func<Maybe<int>, int> errorSelector = null;
                var result = 2.ToMaybe().ToResult(errorSelector);
                Assert.True(result.Either.HasValue, "Result should have value.");
                Assert.Equal(2, result.Either.Value);
            });
            Assert.Null(exception);
        }

        [Fact]
        public void Convert_Maybe_String_Whose_Property_HasValue_Is_False__Expects_Result_With_error_Value() {
            var result = 2.ToMaybeNone().ToResult(x => "ERROR");

            Assert.False(result.Either.HasValue, "Result should have value.");
            Assert.True(result.Either.HasError, "Result should have a error value.");
            Assert.Equal("ERROR", result.Either.Error);
            Assert.Equal(default, result.Either.Value);
        }

        [Fact]
        public void Convert_Maybe_String_Whose_Property_HasValue_Is_True__Expects_Result_With_Ok_Value() {
            var result = 2.ToMaybe().ToResult(x => "ERROR");

            Assert.True(result.Either.HasValue, "Result should have value.");
            Assert.False(result.Either.HasError, "Result should not have a error value.");
            Assert.Equal(default, result.Either.Error);
            Assert.Equal(2, result.Either.Value);
        }

        [Fact]
        public void Convert_Maybe_String_With_Null_Whose_Property_HasValue_Is_False__Expects_Result_With_error_Value() {
            Assert.Throws<ArgumentNullException>(AssertionUtilities.EitherErrorName,() => {
                string str = null;
                var result = 2.ToMaybeNone().ToResult(x => str);
                Assert.False(result.Either.HasValue, "Result should have error.");
                Assert.True(result.Either.HasError, "Result should have a error value.");
                Assert.Null(result.Either.Error);
            });
        }

        [Fact]
        public void Convert_Maybe_String_With_Null_Whose_Property_HasValue_Is_True__Expects_Result_With_Ok_value() {
            string str = null;
            var result = 2.ToMaybe().ToResult(x => str);
            Assert.True(result.Either.HasValue, "Result should have value.");
            Assert.False(result.Either.HasError, "Result should not have a error value.");
            Assert.Equal(2, result.Either.Value);
            Assert.Equal(default, result.Either.Error);
        }

        [Fact]
        public void Convert_Nullable_Int_Whose_Property_HasValue_Is_False__Expects_Result_With_error_Value() {
            int? number = null;
            var result = number.ToResult(x => "ERROR");

            Assert.False(result.Either.HasValue, "Result should have error.");
            Assert.True(result.Either.HasError, "Result should have a error value.");
            Assert.Equal(default, result.Either.Value);
            Assert.Equal("ERROR", result.Either.Error);
        }

        [Fact]
        public void
            Convert_Nullable_Int_Whose_Property_HasValue_Is_False_Pass_Null_error_Selector__Expects_ArgumentNullException_Thrown() {
            Assert.Throws<ArgumentNullException>(() => {
                Func<Maybe<int>, int> errorSelector = null;
                int? nullable = null;
                nullable.ToResult(errorSelector);
            });
        }

        [Fact]
        public void Convert_Nullable_Int_Whose_Property_HasValue_Is_True__Expects_Result_With_Ok_Value() {
            int? number = 2;
            var result = number.ToResult(x => "ERROR");

            Assert.True(result.Either.HasValue, "Result should have value.");
            Assert.False(result.Either.HasError, "Result should not have a error value.");
            Assert.Equal(2, result.Either.Value);
            Assert.Equal(default, result.Either.Error);
        }

        [Fact]
        public void
            Convert_Nullable_Int_Whose_Property_HasValue_Is_True_Pass_Null_error_Selector__Expects_No_ArgumentNullException_Thrown() {
            var exception = Record.Exception(() => {
                Func<Maybe<int>, int> errorSelector = null;
                int? nullable = 2;
                var result = nullable.ToResult(errorSelector);
                Assert.True(result.Either.HasValue, "Result should have value.");
                Assert.Equal(2, result.Either.Value);
            });
            Assert.Null(exception);
        }
    }
}