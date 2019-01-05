using System;
using Assertion;
using Xunit;

namespace Lemonad.ErrorHandling.Unit.Result.Tests {
    public class FullFlatMapTests {
        [Fact]
        public void Passing_Null_ErrorSelector_Throws() {
            Assert.Throws<ArgumentNullException>(
                AssertionUtilities.ErrorSelectorName,
                () => AssertionUtilities
                    .Division(2, 0)
                    .FullFlatMap(x => AssertionUtilities.Division(x, 2), null));
        }

        [Fact]
        public void Passing_Null_Selector_Throws() {
            Assert.Throws<ArgumentNullException>(
                AssertionUtilities.SelectorName,
                () => AssertionUtilities
                    .Division(2, 0)
                    .FullFlatMap((Func<double, IResult<double, string>>) null, s => s)
            );
        }

        [Fact]
        public void Result_With_Error_Flatmaps_Result_with_Error__Expects_Result_With_Error() {
            var flatSelectorExecuted = false;
            var errorSelectorExecuted = false;
            AssertionUtilities
                .Division(2, 0)
                .FullFlatMap(x => {
                    flatSelectorExecuted = true;
                    return AssertionUtilities.Division(x, 0);
                }, s => {
                    errorSelectorExecuted = true;
                    return s;
                })
                .AssertError("Can not divide '2' with '0'.");

            Assert.True(errorSelectorExecuted,
                "Errorselector should get exeuted since there is an error in the source.");
            Assert.False(flatSelectorExecuted,
                "The flatmap selector should not get exectued if the source Result<T, TError> contains error.");
        }

        [Fact]
        public void Result_With_Error_Flatmaps_Result_with_Value__Expects_Result_With_Error() {
            var flatSelectorExecuted = false;
            var errorSelectorExecuted = false;
            AssertionUtilities
                .Division(2, 0)
                .FullFlatMap(x => {
                    flatSelectorExecuted = true;
                    return AssertionUtilities.Division(x, 2);
                }, s => {
                    errorSelectorExecuted = true;
                    return s;
                })
                .AssertError("Can not divide '2' with '0'.");

            Assert.True(errorSelectorExecuted,
                "Errorselector should get exeuted since there is an error in the source..");
            Assert.False(flatSelectorExecuted,
                "The flatmap selector should not get exectued if the source Result<T, TError> contains error.");
        }

        [Fact]
        public void Result_With_Value_Flatmaps_Result_with_Error__Expects_Result_With_Error() {
            var flatSelectorExecuted = false;
            var errorSelectorExecuted = false;
            AssertionUtilities
                .Division(2, 2)
                .FullFlatMap(x => {
                    flatSelectorExecuted = true;
                    return AssertionUtilities.Division(x, 0);
                }, s => {
                    errorSelectorExecuted = true;
                    return s;
                }).AssertError("Can not divide '1' with '0'.");

            Assert.False(errorSelectorExecuted,
                "Errorselector should not get exeuted since the errror came from the result given to the flatselector.");
            Assert.True(flatSelectorExecuted, "The flatmapSelector should get exectued.");
        }

        [Fact]
        public void Result_With_Value_Flatmaps_Result_with_Value__Expects_Result_With_Value() {
            var flatSelectorExecuted = false;
            var errorSelectorExecuted = false;
            AssertionUtilities
                .Division(2, 2)
                .FullFlatMap(x => {
                    flatSelectorExecuted = true;
                    return AssertionUtilities.Division(x, 2);
                }, s => {
                    errorSelectorExecuted = true;
                    return s;
                })
                .AssertValue(0.5d);
            Assert.True(flatSelectorExecuted, "flatmapselector should get executed.");
            Assert.False(errorSelectorExecuted, "Errorselector should not get exeuted.");
        }
    }
}