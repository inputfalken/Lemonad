using System;
using System.Threading.Tasks;
using Assertion;
using Lemonad.ErrorHandling.Extensions.AsyncResult;
using Xunit;

namespace Lemonad.ErrorHandling.Unit.AsyncResult.Tests {
    public class FlattenAsyncTests {
        [Fact]
        public async Task Passing_Null_Selector_Throws() =>
            Assert.Throws<ArgumentNullException>(
                AssertionUtilities.SelectorName,
                () => AssertionUtilities.DivisionAsync(20, 2).FlattenAsync<string>(null)
            );

        [Fact]
        public async Task Passing_Null_Selector_With_ErrorSelector_Overload_Throws() =>
            Assert.Throws<ArgumentNullException>(
                AssertionUtilities.SelectorName,
                () => AssertionUtilities.DivisionAsync(20, 2).FlattenAsync<string, int>(null, i => $"{i}")
            );

        [Fact]
        public async Task Passing_Null_ErrorSelector() =>
            Assert.Throws<ArgumentNullException>(
                AssertionUtilities.ErrorSelectorName,
                () => AssertionUtilities.DivisionAsync(20, 2)
                    .FlattenAsync(d => AssertionUtilities.DivisionAsync(d, 2), null)
            );

        [Fact]
        public async Task Result_With_Error_Flatmaps_Result_with_Error__Expects_Result_With_Error() {
            var flatSelectorExecuted = false;
            var errorSelectorExecuted = false;
            await AssertionUtilities
                .DivisionAsync(2, 0)
                .FlattenAsync(x => {
                    flatSelectorExecuted = true;
                    return AssertionUtilities.DivisionAsync(x, 0);
                }, s => {
                    errorSelectorExecuted = true;
                    return s;
                }).AssertError("Can not divide '2' with '0'.");

            Assert.False(errorSelectorExecuted,
                "Errorselector should not get exeuted since there is an error in the source.");
            Assert.False(flatSelectorExecuted);
        }

        [Fact]
        public async Task
            Result_With_Error_Flatmaps_Result_with_Error_Without_ErrorSelector__Expects_Result_With_Error() {
            var flatSelectorExecuted = false;
            await AssertionUtilities
                .DivisionAsync(2, 0)
                .FlattenAsync(x => {
                    flatSelectorExecuted = true;
                    return AssertionUtilities.DivisionAsync(x, 0);
                })
                .AssertError("Can not divide '2' with '0'.");

            Assert.False(flatSelectorExecuted,
                "The flatmap selector should not get exectued if the source Result<T, TError> contains error.");
        }

        [Fact]
        public async Task Result_With_Error_Flatmaps_Result_with_Value__Expects_Result_With_Error() {
            var flatSelectorExecuted = false;
            var errorSelectorExecuted = false;
            await AssertionUtilities
                .DivisionAsync(2, 0)
                .FlattenAsync(x => {
                    flatSelectorExecuted = true;
                    return AssertionUtilities.DivisionAsync(x, 2);
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
        public async Task
            Result_With_Error_Flattens_Result_with_Value_Without_ErrorSelector__Expects_Result_With_Error() {
            var flatSelectorExecuted = false;
            await AssertionUtilities
                .DivisionAsync(2, 0)
                .FlattenAsync(x => {
                    flatSelectorExecuted = true;
                    return AssertionUtilities.DivisionAsync(x, 2);
                })
                .AssertError("Can not divide '2' with '0'.");

            Assert.False(flatSelectorExecuted,
                "The flatmap selector should not get exectued if the source Result<T, TError> contains error.");
        }

        [Fact]
        public async Task
            Result_With_Value_Flatten_Result_with_Error_Without_ErrorSelector__Expects_Result_With_Error() {
            var flatSelectorExecuted = false;
            await AssertionUtilities
                .DivisionAsync(2, 2)
                .FlattenAsync(x => {
                    flatSelectorExecuted = true;
                    return AssertionUtilities.DivisionAsync(x, 0);
                }).AssertError("Can not divide '1' with '0'.");

            Assert.True(flatSelectorExecuted, "The flatmapSelector should get exectued.");
        }

        [Fact]
        public async Task Result_With_Value_Flatmaps_Result_with_Value__Expects_Result_With_Value() {
            var flatSelectorExecuted = false;
            var errorSelectorExecuted = false;
            await AssertionUtilities
                .DivisionAsync(2, 2)
                .FlattenAsync(x => {
                    flatSelectorExecuted = true;
                    return AssertionUtilities.DivisionAsync(x, 2);
                }, s => {
                    errorSelectorExecuted = true;
                    return s;
                })
                .AssertValue(1);
            Assert.True(flatSelectorExecuted, "flatmapselector should get executed.");
            Assert.False(errorSelectorExecuted, "Errorselector should not get exeuted.");
        }

        [Fact]
        public async Task
            Result_With_Value_Flatmaps_Result_with_Value_Without_ErrorSelector__Expects_Result_With_Value() {
            var flatSelectorExecuted = false;
            await AssertionUtilities
                .DivisionAsync(2, 2)
                .FlattenAsync(x => {
                    flatSelectorExecuted = true;
                    return AssertionUtilities.DivisionAsync(x, 2);
                })
                .AssertValue(1);
            Assert.True(flatSelectorExecuted, "flatmapselector should get executed.");
        }

        [Fact]
        public async Task Result_With_Value_Flattens_Result_with_Error__Expects_Result_With_Error() {
            var flatSelectorExecuted = false;
            var errorSelectorExecuted = false;
            await AssertionUtilities
                .DivisionAsync(2, 2)
                .FlattenAsync(x => {
                    flatSelectorExecuted = true;
                    return AssertionUtilities.DivisionAsync(x, 0);
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
        public async Task
            Result_With_Value_Flattens_Result_with_Error_Without_ErrorSelector__Expects_Result_With_Error() {
            var flatSelectorExecuted = false;
            await AssertionUtilities
                .DivisionAsync(2, 2)
                .FlattenAsync(x => {
                    flatSelectorExecuted = true;
                    return AssertionUtilities.DivisionAsync(x, 0);
                })
                .AssertError("Can not divide '1' with '0'.");

            Assert.True(flatSelectorExecuted,
                "The flatselector should not get executed since flatselector result failed.");
        }
    }
}