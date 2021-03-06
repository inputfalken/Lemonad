using System;
using System.Threading.Tasks;
using Assertion;
using Lemonad.ErrorHandling.Extensions.AsyncResult;
using Xunit;

namespace Lemonad.ErrorHandling.Unit.Result.Tests {
    public class FullFlatMapAsyncResultSelectorTests {
        [Fact]
        public void Passing_Null_ErrorSelector_Throws() {
            Assert.Throws<ArgumentNullException>(
                AssertionUtilities.ErrorSelectorName,
                () => AssertionUtilities
                    .Division(2, 0)
                    .FullFlatMapAsync(x => AssertionUtilities.DivisionAsync(x, 2), (d, d1) => d + d1, null));
        }

        [Fact]
        public void Passing_Null_ResultSelector_Throws() {
            Assert.Throws<ArgumentNullException>(
                AssertionUtilities.ResultSelector,
                () => AssertionUtilities
                    .Division(2, 0)
                    .FullFlatMapAsync(x => AssertionUtilities.DivisionAsync(x, 2), (Func<double, double, double>) null,
                        s => s));
        }

        [Fact]
        public void Passing_Null_Selector_Throws() {
            Assert.Throws<ArgumentNullException>(
                AssertionUtilities.SelectorName,
                () => AssertionUtilities
                    .Division(2, 0)
                    .FullFlatMapAsync((Func<double, IAsyncResult<double, string>>) null, (d, d1) => d + d1, s => s));
        }

        [Fact]
        public async Task Result_With_Error_FlatmapsRS_Result_with_Error__Expects_Result_With_Error() {
            var flatSelectorExecuted = false;
            var resultSelectorExectued = false;
            var errorSelectorExecuted = false;
            await AssertionUtilities
                .Division(2, 0)
                .FullFlatMapAsync(x => {
                    flatSelectorExecuted = true;
                    return AssertionUtilities.DivisionAsync(x, 0);
                }, (y, x) => {
                    resultSelectorExectued = true;
                    return y + x;
                }, s => {
                    errorSelectorExecuted = true;
                    return s;
                }).AssertError("Can not divide '2' with '0'.");

            Assert.True(errorSelectorExecuted,
                "Errorselector should get exeuted since there is an error in the soruce.");
            Assert.False(flatSelectorExecuted,
                "The flatmapSelector should not get exectued if the source Result<T, TError> contains error.");
            Assert.False(resultSelectorExectued,
                "The resultSelector should not get exectued if the source Result<T, TError> contains error.");
        }

        [Fact]
        public async Task Result_With_Error_FlatmapsRS_Result_with_Value__Expects_Result_With_Error() {
            var flatSelectorExecuted = false;
            var resultSelectorExectued = false;
            var errorSelectorExecuted = false;
            await AssertionUtilities
                .Division(2, 0)
                .FullFlatMapAsync(x => {
                    flatSelectorExecuted = true;
                    return AssertionUtilities.DivisionAsync(x, 2);
                }, (y, x) => {
                    resultSelectorExectued = true;
                    return y + x;
                }, s => {
                    errorSelectorExecuted = true;
                    return s;
                })
                .AssertError("Can not divide '2' with '0'.");

            Assert.True(errorSelectorExecuted,
                "Errorselector should get exeuted since there is an error in the soruce.");
            Assert.False(flatSelectorExecuted,
                "The flatmapSelector should not get exectued if the source Result<T, TError> contains error.");
            Assert.False(resultSelectorExectued,
                "The resultSelector should not get exectued if the source Result<T, TError> contains error.");
        }

        [Fact]
        public async Task Result_With_Value_FlatmapsRS_Result_with_Error__Expects_Result_With_Error() {
            var flatSelectorExecuted = false;
            var resultSelectorExectued = false;
            var errorSelectorExecuted = false;
            await AssertionUtilities
                .Division(2, 2)
                .FullFlatMapAsync(x => {
                    flatSelectorExecuted = true;
                    return AssertionUtilities.DivisionAsync(x, 0);
                }, (y, x) => {
                    resultSelectorExectued = true;
                    return y + x;
                }, s => {
                    errorSelectorExecuted = true;
                    return s;
                })
                .AssertError("Can not divide '1' with '0'.");

            Assert.False(errorSelectorExecuted,
                "Errorselector should not get exeuted since the errror came from the result given to the flatselector.");
            Assert.True(flatSelectorExecuted, "The flatmapSelector should get exectued.");
            Assert.False(resultSelectorExectued,
                "The resultSelector should not get executed since flatselector result failed.");
        }

        [Fact]
        public async Task Result_With_Value_FlatmapsRS_Result_with_Value__Expects_Result_With_Value() {
            var flatSelectorExecuted = false;
            var resultSelectorExectued = false;
            var errorSelectorExecuted = false;
            await AssertionUtilities
                .Division(2, 2)
                .FullFlatMapAsync(x => {
                    flatSelectorExecuted = true;
                    return AssertionUtilities.DivisionAsync(x, 2);
                }, (y, x) => {
                    resultSelectorExectued = true;
                    return y + x;
                }, s => {
                    errorSelectorExecuted = true;
                    return s;
                }).AssertValue(1.5d);
            Assert.True(flatSelectorExecuted, "Flatmapselecotr should get executed.");
            Assert.True(resultSelectorExectued,
                "ResultSelector should get executed since both source and the result from flatmapselector contains values.");
            Assert.False(errorSelectorExecuted,
                "Erroselector should not get executed since both source and the result from flatmapselector contains values.");
        }
    }
}