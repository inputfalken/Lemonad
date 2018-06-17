using System;
using Xunit;

namespace Lemonad.ErrorHandling.Test.Either.Tests {
    public class RightWhenFlatMap {
        [Fact]
        public void
            Either_String_Int__Whose_Property_IsRight_Is_False__Flatmaps__Either_String_Double__Whose_Property_IsRight_Is_False__Expects_Either_String_Double__Whose_Property_IsRight_Is_False__With_Left_Message_From_First_either() {
            var intParse = EitherParsers.Int("foo");
            var doubleParse = EitherParsers.Double("foo");
            var rightExecuted = false;
            var leftExectued = false;
            var result = intParse.Flatten(i => {
                rightExecuted = true;
                return doubleParse;
            }, s => {
                leftExectued = true;
                return "Failure" + s;
            });

            Assert.False(rightExecuted, "Right should not get exectued since intparse did not succeed.");
            Assert.False(leftExectued, "Left should not be exectued since intparse did not succeed.");
            Assert.True(result.IsLeft, "Either should have a left value.");
            Assert.False(result.IsRight, "Either should not have a right value.");
            Assert.Equal("Could not parse type System.String(\"foo\") into System.Int32.", result.Left);
            Assert.Equal(default(double), result.Right);
        }

        [Fact]
        public void
            Either_String_Int__Whose_Property_IsRight_Is_False__Flatmaps__Either_String_Double__Whose_Property_IsRight_Is_False__Expects_Either_String_Double__Whose_Property_IsRight_Is_True__With_Left_Message_From_Second_either() {
            var rightExecuted = false;
            var leftExectued = false;
            var intParse = EitherParsers.Int("foo");
            var doubleParse = EitherParsers.Double("2");
            var result = intParse.Flatten(i => {
                rightExecuted = true;
                return doubleParse;
            }, s => {
                leftExectued = true;
                return "Failure " + s;
            });

            Assert.False(rightExecuted, "Right should not get exectued since intparse did not succeed.");
            Assert.False(leftExectued, "Left should not be exectued since intparse did not succeed.");
            Assert.True(result.IsLeft, "Either should have a left value.");
            Assert.False(result.IsRight, "Either should not have a right value.");
            Assert.Equal("Could not parse type System.String(\"foo\") into System.Int32.", result.Left);
            Assert.Equal(default(double), result.Right);
        }

        [Fact]
        public void
            Either_String_Int__Whose_Property_IsRight_Is_False__Flatmaps__Either_String_Double__Whose_Property_IsRight_Is_False_With_Null_LeftSelector__Expects_No_ArgumentNullException() {
            var exception = Record.Exception(() => {
                var intParse = EitherParsers.Int("foo");
                var doubleParse = EitherParsers.Double("foo");
                Func<string, string> leftSelector = null;
                var result = intParse.Flatten(i => { return doubleParse; }, leftSelector);

                Assert.True(result.IsLeft, "Either should have a left value.");
                Assert.False(result.IsRight, "Either should not have a right value.");
                Assert.Equal("Could not parse type System.String(\"foo\") into System.Int32.", result.Left);
                Assert.Equal(default(double), result.Right);
            });
            Assert.Null(exception);
        }

        [Fact]
        public void
            Either_String_Int__Whose_Property_IsRight_Is_False__Flatmaps__Either_String_Double__Whose_Property_IsRight_Is_False_With_Null_RightSelector__Expects_No_ArgumentNullException() {
            var exception = Record.Exception(() => {
                var intParse = EitherParsers.Int("foo");
                var doubleParse = EitherParsers.Double("foo");
                Func<int, Either<string, double>> rightselector = null;
                var result = intParse.Flatten(rightselector, s => { return "Failure" + s; });

                Assert.True(result.IsLeft, "Either should have a left value.");
                Assert.False(result.IsRight, "Either should not have a right value.");
                Assert.Equal("Could not parse type System.String(\"foo\") into System.Int32.", result.Left);
                Assert.Equal(default(double), result.Right);
            });
            Assert.Null(exception);
        }

        [Fact]
        public void
            Either_String_Int__Whose_Property_IsRight_Is_False__Flatmaps__Either_String_Double__Whose_Property_IsRight_Is_False_With_Null_RightSelector_And_Null_LeftSelector__Expects_No_ArgumentNullException() {
            var intParse = EitherParsers.Int("foo");
            Func<int, Either<string, double>> rightselector = null;
            Func<string, string> leftSelector = null;
            var result = intParse.Flatten(rightselector, leftSelector);

            Assert.True(result.IsLeft, "Either should have a left value.");
            Assert.False(result.IsRight, "Either should not have a right value.");
            Assert.Equal("Could not parse type System.String(\"foo\") into System.Int32.", result.Left);
            Assert.Equal(default(double), result.Right);
        }

