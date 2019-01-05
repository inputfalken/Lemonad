using System;
using Assertion;
using Xunit;

namespace Lemonad.ErrorHandling.Unit.Result.Tests {
    public class FullCastTests {
        [Fact]
        public void With_Valid_Value_Cast_Single_Type() {
            var result = AssertionUtilities.GetGender(0).Map(x => (int) x);
            Assert.Null(
                Record.Exception(
                    () => result.FullCast<AssertionUtilities.Gender>()
                )
            );
        }

        [Fact]
        public void With_Valid_Value_Valid_Cast_Double_Type() {
            var result = AssertionUtilities.GetGender(0).Map(x => (int) x);
            Assert.Null(
                Record.Exception(
                    () => result.FullCast<AssertionUtilities.Gender, AssertionUtilities.Error>()
                )
            );
        }

        [Fact]
        public void With_Error_Single_Type_Throws() {
            var result = AssertionUtilities.GetGender(int.MaxValue).Map(x => (int) x);
            Assert.Throws<InvalidCastException>(
                () => result.FullCast<AssertionUtilities.Gender>()
            );
        }

        [Fact]
        public void With_Error_Double_Type_Throws() {
            var result = AssertionUtilities.GetGender(int.MaxValue).Map(x => (int) x);
            Assert.Throws<InvalidCastException>(
                () => result.FullCast<AssertionUtilities.Gender, AssertionUtilities.Error>()
            );
        }
    }
}