using System.Threading.Tasks;
using Xunit;

namespace Lemonad.ErrorHandling.Test.AsyncResult.Tests {
    public class FlattenTests {
        [Fact]
        public async Task Result_With_Error_Flatmaps_Result_with_Error__Expects_Result_With_Error() {
            var flatSelectorExecuted = false;
            var errorSelectorExecuted = false;
            var result =  await AssertionUtilities.DivisionAsync(2, 0).Flatten(x => {
                flatSelectorExecuted = true;
                return AssertionUtilities.DivisionAsync(x, 0);
            }, s => {
                errorSelectorExecuted = true;
                return s;
            });

            Assert.False(errorSelectorExecuted,
                "Errorselector should not get exeuted since there is an error in the source.");
            Assert.False(flatSelectorExecuted,
                "The flatmap selector should not get exectued if the sourcegei Result<T, TError> contains error.");
            Assert.False(result.HasValue, "Result should not have a value.");
            Assert.True(result.HasError, "Result should have a error.");
            Assert.Equal(default, result.Value);
            Assert.Equal("Can not divide '2' with '0'.", result.Error);
        }

        [Fact]
        public async Task Result_With_Error_Flatmaps_Result_with_Error_Without_ErrorSelector__Expects_Result_With_Error() {
            var flatSelectorExecuted = false;
            var result =  await AssertionUtilities.DivisionAsync(2, 0).Flatten(x => {
                flatSelectorExecuted = true;
                return AssertionUtilities.DivisionAsync(x, 0);
            });

            Assert.False(flatSelectorExecuted,
                "The flatmap selector should not get exectued if the source Result<T, TError> contains error.");
            Assert.False(result.HasValue, "Result should not have a value.");
            Assert.True(result.HasError, "Result should have a error.");
            Assert.Equal(default, result.Value);
            Assert.Equal("Can not divide '2' with '0'.", result.Error);
        }

        [Fact]
        public async Task Result_With_Error_Flatmaps_Result_with_Value__Expects_Result_With_Error() {
            var flatSelectorExecuted = false;
            var errorSelectorExecuted = false;
            var result =  await AssertionUtilities.DivisionAsync(2, 0).Flatten(x => {
                flatSelectorExecuted = true;
                return AssertionUtilities.DivisionAsync(x, 2);
            }, s => {
                errorSelectorExecuted = true;
                return s;
            });

            Assert.False(errorSelectorExecuted,
                "Errorselector should not get exeuted since there is an error in the source.");
            Assert.False(flatSelectorExecuted,
                "The flatmap selector should not get exectued if the source Result<T, TError> contains error.");
            Assert.False(result.HasValue, "Result should not have a value.");
            Assert.True(result.HasError, "Result should have a error.");
            Assert.Equal(default, result.Value);
            Assert.Equal("Can not divide '2' with '0'.", result.Error);
        }

        [Fact]
        public async Task Result_With_Error_Flatmaps_Result_with_Value_Without_ErrorSelector__Expects_Result_With_Error() {
            var flatSelectorExecuted = false;
            var result =  await AssertionUtilities.DivisionAsync(2, 0).Flatten(x => {
                flatSelectorExecuted = true;
                return AssertionUtilities.DivisionAsync(x, 2);
            });

            Assert.False(flatSelectorExecuted,
                "The flatmap selector should not get exectued if the source Result<T, TError> contains error.");
            Assert.False(result.HasValue, "Result should not have a value.");
            Assert.True(result.HasError, "Result should have a error.");
            Assert.Equal(default, result.Value);
            Assert.Equal("Can not divide '2' with '0'.", result.Error);
        }

        [Fact]
        public async Task Result_With_Value_Flatmaps_Result_with_Error__Expects_Result_With_Error() {
            var flatSelectorExecuted = false;
            var errorSelectorExecuted = false;
            var result =  await AssertionUtilities.DivisionAsync(2, 2).FlatMap(x => {
                flatSelectorExecuted = true;
                return AssertionUtilities.DivisionAsync(x, 0);
            }, s => {
                errorSelectorExecuted = true;
                return s;
            });

            Assert.True(errorSelectorExecuted,
                "Errorselector should not exeuted since the errror came from the result given to the flatselector.");
            Assert.True(flatSelectorExecuted,
                "The flatmapSelector should get exectued.");
            Assert.False(result.HasValue, "Result should not have a value.");
            Assert.True(result.HasError, "Result should have a error.");
            Assert.Equal(default, result.Value);
            Assert.Equal("Can not divide '1' with '0'.", result.Error);
        }

