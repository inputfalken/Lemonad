using System;
using Assertion;
using Lemonad.ErrorHandling.Extensions.Result;
using Xunit;

namespace Lemonad.ErrorHandling.Unit.ExtensionTests.Result {
    public class ToMaybeTests {
        [Fact]
        public void Passing_Null_Source_Throws() {
            Assert.Throws<ArgumentNullException>(
                AssertionUtilities.ExtensionParameterName,
                () => ((IResult<int, string>) null).ToMaybe()
            );
        }

        [Fact]
        public void Result_With_Error__Expects_Maybe_With_No_value()
            => ErrorHandling.Result.Error<string, int>(2).ToMaybe().AssertNone();

        [Fact]
        public void Result_With_Value__Expects_Maybe_With_Value()
            => ErrorHandling.Result.Value<string, int>("Foo").ToMaybe().AssertValue("Foo");
    }
}