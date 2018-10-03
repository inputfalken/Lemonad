using System;
using Xunit;

namespace Lemonad.ErrorHandling.Test.Maybe.Tests {
    public class SomWhenTests {

        [Fact]
        public void
            Maybe_String_Whose_Property_HasValue_Is_True__Pasing_Null_Predicate__ArgumentNullReferenceException_Thrown() {
            Assert.Throws<ArgumentNullException>(() => {
                Func<string, bool> predicate = null;
                "foo".ToMaybe().Filter(predicate);
            });
        }

        [Fact]
        public void Null_Predicate__Throws_ArgumentNullException() {
            Assert.Throws<ArgumentNullException>(() => {
                Func<string, bool> predicate = null;
                "foo".ToMaybe(predicate);
            });
        }

        [Fact]
        public void When_Predicate_Checks_For_Not_Null__Using_Type_With_Value__Maybe_Is_Expected_To_HaveValue() {
            var maybe = "foobar".ToMaybe(s => s != null);
            Assert.True(maybe.HasValue, "Maybe should have value.");
            Assert.Equal("foobar", maybe.Value);
        }

        [Fact]
        public void When_Predicate_Checks_For_Null__Using_Type_With_Value__Maybe_Is_Expected_To_Not_HaveValue() {
            var maybe = "foobar".ToMaybe(s => s == null);
            Assert.False(maybe.HasValue, "Maybe should not have value.");
            Assert.Equal(default, maybe.Value);
        }

        [Fact]
        public void When_Predicate_Checks_For_Null__Using_Type_Without_Value__Maybe_Is_Expected_To_Throw() {
            Assert.Throws<ArgumentNullException>(AssertionUtilities.EitherValueName,() => {
                string value = null;
                var maybe = value.ToMaybe(s => s == null);
                Assert.True(maybe.HasValue, "Maybe should have value.");
                Assert.Equal(default, maybe.Value);
            });
        }

        [Fact]
        public void When_Predicate_Returns_False__Maybe_Is_Expected_To_HaveValue() {
            var maybe = string.Empty.ToMaybe(s => false);
            Assert.False(maybe.HasValue, "Maybe should not have value.");
            Assert.Equal(default, maybe.Value);
        }

        [Fact]
        public void When_Predicate_Returns_True__Maybe_Is_Expected_To_HaveValue() {
            var maybe = "".ToMaybe(s => true);
            Assert.True(maybe.HasValue, "Maybe should have value.");
            Assert.Equal(string.Empty, maybe.Value);
        }
    }
}