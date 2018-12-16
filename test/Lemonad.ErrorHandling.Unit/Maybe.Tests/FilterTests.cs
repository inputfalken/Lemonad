using System;
using Xunit;

namespace Lemonad.ErrorHandling.Unit.Maybe.Tests {
    public class FilterTests {
        [Fact]
        public void Maybe_With_Error_Null_Predicate__Throws_ArgumentNullException() {
            Assert.Throws<ArgumentNullException>(() => {
                Func<string, bool> predicate = null;
                ErrorHandling.Maybe.None<string>().Filter(predicate);
            });
        }

        [Fact]
        public void Maybe_With_No_Value_With_True_Predicate__Expects_Maybe_No_With_Value() {
            var predicateExecuted = false;
            var maybe = ErrorHandling.Maybe.None<string>().Filter(s => {
                predicateExecuted = true;
                return s != null;
            });
            Assert.False(predicateExecuted);
            Assert.False(maybe.HasValue, "Maybe should have value.");
            Assert.Equal(default, maybe.Value);
        }

        [Fact]
        public void Maybe_With_Value_Null_Predicate__Throws_ArgumentNullException() {
            Assert.Throws<ArgumentNullException>(() => {
                Func<string, bool> predicate = null;
                ErrorHandling.Maybe.Value("foo").Filter(predicate);
            });
        }

        [Fact]
        public void Maybe_With_Value_With_False_Predicate__Expects_Maybe_No_With_Value() {
            var predicateExecuted = false;
            var maybe = ErrorHandling.Maybe.Value("foobar").Filter(s => {
                predicateExecuted = true;
                return s == null;
            });
            Assert.True(predicateExecuted);
            Assert.False(maybe.HasValue, "Maybe should have value.");
            Assert.Equal(default, maybe.Value);
        }

        [Fact]
        public void Maybe_With_Value_With_True_Predicate__Expects_Maybe_With_Value() {
            var predicateExecuted = false;
            var maybe = ErrorHandling.Maybe.Value("foobar").Filter(s => {
                predicateExecuted = true;
                return s != null;
            });
            Assert.True(predicateExecuted);
            Assert.True(maybe.HasValue, "Maybe should have value.");
            Assert.Equal("foobar", maybe.Value);
        }
    }
}