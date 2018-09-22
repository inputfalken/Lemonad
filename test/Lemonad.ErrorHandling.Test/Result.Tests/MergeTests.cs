using Xunit;

namespace Lemonad.ErrorHandling.Test.Result.Tests {
    public class ZipTests {
        [Fact]
        public void Result_With_No_Value_Zips_Result_With_No_Value__Expects_Result_With_No_Value() {
            var resultSelectorInvoked = false;
            var outer = new {Id = 1, Text = "Hello"}.ToResult(x => false, () => "ERROR 1");
            var inner = new {Id = 1, Text = "world"}.ToResult(x => false, () => "ERROR 2");
            var result = outer.Zip(inner, (x, y) => {
                resultSelectorInvoked = true;
                return $"{x.Text} {y.Text}";
            });

            Assert.Equal(result.Value, default);
            Assert.Equal(result.Error, "ERROR 1");
            Assert.False(resultSelectorInvoked, "resultSelectorInvoked");
        }

        [Fact]
        public void Result_With_No_Value_Zips_Result_With_Value__Expects_Result_With_No_Value() {
            var resultSelectorInvoked = false;
            var outer = new {Id = 1, Text = "Hello"}.ToResult(x => false, () => "ERROR 1");
            var inner = new {Id = 1, Text = "world"}.ToResult(x => true, () => "ERROR 2");
            var result = outer.Zip(inner, (x, y) => {
                resultSelectorInvoked = true;
                return $"{x.Text} {y.Text}";
            });

            Assert.Equal(result.Value, default);
            Assert.Equal(result.Error, "ERROR 1");
            Assert.False(resultSelectorInvoked, "resultSelectorInvoked");
        }

        [Fact]
        public void Result_With_Value_Zips_Result_With_No_Value__Expects_Result_With_No_Value() {
            var resultSelectorInvoked = false;
            var outer = new {Id = 1, Text = "Hello"}.ToResult(x => true, () => "ERROR 1");
            var inner = new {Id = 1, Text = "world"}.ToResult(x => false, () => "ERROR 2");
            var result = outer.Zip(inner, (x, y) => {
                resultSelectorInvoked = true;
                return $"{x.Text} {y.Text}";
            });

            Assert.Equal(result.Value, default);
            Assert.Equal(result.Error, "ERROR 2");
            Assert.False(resultSelectorInvoked, "resultSelectorInvoked");
        }

        [Fact]
        public void Result_With_Value_Zips_Result_With_Value__Expects_Result_With_Value() {
            var resultSelectorInvoked = false;
            var outer = new {Id = 1, Text = "Hello"}.ToResult(x => true, () => "ERROR 1");
            var inner = new {Id = 1, Text = "world"}.ToResult(x => true, () => "ERROR 2");
            var result = outer.Zip(inner, (x, y) => {
                resultSelectorInvoked = true;
                return $"{x.Text} {y.Text}";
            });

            Assert.Equal(result.Value, "Hello world");
            Assert.Equal(result.Error, default);
            Assert.True(resultSelectorInvoked, "resultSelectorInvoked");
        }
    }
}
