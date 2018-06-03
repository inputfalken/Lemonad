using System;
using Xunit;

namespace Lemonad.ErrorHandling.Test.Maybe.Tests {
    public class NoneWhenTests {
        [Fact]
        public void When_Predicate_Returns_False__Maybe_Is_Expected_To_HaveValue() {
            var noneWhen = "".NoneWhen(s => false);
            Assert.True(noneWhen.HasValue, "Maybe should have value.");
        }

        [Fact]
        public void When_Predicate_Returns_True__Maybe_Is_Expected_To_HaveValue() {
            var noneWhen = "".NoneWhen(s => true);
            Assert.False(noneWhen.HasValue, "Maybe should not have value.");
        }

        [Fact]
        public void Null_Predicate__Throws_ArgumentNullException() {
            Assert.Throws<ArgumentNullException>(() => {
                Func<string, bool> predicate = null;
                "foo".NoneWhen(predicate);
            });
        }

        [Fact]
        public void Nullable_Bool_Whose_Value_Is_Null__Expects_HasValue_To_Be_False() {
            bool? foo = null;
            Assert.False(foo.NoneWhen(_ => throw new ArgumentNullException()).HasValue,
                "This predicate should not be evaluated.");
        }
    }
}