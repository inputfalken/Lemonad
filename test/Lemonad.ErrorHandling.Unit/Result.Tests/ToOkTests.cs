using System;
using Assertion;
using Lemonad.ErrorHandling.Extensions;
using Xunit;

namespace Lemonad.ErrorHandling.Unit.Result.Tests {
    public class ToResultOkTests {
        [Fact]
        public void Convert_Int_To_ResultOk() => 2.ToResult(x => true, x => "").AssertValue(2);

        [Fact]
        public void Convert_Null_String_To_ResultOk() {
            string str = null;
            Assert.Throws<ArgumentNullException>(AssertionUtilities.ValueParamName,
                () => str.ToResult(x => true, x => ""));
        }

        [Fact]
        public void Convert_String_To_ResultOk() => "hello".ToResult(x => true, x => "").AssertValue("hello");
    }
}