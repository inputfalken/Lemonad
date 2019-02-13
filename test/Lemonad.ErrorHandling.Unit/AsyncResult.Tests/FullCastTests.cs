using System;
using System.Threading.Tasks;
using Assertion;
using Lemonad.ErrorHandling.Extensions.AsyncResult;
using Xunit;

namespace Lemonad.ErrorHandling.Unit.AsyncResult.Tests {
    public class FullCastTests {
        [Fact]
        public Task With_Error_Double_Type_Throws() {
            var result = AssertionUtilities.GetGenderAsync(int.MaxValue).Map(x => (int) x);
            return Assert.ThrowsAsync<InvalidCastException>(
                async () => await result.FullCast<AssertionUtilities.Gender, AssertionUtilities.Error>()
            );
        }

        [Fact]
        public Task With_Error_Single_Type_Throws() {
            var result = AssertionUtilities.GetGenderAsync(int.MaxValue).Map(x => (int) x);
            return Assert.ThrowsAsync<InvalidCastException>(
                async () => await result.FullCast<AssertionUtilities.Gender>()
            );
        }

        [Fact]
        public void With_Valid_Value_Cast_Single_Type() {
            var result = AssertionUtilities.GetGenderAsync(0).Map(x => (int) x);
            Assert.Null(
                Record.Exception(
                    () => result.FullCast<AssertionUtilities.Gender>()
                )
            );
        }

        [Fact]
        public void With_Valid_Value_Valid_Cast_Double_Type() {
            var result = AssertionUtilities.GetGenderAsync(0).Map(x => (int) x);
            Assert.Null(
                Record.Exception(
                    () => result.FullCast<AssertionUtilities.Gender, AssertionUtilities.Error>()
                )
            );
        }
    }
}