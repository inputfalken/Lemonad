using System;
using Lemonad.ErrorHandling.Extensions;
using Xunit;

namespace Lemonad.ErrorHandling.Unit.Result.Tests {
    public class ToResultErrorTests {
        [Fact]
        public void Convert_Int_To_ResultError() {
            var errorSelectorInvoked = false;
            2.ToResultError(i => true, x => {
                errorSelectorInvoked = true;
                return "";
            }).AssertError(2);
            Assert.False(errorSelectorInvoked);
        }

        [Fact]
        public void Convert_Null_String_To_ResultError() {
            string str = null;
            Assert.Throws<ArgumentNullException>(AssertionUtilities.EitherErrorName,
                () => str.ToResultError(x => true, x => ""));
        }

        [Fact]
        public void Convert_String_To_ResultError() {
            "hello".ToResultError(s => true, x => "").AssertError("hello");
        }
    }
}