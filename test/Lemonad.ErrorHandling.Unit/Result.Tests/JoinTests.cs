using Assertion;
using Lemonad.ErrorHandling.Extensions;
using Xunit;

namespace Lemonad.ErrorHandling.Unit.Result.Tests {
    public class JoinTests {
        [Fact]
        public void Result_With_No_Value_Joins_Result_With_No_Value_Using_Matching_Key__Expects_Result_With_No_Value() {
            var outerSelectorInvoked = false;
            var innerSelectorInvoked = false;
            var resultSelectorInvoked = false;
            var outer = new {Id = 1, Text = "Hello"}.ToResult(x => false, x => "ERROR 1");
            var inner = new {Id = 1, Text = "world"}.ToResult(x => false, x => "ERROR 2");
            outer.Join(inner, x => {
                outerSelectorInvoked = true;
                return x.Id;
            }, x => {
                innerSelectorInvoked = true;
                return x.Id;
            }, (x, y) => {
                resultSelectorInvoked = true;
                return $"{x.Text} {y.Text}";
            }, () => string.Empty).AssertError("ERROR 1");

            Assert.False(outerSelectorInvoked, "outerSelectorInvoked");
            Assert.False(innerSelectorInvoked, "innerSelectorInvoked");
            Assert.False(resultSelectorInvoked, "resultSelectorInvoked");
        }

        [Fact]
        public void Result_With_No_Value_Joins_Result_With_Value_Using_Matching_Key__Expects_Result_With_No_Value() {
            var outerSelectorInvoked = false;
            var innerSelectorInvoked = false;
            var resultSelectorInvoked = false;
            var outer = new {Id = 1, Text = "Hello"}.ToResult(x => false, x => "ERROR 1");
            var inner = new {Id = 1, Text = "world"}.ToResult(x => true, x => "ERROR 2");
            outer
                .Join(inner, x => {
                    outerSelectorInvoked = true;
                    return x.Id;
                }, x => {
                    innerSelectorInvoked = true;
                    return x.Id;
                }, (x, y) => {
                    resultSelectorInvoked = true;
                    return $"{x.Text} {y.Text}";
                }, () => string.Empty)
                .AssertError("ERROR 1");

            Assert.False(outerSelectorInvoked, "outerSelectorInvoked");
            Assert.False(innerSelectorInvoked, "innerSelectorInvoked");
            Assert.False(resultSelectorInvoked, "resultSelectorInvoked");
        }

        [Fact]
        public void Result_With_Value_Joins_Result_With_No_Value_Using_Matching_Key__Expects_Result_With_No_Value() {
            var outerSelectorInvoked = false;
            var innerSelectorInvoked = false;
            var resultSelectorInvoked = false;
            var outer = new {Id = 1, Text = "Hello"}.ToResult(x => true, x => "ERROR 1");
            var inner = new {Id = 1, Text = "world"}.ToResult(x => false, x => "ERROR 2");
            outer
                .Join(inner, x => {
                    outerSelectorInvoked = true;
                    return x.Id;
                }, x => {
                    innerSelectorInvoked = true;
                    return x.Id;
                }, (x, y) => {
                    resultSelectorInvoked = true;
                    return $"{x.Text} {y.Text}";
                }, () => string.Empty)
                .AssertError("ERROR 2");

            Assert.False(outerSelectorInvoked, "outerSelectorInvoked");
            Assert.False(innerSelectorInvoked, "innerSelectorInvoked");
            Assert.False(resultSelectorInvoked, "resultSelectorInvoked");
        }

        [Fact]
        public void Result_With_Value_Joins_Result_With_Value_Using_Matching_Key__Expects_Result_With_Value() {
            var outerSelectorInvoked = false;
            var innerSelectorInvoked = false;
            var resultSelectorInvoked = false;
            var outer = new {Id = 1, Text = "Hello"}.ToResult(x => true, x => "ERROR 1");
            var inner = new {Id = 1, Text = "world"}.ToResult(x => true, x => "ERROR 2");
            outer
                .Join(inner, x => {
                    outerSelectorInvoked = true;
                    return x.Id;
                }, x => {
                    innerSelectorInvoked = true;
                    return x.Id;
                }, (x, y) => {
                    resultSelectorInvoked = true;
                    return $"{x.Text} {y.Text}";
                }, () => "")
                .AssertValue("Hello world");

            Assert.True(outerSelectorInvoked, "outerSelectorInvoked");
            Assert.True(innerSelectorInvoked, "innerSelectorInvoked");
            Assert.True(resultSelectorInvoked, "resultSelectorInvoked");
        }

        [Fact]
        public void Result_With_Value_Joins_Result_With_Value_Using_No_Matching_Key__Expects_Result_With_No_Value() {
            var outerSelectorInvoked = false;
            var innerSelectorInvoked = false;
            var resultSelectorInvoked = false;
            var outer = new {Id = 2, Text = "Hello"}.ToResult(x => true, x => "ERROR 1");
            var inner = new {Id = 1, Text = "world"}.ToResult(x => true, x => "ERROR 2");
            outer.Join(inner, x => {
                outerSelectorInvoked = true;
                return x.Id;
            }, x => {
                innerSelectorInvoked = true;
                return x.Id;
            }, (x, y) => {
                resultSelectorInvoked = true;
                return $"{x.Text} {y.Text}";
            }, () => "No key match").AssertError("No key match");

            Assert.True(outerSelectorInvoked, "outerSelectorInvoked");
            Assert.True(innerSelectorInvoked, "innerSelectorInvoked");
            Assert.False(resultSelectorInvoked, "resultSelectorInvoked");
        }
    }
}