        [Fact]
        public async Task Result_With_Value_Flatmaps_Result_with_Error_Without_ErrorSelector__Expects_Result_With_Error() {
            var flatSelectorExecuted = false;
            var result =  await AssertionUtilities.DivisionAsync(2, 2).FlatMap(x => {
                flatSelectorExecuted = true;
                return AssertionUtilities.DivisionAsync(x, 0);
            });

            Assert.True(flatSelectorExecuted,
                "The flatmapSelector should get exectued.");
            Assert.False(result.HasValue, "Result should not have a value.");
            Assert.True(result.HasError, "Result should have a error.");
            Assert.Equal(default, result.Value);
            Assert.Equal("Can not divide '1' with '0'.", result.Error);
        }

        [Fact]
        public async Task Result_With_Value_Flatmaps_Result_with_Value__Expects_Result_With_Value() {
            var flatSelectorExecuted = false;
            var errorSelectorExecuted = false;
            var result =  await AssertionUtilities.DivisionAsync(2, 2).Flatten(x => {
                flatSelectorExecuted = true;
                return AssertionUtilities.DivisionAsync(x, 2);
            }, s => {
                errorSelectorExecuted = true;
                return s;
            });
            Assert.True(flatSelectorExecuted, "flatmapselector should get executed.");
            Assert.False(errorSelectorExecuted, "Errorselector should not get exeuted.");
            Assert.True(result.HasValue, "Result should have a value.");
            Assert.False(result.HasError, "Result should not have a error.");
            Assert.Equal(1, result.Value);
            Assert.Equal(default, result.Error);
        }

        [Fact]
        public async Task Result_With_Value_Flatmaps_Result_with_Value_Without_ErrorSelector__Expects_Result_With_Value() {
            var flatSelectorExecuted = false;
            var result =  await AssertionUtilities.DivisionAsync(2, 2).Flatten(x => {
                flatSelectorExecuted = true;
                return AssertionUtilities.DivisionAsync(x, 2);
            });
            Assert.True(flatSelectorExecuted, "flatmapselector should get executed.");
            Assert.True(result.HasValue, "Result should have a value.");
            Assert.False(result.HasError, "Result should not have a error.");
            Assert.Equal(1, result.Value);
            Assert.Equal(default, result.Error);
        }

        [Fact]
        public async Task Result_With_Value_FlatmapsRS_Result_with_Error__Expects_Result_With_Error() {
            var flatSelectorExecuted = false;
            var errorSelectorExecuted = false;
            var result =  await AssertionUtilities.DivisionAsync(2, 2).Flatten(x => {
                flatSelectorExecuted = true;
                return AssertionUtilities.DivisionAsync(x, 0);
            }, s => {
                errorSelectorExecuted = true;
                return s;
            });

            Assert.True(errorSelectorExecuted,
                "Errorselector should get exeuted since the errror came from the result given to the flatselector.");
            Assert.True(flatSelectorExecuted,
                "The flatselector should not get executed since flatselector result failed.");
            Assert.False(result.HasValue, "Result should not have a value.");
            Assert.True(result.HasError, "Result should have a error.");
            Assert.Equal(default, result.Value);
            Assert.Equal("Can not divide '1' with '0'.", result.Error);
            Assert.False(result.HasValue, "Result should not have a value.");
            Assert.True(result.HasError, "Result should have a error.");
            Assert.Equal(default, result.Value);
            Assert.Equal("Can not divide '1' with '0'.", result.Error);
        }

        [Fact]
        public async Task Result_With_Value_FlatmapsRS_Result_with_Error_Without_ErrorSelector__Expects_Result_With_Error() {
            var flatSelectorExecuted = false;
            var result =  await AssertionUtilities.DivisionAsync(2, 2).Flatten(x => {
                flatSelectorExecuted = true;
                return AssertionUtilities.DivisionAsync(x, 0);
            });

            Assert.True(flatSelectorExecuted,
                "The flatselector should not get executed since flatselector result failed.");
            Assert.False(result.HasValue, "Result should not have a value.");
            Assert.True(result.HasError, "Result should have a error.");
            Assert.Equal(default, result.Value);
            Assert.Equal("Can not divide '1' with '0'.", result.Error);
            Assert.False(result.HasValue, "Result should not have a value.");
            Assert.True(result.HasError, "Result should have a error.");
            Assert.Equal(default, result.Value);
            Assert.Equal("Can not divide '1' with '0'.", result.Error);
        }
    }
}
