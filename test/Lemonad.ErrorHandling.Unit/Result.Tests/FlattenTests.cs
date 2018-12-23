using Xunit;

namespace Lemonad.ErrorHandling.Unit.Result.Tests {
    public class FlattenTests {
        [Fact]
        public void Result_With_Error_Flatmaps_Result_with_Error__Expects_Result_With_Error() {
            var flatSelectorExecuted = false;
            var errorSelectorExecuted = false;
            AssertionUtilities
                .Division(2, 0)
                .Flatten(x => {
                    flatSelectorExecuted = true;
                    return AssertionUtilities.Division(x, 0);
                }, s => {
                    errorSelectorExecuted = true;
                    return s;
                }).AssertError("Can not divide '2' with '0'.");

            Assert.False(errorSelectorExecuted,
                "Errorselector should not get exeuted since there is an error in the source.");
            Assert.False(flatSelectorExecuted,
                "The flatmap selector should not get exectued if the sourcegeiEither. Result<T, TError> contains error.");
        }

        [Fact]
        public void Result_With_Error_Flatmaps_Result_with_Error_Without_ErrorSelector__Expects_Result_With_Error() {
            var flatSelectorExecuted = false;
            AssertionUtilities
                .Division(2, 0)
                .Flatten(x => {
                    flatSelectorExecuted = true;
                    return AssertionUtilities.Division(x, 0);
                })
                .AssertError("Can not divide '2' with '0'.");

            Assert.False(flatSelectorExecuted,
                "The flatmap selector should not get exectued if the source Result<T, TError> contains error.");
        }

        [Fact]
        public void Result_With_Error_Flatmaps_Result_with_Value__Expects_Result_With_Error() {
            var flatSelectorExecuted = false;
            var errorSelectorExecuted = false;
            AssertionUtilities
                .Division(2, 0)
                .Flatten(x => {
                    flatSelectorExecuted = true;
                    return AssertionUtilities.Division(x, 2);
                }, s => {
                    errorSelectorExecuted = true;
                    return s;
                })
                .AssertError("Can not divide '2' with '0'.");

            Assert.False(errorSelectorExecuted,
                "Errorselector should not get exeuted since there is an error in the source.");
            Assert.False(flatSelectorExecuted,
                "The flatmap selector should not get exectued if the source Result<T, TError> contains error.");
        }

        [Fact]
        public void Result_With_Error_Flatmaps_Result_with_Value_Without_ErrorSelector__Expects_Result_With_Error() {
            var flatSelectorExecuted = false;
            AssertionUtilities
                .Division(2, 0)
                .Flatten(x => {
                    flatSelectorExecuted = true;
                    return AssertionUtilities.Division(x, 2);
                })
                .AssertError("Can not divide '2' with '0'.");

            Assert.False(flatSelectorExecuted,
                "The flatmap selector should not get exectued if the source Result<T, TError> contains error.");
        }

        [Fact]
        public void Result_With_Value_Flatmaps_Result_with_Error__Expects_Result_With_Error() {
            var flatSelectorExecuted = false;
            var errorSelectorExecuted = false;
            AssertionUtilities
                .Division(2, 2)
                .FlatMap(x => {
                    flatSelectorExecuted = true;
                    return AssertionUtilities.Division(x, 0);
                }, s => {
                    errorSelectorExecuted = true;
                    return s;
                }).AssertError("Can not divide '1' with '0'.");

            Assert.True(errorSelectorExecuted,
                "Errorselector should not exeuted since the errror came from the result given to the flatselector.");
            Assert.True(flatSelectorExecuted, "The flatmapSelector should get exectued.");
        }

        [Fact]
        public void Result_With_Value_Flatmaps_Result_with_Error_Without_ErrorSelector__Expects_Result_With_Error() {
            var flatSelectorExecuted = false;
            AssertionUtilities
                .Division(2, 2)
                .FlatMap(x => {
                    flatSelectorExecuted = true;
                    return AssertionUtilities.Division(x, 0);
                }).AssertError("Can not divide '1' with '0'.");

            Assert.True(flatSelectorExecuted, "The flatmapSelector should get exectued.");
        }

        [Fact]
        public void Result_With_Value_Flatmaps_Result_with_Value__Expects_Result_With_Value() {
            var flatSelectorExecuted = false;
            var errorSelectorExecuted = false;
            AssertionUtilities
                .Division(2, 2)
                .Flatten(x => {
                    flatSelectorExecuted = true;
                    return AssertionUtilities.Division(x, 2);
                }, s => {
                    errorSelectorExecuted = true;
                    return s;
                })
                .AssertValue(1);
            Assert.True(flatSelectorExecuted, "flatmapselector should get executed.");
            Assert.False(errorSelectorExecuted, "Errorselector should not get exeuted.");
        }

        [Fact]
        public void Result_With_Value_Flatmaps_Result_with_Value_Without_ErrorSelector__Expects_Result_With_Value() {
            var flatSelectorExecuted = false;
            AssertionUtilities
                .Division(2, 2)
                .Flatten(x => {
                    flatSelectorExecuted = true;
                    return AssertionUtilities.Division(x, 2);
                })
                .AssertValue(1);
            Assert.True(flatSelectorExecuted, "flatmapselector should get executed.");
        }

        [Fact]
        public void Result_With_Value_FlatmapsRS_Result_with_Error__Expects_Result_With_Error() {
            var flatSelectorExecuted = false;
            var errorSelectorExecuted = false;
            AssertionUtilities
                .Division(2, 2)
                .Flatten(x => {
                    flatSelectorExecuted = true;
                    return AssertionUtilities.Division(x, 0);
                }, s => {
                    errorSelectorExecuted = true;
                    return s;
                })
                .AssertError("Can not divide '1' with '0'.");

            Assert.True(errorSelectorExecuted,
                "Errorselector should get exeuted since the errror came from the result given to the flatselector.");
            Assert.True(flatSelectorExecuted,
                "The flatselector should not get executed since flatselector result failed.");
        }

        [Fact]
        public void Result_With_Value_FlatmapsRS_Result_with_Error_Without_ErrorSelector__Expects_Result_With_Error() {
            var flatSelectorExecuted = false;
            AssertionUtilities
                .Division(2, 2)
                .Flatten(x => {
                    flatSelectorExecuted = true;
                    return AssertionUtilities.Division(x, 0);
                })
                .AssertError("Can not divide '1' with '0'.");

            Assert.True(flatSelectorExecuted,
                "The flatselector should not get executed since flatselector result failed.");
        }
    }
}