using System;
using Xunit;

namespace Lemonad.ErrorHandling.Test.Result.Tests {
    public class FlatMapOkTests {
        [Fact]
        public void
            Result_String_Int__Whose_Property_HasValue_Is_False__Flatmaps__Result_String_Double__Whose_Property_HasValue_Is_False__Expects_Result_String_Double__Whose_Property_HasValue_Is_False__With_error_Message_From_First_result() {
            var intParse = ResultParsers.Int("foo");
            var doubleParse = ResultParsers.Double("foo");
            var rightExecuted = false;
            var errorExectued = false;
            var result = intParse.FlatMap(i => {
                rightExecuted = true;
                return doubleParse;
            }, s => {
                errorExectued = true;
                return "Failure" + s;
            });

            Assert.False(rightExecuted, "Ok should not get exectued since intparse did not succeed.");
            Assert.False(errorExectued, "error should not be exectued since intparse did not succeed.");
            Assert.True(result.HasError, "Result should have a error value.");
            Assert.False(result.HasValue, "Result should have error.");
            Assert.Equal("Could not parse type System.String(\"foo\") into System.Int32.", result.Error);
            Assert.Equal(default(double), result.Value);
        }

        [Fact]
        public void
            Result_String_Int__Whose_Property_HasValue_Is_False__Flatmaps__Result_String_Double__Whose_Property_HasValue_Is_False__Expects_Result_String_Double__Whose_Property_HasValue_Is_True__With_error_Message_From_Second_result() {
            var rightExecuted = false;
            var errorExectued = false;
            var intParse = ResultParsers.Int("foo");
            var doubleParse = ResultParsers.Double("2");
            var result = intParse.FlatMap(i => {
                rightExecuted = true;
                return doubleParse;
            }, s => {
                errorExectued = true;
                return "Failure " + s;
            });

            Assert.False(rightExecuted, "Ok should not get exectued since intparse did not succeed.");
            Assert.False(errorExectued, "error should not be exectued since intparse did not succeed.");
            Assert.True(result.HasError, "Result should have a error value.");
            Assert.False(result.HasValue, "Result should have error.");
            Assert.Equal("Could not parse type System.String(\"foo\") into System.Int32.", result.Error);
            Assert.Equal(default(double), result.Value);
        }

        [Fact]
        public void
            Result_String_Int__Whose_Property_HasValue_Is_False__Flatmaps__Result_String_Double__Whose_Property_HasValue_Is_False_With_Null_errorSelector__Expects_No_ArgumentNullException() {
            var exception = Record.Exception(() => {
                var intParse = ResultParsers.Int("foo");
                var doubleParse = ResultParsers.Double("foo");
                Func<string, string> errorSelector = null;
                var result = intParse.FlatMap(i => { return doubleParse; }, errorSelector);

                Assert.True(result.HasError, "Result should have a error value.");
                Assert.False(result.HasValue, "Result should have error.");
                Assert.Equal("Could not parse type System.String(\"foo\") into System.Int32.", result.Error);
                Assert.Equal(default(double), result.Value);
            });
            Assert.Null(exception);
        }

        [Fact]
        public void
            Result_String_Int__Whose_Property_HasValue_Is_False__Flatmaps__Result_String_Double__Whose_Property_HasValue_Is_False_With_Null_OkSelector__Expects_No_ArgumentNullException() {
            var exception = Record.Exception(() => {
                var intParse = ResultParsers.Int("foo");
                var doubleParse = ResultParsers.Double("foo");
                Func<int, Result<double, string>> rightselector = null;
                var result = intParse.FlatMap(rightselector, s => { return "Failure" + s; });

                Assert.True(result.HasError, "Result should have a error value.");
                Assert.False(result.HasValue, "Result should have error.");
                Assert.Equal("Could not parse type System.String(\"foo\") into System.Int32.", result.Error);
                Assert.Equal(default(double), result.Value);
            });
            Assert.Null(exception);
        }

        [Fact]
        public void
            Result_String_Int__Whose_Property_HasValue_Is_False__Flatmaps__Result_String_Double__Whose_Property_HasValue_Is_False_With_Null_OkSelector_And_Null_errorSelector__Expects_No_ArgumentNullException() {
            var intParse = ResultParsers.Int("foo");
            Func<int, Result<double, string>> rightselector = null;
            Func<string, string> errorSelector = null;
            var result = intParse.FlatMap(rightselector, errorSelector);

            Assert.True(result.HasError, "Result should have a error value.");
            Assert.False(result.HasValue, "Result should have error.");
            Assert.Equal("Could not parse type System.String(\"foo\") into System.Int32.", result.Error);
            Assert.Equal(default(double), result.Value);
        }

