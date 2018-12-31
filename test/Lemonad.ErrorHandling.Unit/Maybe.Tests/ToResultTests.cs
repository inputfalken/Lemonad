using System;
using Assertion;
using Lemonad.ErrorHandling.Extensions;
using Lemonad.ErrorHandling.Extensions.Maybe;
using Xunit;

namespace Lemonad.ErrorHandling.Unit.Maybe.Tests {
    public class ToResultTests {
        [Fact]
        public void Convert_Maybe_Int_Whose_Property_HasValue_Is_False__Expects_Result_With_error_Value() =>
            2.ToMaybeNone().ToResult(() => "ERROR").AssertError("ERROR");

        [Fact]
        public void
            Convert_Maybe_Int_Whose_Property_HasValue_Is_False_Pass_Null_error_Selector__Expects_ArgumentNullException_Thrown_Thrown() {
            Assert.Throws<ArgumentNullException>(() => {
                Func<int> x = null;
                2.ToMaybeNone().ToResult(x);
            });
        }

        [Fact]
        public void Convert_Maybe_Int_Whose_Property_HasValue_Is_True__Expects_Result_With_Ok_Value() =>
            ErrorHandling.Maybe.Value(2).ToResult(() => "ERROR").AssertValue(2);

        [Fact]
        public void Convert_Maybe_String_Whose_Property_HasValue_Is_False__Expects_Result_With_error_Value() =>
            2.ToMaybeNone().ToResult(() => "ERROR").AssertError("ERROR");

        [Fact]
        public void Convert_Maybe_String_Whose_Property_HasValue_Is_True__Expects_Result_With_Ok_Value() =>
            ErrorHandling.Maybe.Value(2).ToResult(() => "ERROR").AssertValue(2);

        [Fact]
        public void Convert_Maybe_String_With_Null_Whose_Property_HasValue_Is_False__Expects_Result_With_error_Value() {
            Assert.Throws<ArgumentNullException>(AssertionUtilities.ErrorParamName, () => {
                string str = null;
                2.ToMaybeNone().ToResult(() => str);
            });
        }

        [Fact]
        public void Convert_Maybe_String_With_Null_Whose_Property_HasValue_Is_True__Expects_Result_With_Ok_value() {
            string str = null;
            ErrorHandling.Maybe.Value(2).ToResult(() => str).AssertValue(2);
        }

        [Fact]
        public void Convert_Nullable_Int_Whose_Property_HasValue_Is_False__Expects_Result_With_error_Value() {
            int? number = null;
            number.ToResult(() => "ERROR").AssertError("ERROR");
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
            number.ToResult(() => "ERROR").AssertValue(2);
        }

        [Fact]
        public void
            Convert_Nullable_Int_Whose_Property_HasValue_Is_True_Pass_Null_error_Selector__Expects_No_ArgumentNullException_Thrown() {
            var exception = Record.Exception(() => {
                Func<int> errorSelector = null;
                int? nullable = 2;
                nullable.ToResult(errorSelector).AssertValue(2);
            });
            Assert.Null(exception);
        }
    }
}