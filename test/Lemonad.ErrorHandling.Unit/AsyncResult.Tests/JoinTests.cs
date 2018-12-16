using System.Threading.Tasks;
using Lemonad.ErrorHandling.Extensions;
using Lemonad.ErrorHandling.Extensions.AsyncResult;
using Lemonad.ErrorHandling.Extensions.Result;
using Xunit;

namespace Lemonad.ErrorHandling.Unit.AsyncResult.Tests {
    public class JoinTests {
        [Fact]
        public async Task
            Result_With_No_Value_Joins_Result_With_No_Value_Using_Matching_Key__Expects_Result_With_No_Value() {
            var outerSelectorInvoked = false;
            var innerSelectorInvoked = false;
            var resultSelectorInvoked = false;
            var outer = new {Id = 1, Text = "Hello"}.ToResult(x => false, x => "ERROR 1").ToAsyncResult();
            var inner = new {Id = 1, Text = "world"}.ToResult(x => false, x => "ERROR 2").ToAsyncResult();
            var result = await outer.JoinAsync(inner, x => {
                outerSelectorInvoked = true;
                return x.Id;
            }, x => {
                innerSelectorInvoked = true;
                return x.Id;
            }, (x, y) => {
                resultSelectorInvoked = true;
                return $"{x.Text} {y.Text}";
            }, () => string.Empty);

            Assert.Equal(default, result.Either.Value);
            Assert.Equal("ERROR 1", result.Either.Error);

            Assert.False(outerSelectorInvoked, "outerSelectorInvoked");
            Assert.False(innerSelectorInvoked, "innerSelectorInvoked");
            Assert.False(resultSelectorInvoked, "resultSelectorInvoked");
        }

        [Fact]
        public async Task
            Result_With_No_Value_Joins_Result_With_Value_Using_Matching_Key__Expects_Result_With_No_Value() {
            var outerSelectorInvoked = false;
            var innerSelectorInvoked = false;
            var resultSelectorInvoked = false;
            var outer = new {Id = 1, Text = "Hello"}.ToResult(x => false, x => "ERROR 1").ToAsyncResult();
            var inner = new {Id = 1, Text = "world"}.ToResult(x => true, x => "ERROR 2").ToAsyncResult();
            var result = await outer.JoinAsync(inner, x => {
                outerSelectorInvoked = true;
                return x.Id;
            }, x => {
                innerSelectorInvoked = true;
                return x.Id;
            }, (x, y) => {
                resultSelectorInvoked = true;
                return $"{x.Text} {y.Text}";
            }, () => string.Empty);

            Assert.Equal(default, result.Either.Value);
            Assert.Equal("ERROR 1", result.Either.Error);

            Assert.False(outerSelectorInvoked, "outerSelectorInvoked");
            Assert.False(innerSelectorInvoked, "innerSelectorInvoked");
            Assert.False(resultSelectorInvoked, "resultSelectorInvoked");
        }

        [Fact]
        public async Task
            Result_With_Value_Joins_Result_With_No_Value_Using_Matching_Key__Expects_Result_With_No_Value() {
            var outerSelectorInvoked = false;
            var innerSelectorInvoked = false;
            var resultSelectorInvoked = false;
            var outer = new {Id = 1, Text = "Hello"}.ToResult(x => true, x => "ERROR 1").ToAsyncResult();
            var inner = new {Id = 1, Text = "world"}.ToResult(x => false, x => "ERROR 2").ToAsyncResult();
            var result = await outer.JoinAsync(inner, x => {
                outerSelectorInvoked = true;
                return x.Id;
            }, x => {
                innerSelectorInvoked = true;
                return x.Id;
            }, (x, y) => {
                resultSelectorInvoked = true;
                return $"{x.Text} {y.Text}";
            }, () => string.Empty);

            Assert.Equal(default, result.Either.Value);
            Assert.Equal("ERROR 2", result.Either.Error);

            Assert.False(outerSelectorInvoked, "outerSelectorInvoked");
            Assert.False(innerSelectorInvoked, "innerSelectorInvoked");
            Assert.False(resultSelectorInvoked, "resultSelectorInvoked");
        }

        [Fact]
        public async Task Result_With_Value_Joins_Result_With_Value_Using_Matching_Key__Expects_Result_With_Value() {
            var outerSelectorInvoked = false;
            var innerSelectorInvoked = false;
            var resultSelectorInvoked = false;
            var outer = new {Id = 1, Text = "Hello"}.ToResult(x => true, x => "ERROR 1").ToAsyncResult();
            var inner = new {Id = 1, Text = "world"}.ToResult(x => true, x => "ERROR 2").ToAsyncResult();
            var result = await outer.JoinAsync(inner, x => {
                outerSelectorInvoked = true;
                return x.Id;
            }, x => {
                innerSelectorInvoked = true;
                return x.Id;
            }, (x, y) => {
                resultSelectorInvoked = true;
                return $"{x.Text} {y.Text}";
            }, () => "");

            Assert.Equal("Hello world", result.Either.Value);
            Assert.Equal(default, result.Either.Error);

            Assert.True(outerSelectorInvoked, "outerSelectorInvoked");
            Assert.True(innerSelectorInvoked, "innerSelectorInvoked");
            Assert.True(resultSelectorInvoked, "resultSelectorInvoked");
        }

        [Fact]
        public async Task
            Result_With_Value_Joins_Result_With_Value_Using_No_Matching_Key__Expects_Result_With_No_Value() {
            var outerSelectorInvoked = false;
            var innerSelectorInvoked = false;
            var resultSelectorInvoked = false;
            var outer = new {Id = 2, Text = "Hello"}.ToResult(x => true, x => "ERROR 1").ToAsyncResult();
            var inner = new {Id = 1, Text = "world"}.ToResult(x => true, x => "ERROR 2").ToAsyncResult();
            var result = await outer.JoinAsync(inner, x => {
                outerSelectorInvoked = true;
                return x.Id;
            }, x => {
                innerSelectorInvoked = true;
                return x.Id;
            }, (x, y) => {
                resultSelectorInvoked = true;
                return $"{x.Text} {y.Text}";
            }, () => "No key match");

            Assert.Equal(default, result.Either.Value);
            Assert.Equal("No key match", result.Either.Error);

            Assert.True(outerSelectorInvoked, "outerSelectorInvoked");
            Assert.True(innerSelectorInvoked, "innerSelectorInvoked");
            Assert.False(resultSelectorInvoked, "resultSelectorInvoked");
        }
    }
}