        [Fact]
        public void
            Either_String_Int__Whose_Property_IsRight_Is_False__Flatmaps__Either_String_Double__Whose_Property_IsRight_Is_True_With_Null_LeftSelector__Expects_No_ArgumentNullException() {
            var exception = Record.Exception(() => {
                var intParse = EitherParsers.Int("foo");
                var doubleParse = EitherParsers.Double("2");
                Func<int, Either<string, double>> rightselector = null;
                Func<string, string> leftSelector = null;
                var result = intParse.Flatten(x => doubleParse, leftSelector);

                Assert.True(result.IsLeft, "Either should have a left value.");
                Assert.False(result.IsRight, "Either should not have a right value.");
                Assert.Equal("Could not parse type System.String(\"foo\") into System.Int32.", result.Left);
                Assert.Equal(default(double), result.Right);
            });
            Assert.Null(exception);
        }

        [Fact]
        public void
            Either_String_Int__Whose_Property_IsRight_Is_False__Flatmaps__Either_String_Double__Whose_Property_IsRight_Is_True_With_Null_RightSelector__Expects_ArgumentNullException() {
            var exception = Record.Exception(() => {
                var intParse = EitherParsers.Int("foo");
                var doubleParse = EitherParsers.Double("2");
                Func<int, Either<string, double>> rightselector = null;
                Func<string, string> leftSelector = null;
                var result = intParse.Flatten(rightselector, s => s);

                Assert.True(result.IsLeft, "Either should have a left value.");
                Assert.False(result.IsRight, "Either should not have a right value.");
                Assert.Equal("Could not parse type System.String(\"foo\") into System.Int32.", result.Left);
                Assert.Equal(default(double), result.Right);
            });
            Assert.Null(exception);
        }

        [Fact]
        public void
            Either_String_Int__Whose_Property_IsRight_Is_False__Flatmaps__Either_String_Double__Whose_Property_IsRight_Is_True_With_Null_RightSelector_And_Null_LeftSelector__Expects_No_ArgumentNullException() {
            var exception = Record.Exception(() => {
                var intParse = EitherParsers.Int("foo");
                var doubleParse = EitherParsers.Double("2");
                Func<int, Either<string, double>> rightselector = null;
                Func<string, string> leftSelector = null;
                var result = intParse.Flatten(rightselector, leftSelector);

                Assert.True(result.IsLeft, "Either should have a left value.");
                Assert.False(result.IsRight, "Either should not have a right value.");
                Assert.Equal("Could not parse type System.String(\"foo\") into System.Int32.", result.Left);
                Assert.Equal(default(double), result.Right);
            });
            Assert.Null(exception);
        }

        [Fact]
        public void
            Either_String_Int__Whose_Property_IsRight_Is_False__Flatmaps__Either_String_Double__Whose_Property_True_Is_False__Expects_Either_String_Double__Whose_Property_IsRight_Is_True__With_Left_Message_From_Second_either() {
            var rightExecuted = false;
            var leftExectued = false;
            var intParse = EitherParsers.Int("2");
            var doubleParse = EitherParsers.Double("2");
            var result = intParse.Flatten(i => {
                rightExecuted = true;
                return doubleParse;
            }, s => {
                leftExectued = true;
                return "Failure " + s;
            });

            Assert.True(rightExecuted, "Right should get exectued since intparse succeded..");
            Assert.False(leftExectued, "Left should not be exectued since both intparse and doubleparse succeded.");
            Assert.False(result.IsLeft, "Either should not have a left value.");
            Assert.True(result.IsRight, "Either should have a right value.");
            Assert.Equal(default(string), result.Left);
            Assert.Equal(2, result.Right);
        }

        [Fact]
        public void
            Either_String_Int__Whose_Property_IsRight_Is_True__Flatmaps__Either_String_Double__Whose_Property_IsRight_Is_False__Expects_Either_String_Double__Whose_Property_IsRight_Is_False__With_Left_Message_From_Second_either() {
            var intParse = EitherParsers.Int("2");
            var doubleParse = EitherParsers.Double("foo");
            var rightExecuted = false;
            var leftExectued = false;
            var result = intParse.Flatten(i => {
                rightExecuted = true;
                return doubleParse;
            }, s => {
                leftExectued = true;
                return "Failure " + s;
            });

            Assert.True(rightExecuted, "Right should get exectued since intparse did succeed.");
            Assert.True(leftExectued, "Left should get exectued since intparse did succeed.");
            Assert.True(result.IsLeft, "Either should have a left value.");
            Assert.False(result.IsRight, "Either should not have a right value.");
            Assert.Equal("Failure Could not parse type System.String(\"foo\") into System.Double.", result.Left);
            Assert.Equal(default(double), result.Right);
        }

