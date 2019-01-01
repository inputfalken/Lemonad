using Assertion;
using Lemonad.ErrorHandling.Extensions;
using Xunit;

namespace Lemonad.ErrorHandling.Unit.ExtensionTests.Nullable {
    public class ToMaybe {

        [Fact]
        public void With_Value__Expects_Value() {
            int? foo = 1;
            foo.ToMaybe().AssertValue(1);
        }

        [Fact]
        public void With_Null__Expects_None() {
            int? foo = null;
            foo.ToMaybe().AssertNone();
        }
    }
}