using System;
using Xunit;

namespace Lemonad.ErrorHandling.Unit.Result.Tests {
    public class CastTests {
        [Fact]
        public void Result_With_Error_Using_Invalid_Cast_Expects_No_Exception() {
            const int identity = 3;
            AssertionUtilities
                .GetGender(identity)
                .Cast<string>()
                .AssertError("Could not determine gender.");
        }

        [Fact]
        public void Result_With_Error_Using_Valid_Cast_Expects_Error() {
            const int identity = 3;
            AssertionUtilities
                .GetGender(identity)
                .Cast<int>()
                .AssertError("Could not determine gender.");
        }

        [Fact]
        public void Result_With_Value_Using_Invalid_Cast_Expects_Cast_Exception() {
            const int identity = 0;
            Assert.Throws<InvalidCastException>(() => AssertionUtilities.GetGender(identity).Cast<string>());
        }

        [Fact]
        public void Result_With_Value_Using_Valid_Cast_Expects_Value_As_Int() {
            const int identity = 0;
            AssertionUtilities
                .GetGender(identity)
                .Cast<int>()
                .AssertValue(identity);
        }
    }
}