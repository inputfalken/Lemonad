using System;
using System.Threading.Tasks;
using Assertion;
using Xunit;

namespace Lemonad.ErrorHandling.Unit.AsyncResult.Tests {
    public class MatchTests {
        [Fact]
        public async Task Passing_Null_ErrorSelector_Throws()
            => await Assert.ThrowsAsync<ArgumentNullException>(
                AssertionUtilities.ErrorSelectorName,
                () => AssertionUtilities.DivisionAsync(10, 0).Match(d => "", null)
            );

        [Fact]
        public async Task Passing_Null_Selector_Throws()
            => await Assert.ThrowsAsync<ArgumentNullException>(
                AssertionUtilities.SelectorName,
                () => AssertionUtilities.DivisionAsync(10, 0).Match(null, s => s)
            );

        [Fact]
        public async Task Result_With_Error__Expect_ErrorAction() {
            var selectorExectued = false;
            var errorSelectorExectued = false;
            await AssertionUtilities.DivisionAsync(10, 0)
                .Match(d => { selectorExectued = true; }, s => { errorSelectorExectued = true; });

            Assert.False(selectorExectued, "Selector should not get executed.");
            Assert.True(errorSelectorExectued, "Error selector should get exectued.");
        }

        [Fact]
        public async Task Result_With_Error__Expect_ErrorSelector() {
            var selectorExectued = false;
            var errorSelectorExectued = false;
            var result = await AssertionUtilities.DivisionAsync(10, 0).Match(d => {
                selectorExectued = true;
                return d;
            }, s => {
                errorSelectorExectued = true;
                return -1;
            });

            Assert.False(selectorExectued, "Selector should not get executed.");
            Assert.True(errorSelectorExectued, "Error selector should get exectued.");
            Assert.Equal(-1, result);
        }

        [Fact]
        public async Task
            Result_With_Value__Expect_Action() {
            var selectorExectued = false;
            var errorSelectorExectued = false;
            await AssertionUtilities.DivisionAsync(10, 2)
                .Match(d => { selectorExectued = true; }, s => { errorSelectorExectued = true; });

            Assert.True(selectorExectued, "Selector should get executed.");
            Assert.False(errorSelectorExectued, "Error selector not should get exectued.");
        }

        [Fact]
        public async Task
            Result_With_Value__Expect_Selector() {
            var selectorExectued = false;
            var errorSelectorExectued = false;
            var result = await AssertionUtilities.DivisionAsync(10, 2).Match(d => {
                selectorExectued = true;
                return d;
            }, s => {
                errorSelectorExectued = true;
                return -1;
            });

            Assert.True(selectorExectued, "Selector should get executed.");
            Assert.False(errorSelectorExectued, "Error selector not should get exectued.");
            Assert.Equal(5, result);
        }

        [Fact]
        public async Task Void_Passing_Null_ErrorSelector_Throws()
            => await Assert.ThrowsAsync<ArgumentNullException>(
                AssertionUtilities.ErrorActionParamName,
                () => AssertionUtilities.DivisionAsync(10, 0).Match(d => { }, null)
            );

        [Fact]
        public async Task Void_Passing_Null_Selector_Throws()
            => await Assert.ThrowsAsync<ArgumentNullException>(
                AssertionUtilities.ActionParamName,
                () => AssertionUtilities.DivisionAsync(10, 0).Match(null, s => { })
            );
    }
}