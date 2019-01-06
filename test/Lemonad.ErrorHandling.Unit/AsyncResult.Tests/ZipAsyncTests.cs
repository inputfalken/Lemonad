using System;
using System.Threading.Tasks;
using Assertion;
using Lemonad.ErrorHandling.Extensions;
using Lemonad.ErrorHandling.Extensions.AsyncResult;
using Lemonad.ErrorHandling.Extensions.Result;
using Xunit;

namespace Lemonad.ErrorHandling.Unit.AsyncResult.Tests {
    public class ZipAsyncTests {
        [Fact]
        public void Passing_Null_IResult_Throws() {
            Assert.Throws<ArgumentNullException>(
                AssertionUtilities.ZipOtherParameter,
                () => new {Id = 1, Text = "Hello"}
                    .ToResult(x => false, x => "ERROR 1")
                    .ToAsyncResult()
                    .ZipAsync<string, string>(null, (x, y) => $"{x.Text} {y}")
            );
        }

        [Fact]
        public void Passing_Null_Selector_Throws() {
            var inner = "world".ToResult(x => false, x => "ERROR 2").ToAsyncResult();
            Assert.Throws<ArgumentNullException>(
                AssertionUtilities.SelectorName,
                () => "hello"
                    .ToResult(x => false, x => "ERROR 1")
                    .ToAsyncResult()
                    .ZipAsync<string, string>(inner, null)
            );
        }

        [Fact]
        public async Task Result_With_No_Value_Zips_Result_With_No_Value__Expects_Result_With_No_Value() {
            var resultSelectorInvoked = false;
            var outer = new {Id = 1, Text = "Hello"}.ToResult(x => false, x => "ERROR 1").ToAsyncResult();
            var inner = new {Id = 1, Text = "world"}.ToResult(x => false, x => "ERROR 2").ToAsyncResult();
            await outer
                .ZipAsync(inner, (x, y) => {
                    resultSelectorInvoked = true;
                    return $"{x.Text} {y.Text}";
                })
                .AssertError("ERROR 1");

            Assert.False(resultSelectorInvoked, "resultSelectorInvoked");
        }

        [Fact]
        public async Task Result_With_No_Value_Zips_Result_With_Value__Expects_Result_With_No_Value() {
            var resultSelectorInvoked = false;
            var outer = new {Id = 1, Text = "Hello"}.ToResult(x => false, x => "ERROR 1").ToAsyncResult();
            var inner = new {Id = 1, Text = "world"}.ToResult(x => true, x => "ERROR 2").ToAsyncResult();
            await outer
                .ZipAsync(inner, (x, y) => {
                    resultSelectorInvoked = true;
                    return $"{x.Text} {y.Text}";
                })
                .AssertError("ERROR 1");

            Assert.False(resultSelectorInvoked, "resultSelectorInvoked");
        }

        [Fact]
        public async Task Result_With_Value_Zips_Result_With_No_Value__Expects_Result_With_No_Value() {
            var resultSelectorInvoked = false;
            var outer = new {Id = 1, Text = "Hello"}.ToResult(x => true, x => "ERROR 1").ToAsyncResult();
            var inner = new {Id = 1, Text = "world"}.ToResult(x => false, x => "ERROR 2").ToAsyncResult();
            await outer
                .ZipAsync(inner, (x, y) => {
                    resultSelectorInvoked = true;
                    return $"{x.Text} {y.Text}";
                })
                .AssertError("ERROR 2");

            Assert.False(resultSelectorInvoked, "resultSelectorInvoked");
        }

        [Fact]
        public async Task Result_With_Value_Zips_Result_With_Value__Expects_Result_With_Value() {
            var resultSelectorInvoked = false;
            var outer = new {Id = 1, Text = "Hello"}.ToResult(x => true, x => "ERROR 1").ToAsyncResult();
            var inner = new {Id = 1, Text = "world"}.ToResult(x => true, x => "ERROR 2").ToAsyncResult();
            await outer
                .ZipAsync(inner, (x, y) => {
                    resultSelectorInvoked = true;
                    return $"{x.Text} {y.Text}";
                })
                .AssertValue("Hello world");

            Assert.True(resultSelectorInvoked, "resultSelectorInvoked");
        }
    }
}