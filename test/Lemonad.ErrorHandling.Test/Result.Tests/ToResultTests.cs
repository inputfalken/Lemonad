using Xunit;

namespace Lemonad.ErrorHandling.Test.Result.Tests {
    public class ToResultTests {
        [Fact]
        public void Create_Result_From_Value_With_False_Predicate() {
            var predicateExecuted = false;
            var errorSelectorExecuted = false;
            var result = "value".ToResult(x => {
                predicateExecuted = true;
                return string.IsNullOrWhiteSpace(x);
            }, x => {
                errorSelectorExecuted = true;
                return "ERROR";
            });
            Assert.Equal("ERROR", result.Either.Error);
            Assert.Equal(default, result.Either.Value);
            Assert.False(result.Either.HasValue);
            Assert.True(result.Either.HasError);
            Assert.True(predicateExecuted);
            Assert.True(errorSelectorExecuted);
        }

        [Fact]
        public void Create_Result_From_Value_With_True_Predicate() {
            var predicateExecuted = false;
            var errorSelectorExecuted = false;
            var result = "value".ToResult(x => {
                predicateExecuted = true;
                return string.IsNullOrWhiteSpace(x) == false;
            }, x => {
                errorSelectorExecuted = true;
                return "ERROR";
            });
            Assert.Equal(default, result.Either.Error);
            Assert.Equal("value", result.Either.Value);
            Assert.True(result.Either.HasValue);
            Assert.False(result.Either.HasError);
            Assert.True(predicateExecuted);
            Assert.False(errorSelectorExecuted);
        }

        [Fact]
        public void Passing_None_Null_Value_With_Null_Check_Predicate_Does_Not_Throw() {
            var exception = Record.Exception(() => {
                var foo = "foo".ToResult(s => s != null, s => s.Match(y => y, () => "null value"));
                Assert.Equal(default, foo.Either.Error);
                Assert.Equal("foo", foo.Either.Value);
                Assert.False(foo.Either.HasError);
                Assert.True(foo.Either.HasValue);
            });
            Assert.Null(exception);
        }

        [Fact]
        public void Passing_Null_Value_With_Null_Check_Predicate_Does_Not_Throw() {
            var exception = Record.Exception(() => {
                string x = null;
                var foo = x.ToResult(s => s != null, s => s.Match(y => y, () => "null value"));
                Assert.Equal("null value", foo.Either.Error);
                Assert.Equal(default, foo.Either.Value);
                Assert.True(foo.Either.HasError);
                Assert.False(foo.Either.HasValue);
            });
            Assert.Null(exception);
        }
    }
}