        [Fact]
        public void
            Result_String_Int__Whose_Property_HasValue_Is_False__Flatmaps__Result_String_Double__Whose_Property_HasValue_Is_True_With_Null_errorSelector__Expects_No_ArgumentNullException() {
            var exception = Record.Exception(() => {
                var intParse = ResultParsers.Int("foo");
                var doubleParse = ResultParsers.Double("2");
                Func<int, Result<string, double>> rightselector = null;
                Func<string, string> errorSelector = null;
                var result = intParse.FlatMap(x => doubleParse, errorSelector);

                Assert.True(result.HasError, "Result should have a error value.");
                Assert.False(result.HasValue, "Result should have error.");
                Assert.Equal("Could not parse type System.String(\"foo\") into System.Int32.", result.Error);
                Assert.Equal(default(double), result.Value);
            });
            Assert.Null(exception);
        }

        [Fact]
        public void
            Result_String_Int__Whose_Property_HasValue_Is_False__Flatmaps__Result_String_Double__Whose_Property_HasValue_Is_True_With_Null_OkSelector__Expects_ArgumentNullException() {
            var exception = Record.Exception(() => {
                var intParse = ResultParsers.Int("foo");
                var doubleParse = ResultParsers.Double("2");
                Func<int, Result<double, string>> rightselector = null;
                Func<string, string> errorSelector = null;
                var result = intParse.FlatMap(rightselector, s => s);

                Assert.True(result.HasError, "Result should have a error value.");
                Assert.False(result.HasValue, "Result should have error.");
                Assert.Equal("Could not parse type System.String(\"foo\") into System.Int32.", result.Error);
                Assert.Equal(default(double), result.Value);
            });
            Assert.Null(exception);
        }

        [Fact]
        public void
            Result_String_Int__Whose_Property_HasValue_Is_False__Flatmaps__Result_String_Double__Whose_Property_HasValue_Is_True_With_Null_OkSelector_And_Null_errorSelector__Expects_No_ArgumentNullException() {
            var exception = Record.Exception(() => {
                var intParse = ResultParsers.Int("foo");
                var doubleParse = ResultParsers.Double("2");
                Func<int, Result<double, string>> rightselector = null;
                Func<string, string> errorSelector = null;
                var result = intParse.FlatMap(rightselector, errorSelector);

                Assert.True(result.HasError, "Result should have a error value.");
                Assert.False(result.HasValue, "Result should have error.");
                Assert.Equal("Could not parse type System.String(\"foo\") into System.Int32.", result.Error);
                Assert.Equal(default(double), result.Value);
            });
            Assert.Null(exception);
        }

        [Fact]
        public void
            Result_String_Int__Whose_Property_HasValue_Is_False__Flatmaps__Result_String_Double__Whose_Property_True_Is_False__Expects_Result_String_Double__Whose_Property_HasValue_Is_True__With_error_Message_From_Second_result() {
            var rightExecuted = false;
            var errorExectued = false;
            var intParse = ResultParsers.Int("2");
            var doubleParse = ResultParsers.Double("2");
            var result = intParse.FlatMap(i => {
                rightExecuted = true;
                return doubleParse;
            }, s => {
                errorExectued = true;
                return "Failure " + s;
            });

            Assert.True(rightExecuted, "Ok should get exectued since intparse succeded..");
            Assert.False(errorExectued, "error should not be exectued since both intparse and doubleparse succeded.");
            Assert.False(result.HasError, "Result should not have a error value.");
            Assert.True(result.HasValue, "Result should have value.");
            Assert.Equal(default(string), result.Error);
            Assert.Equal(2, result.Value);
        }

        [Fact]
        public void
            Result_String_Int__Whose_Property_HasValue_Is_True__Flatmaps__Result_String_Double__Whose_Property_HasValue_Is_False__Expects_Result_String_Double__Whose_Property_HasValue_Is_False__With_error_Message_From_Second_result() {
            var intParse = ResultParsers.Int("2");
            var doubleParse = ResultParsers.Double("foo");
            var rightExecuted = false;
            var errorExectued = false;
            var result = intParse.FlatMap(i => {
                rightExecuted = true;
                return doubleParse;
            }, s => {
                errorExectued = true;
                return "Failure " + s;
            });

            Assert.True(rightExecuted, "Ok should get exectued since intparse did succeed.");
            Assert.True(errorExectued, "error should get exectued since intparse did succeed.");
            Assert.True(result.HasError, "Result should have a error value.");
            Assert.False(result.HasValue, "Result should have error.");
            Assert.Equal("Failure Could not parse type System.String(\"foo\") into System.Double.", result.Error);
            Assert.Equal(default(double), result.Value);
        }

