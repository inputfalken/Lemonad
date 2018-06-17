using System;
using Xunit;

namespace Lemonad.ErrorHandling.Test.Result.Tests {
    public class ToResultTests {
        [Fact]
        public void Convert_Maybe_Int_Whose_Property_HasValue_Is_False__Expects_Result_With_error_Value() {
            var either = 2.None().ToResult(() => "ERROR");

            Assert.False(either.HasValue, "Result should not have a right value.");
            Assert.True(either.HasError, "Result should have a error value.");
            Assert.Equal(default(int), either.Value);
            Assert.Equal("ERROR", either.Error);
        }

        [Fact]
        public void
            Convert_Maybe_Int_Whose_Property_HasValue_Is_False_Pass_Null_error_Selector__Expects_ArgumentNullException_Thrown_Thrown() {
            Assert.Throws<ArgumentNullException>(() => {
                Func<int> x = null;
                2.None().ToResult(x);
            });
        }

        [Fact]
        public void Convert_Maybe_Int_Whose_Property_HasValue_Is_True__Expects_Result_With_Ok_Value() {
            var either = 2.Some().ToResult(() => "ERROR");

            Assert.True(either.HasValue, "Result should have a right value.");
            Assert.False(either.HasError, "Result should not have a error value.");
            Assert.Equal(2, either.Value);
            Assert.Equal(default(string), either.Error);
        }

        [Fact]
        public void
            Convert_Maybe_Int_Whose_Property_HasValue_Is_True_Pass_Null_error_Selector__Expects_No_ArgumentNullException_Thrown() {
            var exception = Record.Exception(() => {
                Func<int> errorSelector = null;
                var either = 2.Some().ToResult(errorSelector);
                Assert.True(either.HasValue, "Result should have a right value.");
                Assert.Equal(2, either.Value);
            });
            Assert.Null(exception);
        }

        [Fact]
        public void Convert_Maybe_String_Whose_Property_HasValue_Is_False__Expects_Result_With_error_Value() {
            var either = 2.None().ToResult(() => "ERROR");

            Assert.False(either.HasValue, "Result should have a right value.");
            Assert.True(either.HasError, "Result should have a error value.");
            Assert.Equal("ERROR", either.Error);
            Assert.Equal(default(int), either.Value);
        }

        [Fact]
        public void Convert_Maybe_String_Whose_Property_HasValue_Is_True__Expects_Result_With_Ok_Value() {
            var either = 2.Some().ToResult(() => "ERROR");

            Assert.True(either.HasValue, "Result should have a right value.");
            Assert.False(either.HasError, "Result should not have a error value.");
            Assert.Equal(default(string), either.Error);
            Assert.Equal(2, either.Value);
        }

        [Fact]
        public void Convert_Maybe_String_With_Null_Whose_Property_HasValue_Is_False__Expects_Result_With_error_Value() {
            string str = null;
            var either = 2.None().ToResult(() => str);
            Assert.False(either.HasValue, "Result should not have a right value.");
            Assert.True(either.HasError, "Result should have a error value.");
            Assert.Null(either.Error);
        }

        [Fact]
        public void Convert_Maybe_String_With_Null_Whose_Property_HasValue_Is_True__Expects_Result_With_Ok_value() {
            string str = null;
            var either = 2.Some().ToResult(() => str);
            Assert.True(either.HasValue, "Result should have a right value.");
            Assert.False(either.HasError, "Result should not have a error value.");
            Assert.Equal(2, either.Value);
            Assert.Equal(default(string), either.Error);
        }

        [Fact]
        public void Convert_Nullable_Int_Whose_Property_HasValue_Is_False__Expects_Result_With_error_Value() {
            int? number = null;
            var either = number.ToResult(() => "ERROR");

            Assert.False(either.HasValue, "Result should not have a right value.");
            Assert.True(either.HasError, "Result should have a error value.");
            Assert.Equal(default(int), either.Value);
            Assert.Equal("ERROR", either.Error);
        }

        [Fact]
        public void
            Convert_Nullable_Int_Whose_Property_HasValue_Is_False_Pass_Null_error_Selector__Expects_ArgumentNullException_Thrown() {
            Assert.Throws<ArgumentNullException>(() => {
                Func<int> errorSelector = null;
                int? nullable = null;
                nullable.ToResult(errorSelector);
            });
        }

        [Fact]
        public void Convert_Nullable_Int_Whose_Property_HasValue_Is_True__Expects_Result_With_Ok_Value() {
            int? number = 2;
            var either = number.ToResult(() => "ERROR");

            Assert.True(either.HasValue, "Result should have a right value.");
            Assert.False(either.HasError, "Result should not have a error value.");
            Assert.Equal(2, either.Value);
            Assert.Equal(default(string), either.Error);
        }

        [Fact]
        public void
            Convert_Nullable_Int_Whose_Property_HasValue_Is_True_Pass_Null_error_Selector__Expects_No_ArgumentNullException_Thrown() {
            var exception = Record.Exception(() => {
                Func<int> errorSelector = null;
                int? nullable = 2;
                var either = nullable.ToResult(errorSelector);
                Assert.True(either.HasValue, "Result should have a right value.");
                Assert.Equal(2, either.Value);
            });
            Assert.Null(exception);
        }
    }
}
