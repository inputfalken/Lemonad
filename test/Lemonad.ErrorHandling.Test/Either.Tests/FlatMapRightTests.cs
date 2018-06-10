using Xunit;

namespace Lemonad.ErrorHandling.Test.Either.Tests {
    public class FlatMapRightTests {
        [Fact]
        public void
            Either_String_Int__Whose_Property_IsRight_Is_False__Flatmaps__Either_String_Double__Whose_Property_IsRight_Is_False__Expects_Either_String_Double__Whose_Property_IsRight_Is_False__With_Left_Message_From_First_either() {
            var intParse = EitherParsers.Int("foo");
            var doubleParse = EitherParsers.Double("foo");
            var rightExecuted = false;
            var leftExectued = false;
            var result = intParse.FlatMapRight(i => {
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
            var result = intParse.FlatMapRight(i => {
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
            Either_String_Int__Whose_Property_IsRight_Is_False__Flatmaps__Either_String_Double__Whose_Property_True_Is_False__Expects_Either_String_Double__Whose_Property_IsRight_Is_True__With_Left_Message_From_Second_either() {
            var rightExecuted = false;
            var leftExectued = false;
            var intParse = EitherParsers.Int("2");
            var doubleParse = EitherParsers.Double("2");
            var result = intParse.FlatMapRight(i => {
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
            var result = intParse.FlatMapRight(i => {
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
    }
}