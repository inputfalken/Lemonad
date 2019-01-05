using System;
using System.Threading.Tasks;
using Assertion;
using Lemonad.ErrorHandling.Extensions.AsyncResult;
using Xunit;

namespace Lemonad.ErrorHandling.Unit.Result.Tests {
    public class FullMapAsyncTests {
        [Fact]
        public void Async_Selector_And_ErrorSelector_Passing_Null_Selector_Throws()
            => Assert.Throws<ArgumentNullException>(
                AssertionUtilities.SelectorName,
                () => AssertionUtilities.Division(10, 2).FullMapAsync((Func<double, Task<string>>) null,
                    async s => {
                        await AssertionUtilities.Delay;
                        return string.Empty;
                    })
            );

        [Fact]
        public void Async_Selector_And_ErrorSelector_Passing_Null_ErrorSelector_Throws()
            => Assert.Throws<ArgumentNullException>(
                AssertionUtilities.ErrorSelectorName,
                () => AssertionUtilities.Division(10, 2)
                    .FullMapAsync(async d => {
                        await AssertionUtilities.Delay;

                        return string.Empty;
                    }, (Func<string, Task<string>>) null)
            );

        [Fact]
        public void Async_Selector_Passing_Null_Selector_Throws()
            => Assert.Throws<ArgumentNullException>(
                AssertionUtilities.SelectorName,
                () => AssertionUtilities.Division(10, 2).FullMapAsync((Func<double, Task<string>>) null, s => s)
            );

        [Fact]
        public void Async_Selector_Passing_Null_ErrorSelector_Throws()
            => Assert.Throws<ArgumentNullException>(
                AssertionUtilities.ErrorSelectorName,
                () => AssertionUtilities.Division(10, 2)
                    .FullMapAsync( async d => {
                        await AssertionUtilities.Delay;
                        return string.Empty;
                    }, (Func<string, string>) null)
            );

        [Fact]
        public void Async_ErrorSelector_Passing_Null_Selector_Throws()
            => Assert.Throws<ArgumentNullException>(
                AssertionUtilities.SelectorName,
                () => AssertionUtilities.Division(10, 2)
                    .FullMapAsync(
                        (Func<double, string>) null,
                        async s => {
                            await AssertionUtilities.Delay;
                            return string.Empty;
                        })
            );

        [Fact]
        public void Async_ErrorSelector_Passing_Null_ErrorSelector_Throws()
            => Assert.Throws<ArgumentNullException>(
                AssertionUtilities.ErrorSelectorName,
                () => AssertionUtilities.Division(10, 2)
                    .FullMapAsync(d => d, (Func<string, Task<string>>) null)
            );

        [Fact]
        public async Task
            Async_Selector_Result_With_Error_Expects__Selector_Never__To_Be_Executed_And_ErrorSelector_To_Be_Invoked() {
            var selectorExectued = false;
            var errorSelectorExectued = false;
            await AssertionUtilities
                .Division(10, 0)
                .FullMapAsync(async d => {
                    selectorExectued = true;
                    await Task.Delay(50);
                    return d * 2;
                }, s => {
                    errorSelectorExectued = true;
                    return s.ToUpper();
                }).AssertError("CAN NOT DIVIDE '10' WITH '0'.");

            Assert.False(selectorExectued, "Should not get exectued since there's an error from the result.");
            Assert.True(errorSelectorExectued, "Should get exectued since there's an error from the result.");
        }

        [Fact]
        public async Task
            Async_Selector_Result_With_Value_Expects__Selector_To_Be_Executed_And_ErrorSelector_To_Never_Be_Invoked() {
            var selectorExectued = false;
            var errorSelectorExectued = false;
            await AssertionUtilities
                .Division(10, 2)
                .FullMapAsync(async d => {
                    selectorExectued = true;
                    await Task.Delay(50);
                    return d * 10;
                }, s => {
                    errorSelectorExectued = true;
                    return s.ToUpper();
                }).AssertValue(50);

            Assert.True(selectorExectued, "Should get exectued since there's an value from the result.");
            Assert.False(errorSelectorExectued, "Should not get exectued since there's an value from the result.");
        }

        [Fact]
        public async Task
            Async_Selector_And_ErrorSelector_Result_With_Value_Expects__Selector_To_Be_Executed_And_ErrorSelector_To_Never_Be_Invoked() {
            var selectorExectued = false;
            var errorSelectorExectued = false;
            await AssertionUtilities
                .Division(10, 2)
                .FullMapAsync(async d => {
                    selectorExectued = true;
                    await Task.Delay(50);
                    return d * 10;
                }, async s => {
                    await AssertionUtilities.Delay;
                    errorSelectorExectued = true;
                    return s.ToUpper();
                }).AssertValue(50);

            Assert.True(selectorExectued, "Should get exectued since there's an value from the result.");
            Assert.False(errorSelectorExectued, "Should not get exectued since there's an value from the result.");
        }

        [Fact]
        public async Task
            Async_ErrorSelector_Result_With_Error_Expects__Selector_Never__To_Be_Executed_And_ErrorSelector_To_Be_Invoked() {
            var selectorExectued = false;
            var errorSelectorExectued = false;
            await AssertionUtilities
                .Division(10, 0)
                .FullMapAsync(async d => {
                    selectorExectued = true;
                    await Task.Delay(50);
                    return d * 2;
                }, async s => {
                    await AssertionUtilities.Delay;
                    errorSelectorExectued = true;
                    return s.ToUpper();
                }).AssertError("CAN NOT DIVIDE '10' WITH '0'.");

            Assert.False(selectorExectued, "Should not get exectued since there's an error from the result.");
            Assert.True(errorSelectorExectued, "Should get exectued since there's an error from the result.");
        }

        [Fact]
        public async Task
            Async_ErrorSelector_Result_With_Value_Expects__Selector_To_Be_Executed_And_ErrorSelector_To_Never_Be_Invoked() {
            var selectorExectued = false;
            var errorSelectorExectued = false;
            await AssertionUtilities
                .Division(10, 2)
                .FullMapAsync(d => {
                    selectorExectued = true;
                    return d * 10;
                }, async s => {
                    await AssertionUtilities.Delay;
                    errorSelectorExectued = true;
                    return s;
                }).AssertValue(50);

            Assert.True(selectorExectued, "Should get exectued since there's an value from the result.");
            Assert.False(errorSelectorExectued, "Should not get exectued since there's an value from the result.");
        }
    }
}