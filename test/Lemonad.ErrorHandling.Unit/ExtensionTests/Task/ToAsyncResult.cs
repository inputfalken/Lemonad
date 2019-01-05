using System;
using System.Threading.Tasks;
using Assertion;
using Lemonad.ErrorHandling.Extensions.AsyncResult;
using Lemonad.ErrorHandling.Extensions.Task;
using Xunit;

namespace Lemonad.ErrorHandling.Unit.ExtensionTests.Task {
    public class ToAsyncResult {
        [Fact]
        public void Passing_Null_Predicate_Throws() {
            Assert.Throws<ArgumentNullException>(
                AssertionUtilities.PredicateName,
                () => System.Threading.Tasks.Task.FromResult("foo").ToAsyncResult(null, _ => "ERROR")
            );
        }

        [Fact]
        public void Passing_Null_Selector_Throws() {
            Assert.Throws<ArgumentNullException>(
                AssertionUtilities.ErrorSelectorName,
                () => System.Threading.Tasks.Task.FromResult("foo").ToAsyncResult<string, int>(s => true, null)
            );
        }

        [Fact]
        public void Passing_Null_Source_Throws() {
            Assert.Throws<ArgumentNullException>(
                AssertionUtilities.ExtensionParameterName,
                () => ((Task<string>) null).ToAsyncResult(s => false, _ => "ERROR")
            );
        }

        [Fact]
        public async System.Threading.Tasks.Task Task_With_Falsy_Predicate__Expects_Value() {
            await System.Threading.Tasks.Task
                .FromResult("foobar")
                .ToAsyncResult(x => x != "foobar", _ => "ERROR")
                .AssertError("ERROR");
        }

        [Fact]
        public async System.Threading.Tasks.Task Task_With_Truthy_Predicate__Expects_Value() {
            await System.Threading.Tasks.Task
                .FromResult("foobar")
                .ToAsyncResult(x => x == "foobar", _ => "ERROR")
                .AssertValue("foobar");
        }
    }
}