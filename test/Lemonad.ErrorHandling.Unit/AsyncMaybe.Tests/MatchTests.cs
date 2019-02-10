using System;
using System.Threading.Tasks;
using Lemonad.ErrorHandling.Extensions;
using Xunit;

namespace Lemonad.ErrorHandling.Unit.AsyncMaybe.Tests {
    public class MatchTests {
        [Fact]
        public async Task Action_Overload__Match_AsyncMaybe_With_Value__Expects_Value_Of_SomeSelector() {
            await ErrorHandling.AsyncMaybe.Value("hello")
                .Match(s => { Assert.Equal("hello", s); }, () => { Assert.True(false); });
        }

        [Fact]
        public async Task Action_Overload__Match_AsyncMaybe_Without_Value__Expects_Value_Of_NoneSelector() {
            await ErrorHandling.AsyncMaybe.None<string>()
                .Match(s => { Assert.True(false); }, () => { Assert.True(true); });
        }

        [Fact]
        public void Action_Overload__Null_NoneSelector__Throws() {
            Action noneSelector = null;
            Assert.Throws<ArgumentNullException>(() =>
                ErrorHandling.AsyncMaybe.Value("hello").ToMaybe(s => false)
                    .Match(s => { Assert.True(false); }, noneSelector));
        }

        [Fact]
        public async Task Action_Overload__Null_NoneSelector_And_SomeSelector__Throws() {
            Action noneSelector = null;
            Action<string> someSelector = null;
            await Assert.ThrowsAsync<ArgumentNullException>(() =>
                ErrorHandling.AsyncMaybe.None<string>().Match(someSelector, noneSelector));
        }

        [Fact]
        public async Task Match_AsyncMaybe_With_Value__Expects_Value_Of_SomeSelector() {
            var match = await ErrorHandling.AsyncMaybe.Value("hello").Match(s => s.Length, () => 0);
            Assert.Equal(5, match);
        }

        [Fact]
        public async Task Match_AsyncMaybe_Without_Value__Expects_Value_Of_NoneSelector() {
            var match = await ErrorHandling.AsyncMaybe.None<string>().Match(s => s.Length, () => 0);
            Assert.Equal(0, match);
        }

        [Fact]
        public async Task Null_NoneSelector__Throws() {
            Func<int> noneSelector = null;
            await Assert.ThrowsAsync<ArgumentNullException>(() =>
                ErrorHandling.AsyncMaybe.None<string>().Match(s => s.Length, noneSelector));
        }

        [Fact]
        public async Task Null_NoneSelector_And_SomeSelector__Throws() {
            Func<int> noneSelector = null;
            Func<string, int> someSelector = null;
            await Assert.ThrowsAsync<ArgumentNullException>(() =>
                ErrorHandling.AsyncMaybe.None<string>().Match(someSelector, noneSelector));
        }
    }
}