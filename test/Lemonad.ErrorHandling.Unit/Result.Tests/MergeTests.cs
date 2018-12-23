using Lemonad.ErrorHandling.Extensions;
using Xunit;

namespace Lemonad.ErrorHandling.Unit.Result.Tests {
    public class ZipTests {
        [Fact]
        public void Result_With_No_Value_Zips_Result_With_No_Value__Expects_Result_With_No_Value() {
            var resultSelectorInvoked = false;
            var outer = new {Id = 1, Text = "Hello"}.ToResult(x => false, x => "ERROR 1");
            var inner = new {Id = 1, Text = "world"}.ToResult(x => false, x => "ERROR 2");
            outer
                .Zip(inner, (x, y) => {
                    resultSelectorInvoked = true;
                    return $"{x.Text} {y.Text}";
                })
                .AssertError("ERROR 1");

            Assert.False(resultSelectorInvoked, "resultSelectorInvoked");
        }

        [Fact]
        public void Result_With_No_Value_Zips_Result_With_Value__Expects_Result_With_No_Value() {
            var resultSelectorInvoked = false;
            var outer = new {Id = 1, Text = "Hello"}.ToResult(x => false, x => "ERROR 1");
            var inner = new {Id = 1, Text = "world"}.ToResult(x => true, x => "ERROR 2");
            outer
                .Zip(inner, (x, y) => {
                    resultSelectorInvoked = true;
                    return $"{x.Text} {y.Text}";
                })
                .AssertError("ERROR 1");

            Assert.False(resultSelectorInvoked, "resultSelectorInvoked");
        }

        [Fact]
        public void Result_With_Value_Zips_Result_With_No_Value__Expects_Result_With_No_Value() {
            var resultSelectorInvoked = false;
            var outer = new {Id = 1, Text = "Hello"}.ToResult(x => true, x => "ERROR 1");
            var inner = new {Id = 1, Text = "world"}.ToResult(x => false, x => "ERROR 2");
            outer
                .Zip(inner, (x, y) => {
                    resultSelectorInvoked = true;
                    return $"{x.Text} {y.Text}";
                })
                .AssertError("ERROR 2");

            Assert.False(resultSelectorInvoked, "resultSelectorInvoked");
        }

        [Fact]
        public void Result_With_Value_Zips_Result_With_Value__Expects_Result_With_Value() {
            var resultSelectorInvoked = false;
            var outer = new {Id = 1, Text = "Hello"}.ToResult(x => true, x => "ERROR 1");
            var inner = new {Id = 1, Text = "world"}.ToResult(x => true, x => "ERROR 2");
            outer
                .Zip(inner, (x, y) => {
                    resultSelectorInvoked = true;
                    return $"{x.Text} {y.Text}";
                })
                .AssertValue("Hello world");

            Assert.True(resultSelectorInvoked, "resultSelectorInvoked");
        }
    }
}