        [Fact]
        public void
            Result_String_Int__Whose_Property_HasValue_Is_True__Flatmaps__Result_String_Double__Whose_Property_HasValue_Is_False_With_Null_errorSelector__Expects_ArgumentNullException() {
            Assert.Throws<ArgumentNullException>(() => {
                var intParse = ResultParsers.Int("2");
                var doubleParse = ResultParsers.Double("foo");
                Func<int, Result<string, double>> rightselector = null;
                Func<string, string> errorSelector = null;
                var result = intParse.FlatMap(i => doubleParse, errorSelector);

                Assert.True(result.HasError, "Result should have a error value.");
                Assert.False(result.HasValue, "Result should have error.");
                Assert.Equal("Failure Could not parse type System.String(\"foo\") into System.Double.", result.Error);
                Assert.Equal(default(double), result.Value);
            });
        }

        [Fact]
        public void
            Result_String_Int__Whose_Property_HasValue_Is_True__Flatmaps__Result_String_Double__Whose_Property_HasValue_Is_False_With_Null_OkSelector__Expects_No_ArgumentNullException() {
            Assert.Throws<ArgumentNullException>(() => {
                var intParse = ResultParsers.Int("2");
                Func<int, Result<double, string>> rightselector = null;
                Func<string, string> errorSelector = null;
                intParse.FlatMap(rightselector, s => s);
            });
        }

        [Fact]
        public void
            Result_String_Int__Whose_Property_HasValue_Is_True__Flatmaps__Result_String_Double__Whose_Property_HasValue_Is_False_With_Null_OkSelector_And_Null_errorSelector__Expects_No_ArgumentNullException() {
            Assert.Throws<ArgumentNullException>(() => {
                var intParse = ResultParsers.Int("2");
                Func<int, Result<double, string>> rightselector = null;
                Func<string, string> errorSelector = null;
                intParse.FlatMap(rightselector, errorSelector);
            });
        }

        [Fact]
        public void
            Result_String_Int__Whose_Property_HasValue_Is_True__Flatmaps__Result_String_Double__Whose_Property_HasValue_Is_True_With_Null_errorSelector__Expects_No_ArgumentNullException() {
            var exception = Record.Exception(() => {
                var rightExecuted = false;
                var errorExectued = false;
                var intParse = ResultParsers.Int("2");
                var doubleParse = ResultParsers.Double("2");
                Func<string, string> errorSelector = null;
                var result = intParse.FlatMap(i => {
                    rightExecuted = true;
                    return doubleParse;
                }, errorSelector);

                Assert.True(rightExecuted, "Ok should get exectued since intparse succeded..");
                Assert.False(errorExectued, "error should not be exectued since both intparse and doubleparse succeded.");
                Assert.False(result.HasError, "Result should not have a error value.");
                Assert.True(result.HasValue, "Result should have value.");
                Assert.Equal(default(string), result.Error);
                Assert.Equal(2, result.Value);
            });
            Assert.Null(exception);
        }

        [Fact]
        public void
            Result_String_Int__Whose_Property_HasValue_Is_True__Flatmaps__Result_String_Double__Whose_Property_HasValue_Is_True_With_Null_OkSelector__Expects_ArgumentNullException() {
            Assert.Throws<ArgumentNullException>(() => {
                var intParse = ResultParsers.Int("2");
                var doubleParse = ResultParsers.Double("2");
                Func<int, Result<double, string>> rightselector = null;
                Func<string, string> errorSelector = null;
                var result = intParse.FlatMap(rightselector, s => s);

                Assert.True(result.HasError, "Result should have a error value.");
                Assert.False(result.HasValue, "Result should have error.");
                Assert.Equal("Could not parse type System.String(\"foo\") into System.Int32.", result.Error);
                Assert.Equal(default(double), result.Value);
            });
        }

        [Fact]
        public void
            Result_String_Int__Whose_Property_HasValue_Is_True__Flatmaps__Result_String_Double__Whose_Property_HasValue_Is_True_With_Null_OkSelector_And_Null_errorSelector__Expects_No_ArgumentNullException() {
            Assert.Throws<ArgumentNullException>(() => {
                var intParse = ResultParsers.Int("2");
                var doubleParse = ResultParsers.Double("2");
                Func<int, Result<double, string>> rightselector = null;
                Func<string, string> errorSelector = null;
                var result = intParse.FlatMap(rightselector, errorSelector);

                Assert.True(result.HasError, "Result should have a error value.");
                Assert.False(result.HasValue, "Result should have error.");
                Assert.Equal("Could not parse type System.String(\"foo\") into System.Int32.", result.Error);
                Assert.Equal(default(double), result.Value);
            });
        }
    }
}
