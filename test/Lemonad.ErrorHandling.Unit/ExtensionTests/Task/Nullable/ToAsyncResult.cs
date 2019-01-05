using System;
using System.Threading.Tasks;
using Assertion;
using Lemonad.ErrorHandling.Extensions.AsyncResult;
using Lemonad.ErrorHandling.Extensions.Task;
using Xunit;

namespace Lemonad.ErrorHandling.Unit.ExtensionTests.Task.Nullable {
    public class ToAsyncResult {
        [Fact]
        public void Passing_Null_Selector_Throws() {
            Assert.Throws<ArgumentNullException>(
                AssertionUtilities.ErrorSelectorName,
                () => System.Threading.Tasks.Task.FromResult((int?) 2).ToAsyncResult<int, int>(null)
            );
        }

        [Fact]
        public void Passing_Null_Source_Throws() {
            Assert.Throws<ArgumentNullException>(
                AssertionUtilities.ExtensionParameterName,
                () => ((Task<int?>) null).ToAsyncResult(() => "ERROR")
            );
        }

        [Fact]
        public async System.Threading.Tasks.Task Task_With_Nullable_With_Null_Expects_Value() {
            await System.Threading.Tasks.Task
                .FromResult((int?) null)
                .ToAsyncResult(() => "ERROR")
                .AssertError("ERROR");
        }

        [Fact]
        public async System.Threading.Tasks.Task Task_With_Nullable_With_Value_Expects_Value() {
            await System.Threading.Tasks.Task
                .FromResult((int?) 2)
                .ToAsyncResult(() => "ERROR")
                .AssertValue(2);
        }
    }
}