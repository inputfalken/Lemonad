using System;
using Assertion;
using Xunit;

namespace Lemonad.ErrorHandling.Unit.Result.Tests {
    public class SafeCastTests {
        [Fact]
        public void Passing_Null_ErrorSelector_Throws() {
            Assert.Throws<ArgumentNullException>(
                AssertionUtilities.ErrorSelectorName,
                () => AssertionUtilities.GetGender(2).SafeCast<int>(null)
            );
        }

        [Fact]
        public void Result_With_Error_Using_Invalid_Cast_Expects_No_Exception() {
            const int identity = 3;
            AssertionUtilities
                .GetGender(identity)
                .SafeCast<int>(gender => "ERROR")
                .AssertError("Could not determine gender.");
        }

        [Fact]
        public void Result_With_Error_Using_Valid_Cast_Expects_Error() {
            const int identity = 3;
            AssertionUtilities
                .GetGender(identity)
                .SafeCast<int>(gender => "ERROR")
                .AssertError("Could not determine gender.");
        }

        [Fact]
        public void Result_With_Value_Using_Invalid_Cast_Expects_Cast_Exception() {
            AssertionUtilities.GetGender(0)
                .SafeCast<int>(gender => "ERROR");
        }

        [Fact]
        public void Result_With_Value_Using_Invalid_Cast_Expects_Value_As_Int() {
            ErrorHandling.Result
                .Value<int, string>(20)
                .SafeCast<AssertionUtilities.Gender>(gender => "ERROR")
                .AssertError("ERROR");
        }
    }
}