        [Fact]
        public void
            Either_String_Int__Whose_Property_IsRight_Is_True__Flatmaps__Either_String_Double__Whose_Property_IsRight_Is_False_With_Null_LeftSelector__Expects_ArgumentNullException() {
            Assert.Throws<ArgumentNullException>(() => {
                var intParse = EitherParsers.Int("2");
                var doubleParse = EitherParsers.Double("foo");
                Func<int, Either<string, double>> rightselector = null;
                Func<string, string> leftSelector = null;
                var result = intParse.Flatten(i => doubleParse, leftSelector);

                Assert.True(result.IsLeft, "Either should have a left value.");
                Assert.False(result.IsRight, "Either should not have a right value.");
                Assert.Equal("Failure Could not parse type System.String(\"foo\") into System.Double.", result.Left);
                Assert.Equal(default(double), result.Right);
            });
        }

        [Fact]
        public void
            Either_String_Int__Whose_Property_IsRight_Is_True__Flatmaps__Either_String_Double__Whose_Property_IsRight_Is_False_With_Null_RightSelector__Expects_No_ArgumentNullException() {
            Assert.Throws<ArgumentNullException>(() => {
                var intParse = EitherParsers.Int("2");
                Func<int, Either<string, double>> rightselector = null;
                Func<string, string> leftSelector = null;
                intParse.Flatten(rightselector, s => s);
            });
        }

        [Fact]
        public void
            Either_String_Int__Whose_Property_IsRight_Is_True__Flatmaps__Either_String_Double__Whose_Property_IsRight_Is_False_With_Null_RightSelector_And_Null_LeftSelector__Expects_No_ArgumentNullException() {
            Assert.Throws<ArgumentNullException>(() => {
                var intParse = EitherParsers.Int("2");
                Func<int, Either<string, double>> rightselector = null;
                Func<string, string> leftSelector = null;
                intParse.Flatten(rightselector, leftSelector);
            });
        }

        [Fact]
        public void
            Either_String_Int__Whose_Property_IsRight_Is_True__Flatmaps__Either_String_Double__Whose_Property_IsRight_Is_True_With_Null_LeftSelector__Expects_No_ArgumentNullException() {
            var exception = Record.Exception(() => {
                var rightExecuted = false;
                var leftExectued = false;
                var intParse = EitherParsers.Int("2");
                var doubleParse = EitherParsers.Double("2");
                Func<string, string> leftSelector = null;
                var result = intParse.Flatten(i => {
                    rightExecuted = true;
                    return doubleParse;
                }, leftSelector);

                Assert.True(rightExecuted, "Right should get exectued since intparse succeded..");
                Assert.False(leftExectued, "Left should not be exectued since both intparse and doubleparse succeded.");
                Assert.False(result.IsLeft, "Either should not have a left value.");
                Assert.True(result.IsRight, "Either should have a right value.");
                Assert.Equal(default(string), result.Left);
                Assert.Equal(2, result.Right);
            });
            Assert.Null(exception);
        }

        [Fact]
        public void
            Either_String_Int__Whose_Property_IsRight_Is_True__Flatmaps__Either_String_Double__Whose_Property_IsRight_Is_True_With_Null_RightSelector__Expects_ArgumentNullException() {
            Assert.Throws<ArgumentNullException>(() => {
                var intParse = EitherParsers.Int("2");
                var doubleParse = EitherParsers.Double("2");
                Func<int, Either<string, double>> rightselector = null;
                Func<string, string> leftSelector = null;
                var result = intParse.Flatten(rightselector, s => s);

                Assert.True(result.IsLeft, "Either should have a left value.");
                Assert.False(result.IsRight, "Either should not have a right value.");
                Assert.Equal("Could not parse type System.String(\"foo\") into System.Int32.", result.Left);
                Assert.Equal(default(double), result.Right);
            });
        }

        [Fact]
        public void
            Either_String_Int__Whose_Property_IsRight_Is_True__Flatmaps__Either_String_Double__Whose_Property_IsRight_Is_True_With_Null_RightSelector_And_Null_LeftSelector__Expects_No_ArgumentNullException() {
            Assert.Throws<ArgumentNullException>(() => {
                var intParse = EitherParsers.Int("2");
                var doubleParse = EitherParsers.Double("2");
                Func<int, Either<string, double>> rightselector = null;
                Func<string, string> leftSelector = null;
                var result = intParse.Flatten(rightselector, leftSelector);

                Assert.True(result.IsLeft, "Either should have a left value.");
                Assert.False(result.IsRight, "Either should not have a right value.");
                Assert.Equal("Could not parse type System.String(\"foo\") into System.Int32.", result.Left);
                Assert.Equal(default(double), result.Right);
            });
        }
    }
}