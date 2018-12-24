using Assertion;
using Lemonad.ErrorHandling.Extensions;
using Xunit;

namespace Lemonad.ErrorHandling.Unit.Result.Tests {
    public class ToResultTests {
        [Fact]
        public void Create_Result_From_Value_With_False_Predicate() {
            var predicateExecuted = false;
            var errorSelectorExecuted = false;
            "value".ToResult(x => {
                predicateExecuted = true;
                return string.IsNullOrWhiteSpace(x);
            }, x => {
                errorSelectorExecuted = true;
                return "ERROR";
            }).AssertError("ERROR");
            Assert.True(predicateExecuted);
            Assert.True(errorSelectorExecuted);
        }

        [Fact]
        public void Create_Result_From_Value_With_True_Predicate() {
            var predicateExecuted = false;
            var errorSelectorExecuted = false;
            "value".ToResult(x => {
                    predicateExecuted = true;
                    return string.IsNullOrWhiteSpace(x) == false;
                }, x => {
                    errorSelectorExecuted = true;
                    return "ERROR";
                })
                .AssertValue("value");
            Assert.True(predicateExecuted);
            Assert.False(errorSelectorExecuted);
        }

        [Fact]
        public void Passing_None_Null_Value_With_Null_Check_Predicate_Does_Not_Throw() {
            var exception = Record.Exception(() =>
                "foo".ToResult(s => s is null == false, _ => "null value").AssertValue("foo"));
            Assert.Null(exception);
        }

        [Fact]
        public void Passing_Null_Value_With_Null_Check_Predicate_Does_Not_Throw() {
            string x = null;
            var exception = Record.Exception(() =>
                x.ToResult(s => s is null == false, s => s ?? "null value").AssertError("null value"));
            Assert.Null(exception);
        }
    }
}