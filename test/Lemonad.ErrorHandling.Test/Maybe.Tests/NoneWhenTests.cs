using System;
using Xunit;

namespace Lemonad.ErrorHandling.Test.Maybe.Tests {
    public class NoneWhenTests {
        [Fact]
        public void
            Maybe_String_Whose_Property_HasValue_Is_True__Pasing_Null_Predicate__ArgumentNullReferenceException_Thrown() {
            Assert.Throws<ArgumentNullException>(() => {
                Func<string, bool> predicate = null;
                "foo".Some().NoneWhen(predicate);
            });
        }

        [Fact]
        public void
            Maybe_String_Whose_Property_HasValue_Is_False__Pasing_Null_Predicate__No_ArgumentNullReferenceException_Thrown() {
            var exception = Record.Exception(() => {
                Func<string, bool> predicate = null;
                var someWhen = "foo".None().NoneWhen(predicate);
                Assert.False(someWhen.HasValue, "Maybe should not have value.");
                Assert.Equal(default(string), someWhen.Value);
            });
            Assert.Null(exception);
        }

        [Fact]
        public void Null_Predicate__Throws_ArgumentNullException() {
            Assert.Throws<ArgumentNullException>(() => {
                Func<string, bool> predicate = null;
                "foo".NoneWhen(predicate);
            });
        }

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
    }
}