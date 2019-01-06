using System;
using System.Threading.Tasks;
using Assertion;
using Lemonad.ErrorHandling.Extensions.AsyncResult;
using Xunit;

namespace Lemonad.ErrorHandling.Unit.Result.Tests {
    public class FlatMapAsyncTests {
        [Fact]
        public void Passing_Null_ResultSelector_Throws()
            => Assert.Throws<ArgumentNullException>(
                AssertionUtilities.ResultSelector,
                () => AssertionUtilities.Division(10, 2)
                    .FlatMapAsync(
                        s => AssertionUtilities.DivisionAsync(s, 2),
                        (Func<double, double, double>) null
                    )
            );

        [Fact]
        public void Passing_Null_Selector_Throws()
            => Assert.Throws<ArgumentNullException>(
                AssertionUtilities.SelectorName,
                () => AssertionUtilities.Division(10, 2)
                    .FlatMapAsync(
                        (Func<double, IAsyncResult<double, string>>) null
                    )
            );

        [Fact]
        public void Passing_Null_Selector_With_ResultSelector_Overload_Throws()
            => Assert.Throws<ArgumentNullException>(
                AssertionUtilities.SelectorName,
                () => AssertionUtilities.Division(10, 2)
                    .FlatMapAsync(
                        (Func<double, IAsyncResult<double, string>>) null,
                        (d, d1) => d + d1
                    )
            );

        [Fact]
        public async Task Result_With_Error_Flatmaps_Result_with_Error__Expects_Result_With_Error() {
            var flatSelectorExecuted = false;
            await AssertionUtilities
                .Division(2, 0)
                .FlatMapAsync(x => {
                    flatSelectorExecuted = true;
                    return AssertionUtilities.DivisionAsync(x, 0);
                }).AssertError("Can not divide '2' with '0'.");

            Assert.False(flatSelectorExecuted,
                "The flatmap selector should not get exectued if the source Result<T, TError> contains error.");
        }

        [Fact]
        public async Task Result_With_Error_Flatmaps_Result_with_Value__Expects_Result_With_Error() {
            var flatSelectorExecuted = false;
            await AssertionUtilities
                .Division(2, 0)
                .FlatMapAsync(x => {
                    flatSelectorExecuted = true;
                    return AssertionUtilities.DivisionAsync(x, 2);
                })
                .AssertError("Can not divide '2' with '0'.");

            Assert.False(flatSelectorExecuted,
                "The flatmap selector should not get exectued if the source Result<T, TError> contains error.");
        }

        [Fact]
        public async Task Result_With_Error_FlatmapsRS_Result_with_Error__Expects_Result_With_Error() {
            var flatSelectorExecuted = false;
            var resultSelectorExectued = false;
            await AssertionUtilities
                .Division(2, 0)
                .FlatMapAsync(x => {
                    flatSelectorExecuted = true;
                    return AssertionUtilities.DivisionAsync(x, 0);
                }, (y, x) => {
                    resultSelectorExectued = true;
                    return y + x;
                })
                .AssertError("Can not divide '2' with '0'.");

            Assert.False(flatSelectorExecuted,
                "The flatmapSelector should not get exectued if the source Result<T, TError> contains error.");
            Assert.False(resultSelectorExectued,
                "The resultSelector should not get exectued if the source Result<T, TError> contains error.");
        }

        [Fact]
        public async Task Result_With_Error_FlatmapsRS_Result_with_Value__Expects_Result_With_Error() {
            var flatSelectorExecuted = false;
            var resultSelectorExectued = false;
            await AssertionUtilities
                .Division(2, 0)
                .FlatMapAsync(x => {
                    flatSelectorExecuted = true;
                    return AssertionUtilities.DivisionAsync(x, 2);
                }, (y, x) => {
                    resultSelectorExectued = true;
                    return y + x;
                }).AssertError("Can not divide '2' with '0'.");

            Assert.False(flatSelectorExecuted,
                "The flatmapSelector should not get exectued if the source Result<T, TError> contains error.");
            Assert.False(resultSelectorExectued,
                "The resultSelector should not get exectued if the source Result<T, TError> contains error.");
        }

        [Fact]
        public async Task Result_With_Value_Flatmaps_Result_with_Error__Expects_Result_With_Value() {
            var flatSelectorExecuted = false;
            await AssertionUtilities
                .Division(2, 2)
                .FlatMapAsync(x => {
                    flatSelectorExecuted = true;
                    return AssertionUtilities.DivisionAsync(x, 0);
                })
                .AssertError("Can not divide '1' with '0'.");

            Assert.True(flatSelectorExecuted,
                "The flatmapSelector should get exectued.");
        }

        [Fact]
        public async Task Result_With_Value_Flatmaps_Result_with_Value__Expects_Result_With_Value() {
            var flatSelectorExecuted = false;
            await AssertionUtilities
                .Division(2, 2)
                .FlatMapAsync(x => {
                    flatSelectorExecuted = true;
                    return AssertionUtilities.DivisionAsync(x, 2);
                })
                .AssertValue(0.5d);
            Assert.True(flatSelectorExecuted, "flatmapselector should get executed.");
        }

        [Fact]
        public async Task Result_With_Value_FlatmapsRS_Result_with_Error__Expects_Result_With_Error() {
            var flatSelectorExecuted = false;
            var resultSelectorExectued = false;
            await AssertionUtilities.Division(2, 2).FlatMapAsync(x => {
                flatSelectorExecuted = true;
                return AssertionUtilities.DivisionAsync(x, 0);
            }, (y, x) => {
                resultSelectorExectued = true;
                return y + x;
            }).AssertError("Can not divide '1' with '0'.");

            Assert.True(flatSelectorExecuted,
                "The flatmapSelector should get exectued.");
            Assert.False(resultSelectorExectued,
                "The resultSelector should not get executed since flatselector result failed.");
        }

        [Fact]
        public async Task Result_With_Value_FlatmapsRS_Result_with_Value__Expects_Result_With_Value() {
            var flatSelectorExecuted = false;
            var resultSelectorExectued = false;
            await AssertionUtilities
                .Division(2, 2)
                .FlatMapAsync(x => {
                    flatSelectorExecuted = true;
                    return AssertionUtilities.DivisionAsync(x, 2);
                }, (y, x) => {
                    resultSelectorExectued = true;
                    return y + x;
                })
                .AssertValue(1.5d);
            Assert.True(flatSelectorExecuted, "Flatmapselecotr should get executed.");
            Assert.True(resultSelectorExectued,
                "ResultSelector should get executed since both source and the result from flatmapselector contains values.");
        }
    }
}