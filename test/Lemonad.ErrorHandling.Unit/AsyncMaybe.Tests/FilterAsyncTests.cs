using System;
using System.Threading.Tasks;
using Assertion;
using Lemonad.ErrorHandling.Extensions.AsyncMaybe;
using Xunit;

namespace Lemonad.ErrorHandling.Unit.AsyncMaybe.Tests {
    public class FilterAsyncTests {
        [Fact]
        public void Maybe_With_Error_Null_Predicate__Throws_ArgumentNullException() {
            Assert.Throws<ArgumentNullException>(() => {
                Func<string, Task<bool>> predicate = null;
                ErrorHandling.AsyncMaybe.None<string>().FilterAsync(predicate);
            });
        }

        [Fact]
        public void Maybe_With_Value_Null_Predicate__Throws_ArgumentNullException() {
            Assert.Throws<ArgumentNullException>(() => {
                Func<string, Task<bool>> predicate = null;
                ErrorHandling.AsyncMaybe.Value("foo").FilterAsync(predicate);
            });
        }

        [Fact]
        public async Task Maybe_With_Value_With_False_Predicate__Expects_Maybe_No_With_Value() {
            var predicateExecuted = false;
            await ErrorHandling.AsyncMaybe.Value("foobar").FilterAsync(async s => {
                await AssertionUtilities.Delay;
                predicateExecuted = true;
                return s is null;
            }).AssertNone();
            Assert.True(predicateExecuted);
        }

        [Fact]
        public async Task Maybe_With_Value_With_True_Predicate__Expects_Maybe_With_Value() {
            var predicateExecuted = false;
            await ErrorHandling.AsyncMaybe
                .Value("foobar")
                .FilterAsync(async s => {
                    await AssertionUtilities.Delay;
                    predicateExecuted = true;
                    return s is null == false;
                })
                .AssertValue("foobar");
            Assert.True(predicateExecuted);
        }

        [Fact]
        public async Task TaskMaybe_With_No_Value_With_True_Predicate__Expects_Maybe_No_With_Value() {
            var predicateExecuted = false;
            await ErrorHandling.AsyncMaybe.None<string>()
                .FilterAsync(async s => {
                    await AssertionUtilities.Delay;
                    predicateExecuted = true;
                    return s is null == false;
                })
                .AssertNone();
            Assert.False(predicateExecuted);
        }
    }
}