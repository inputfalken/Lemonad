using System;
using System.Threading.Tasks;
using Assertion;
using Lemonad.ErrorHandling.Extensions.AsyncResult;
using Lemonad.ErrorHandling.Extensions.Task;
using Xunit;

namespace Lemonad.ErrorHandling.Unit.ExtensionTests.Task {
    public class ToAsyncResultError {
        [Fact]
        public void Passing_Null_Source_Throws() {
            Assert.Throws<ArgumentNullException>(
                AssertionUtilities.ExtensionParameterName,
                () => ((Task<string>) null).ToAsyncResultError(s => false, _ => "ERROR")
            );
        }

        [Fact]
        public void Passing_Null_Predicate_Throws() {
            Assert.Throws<ArgumentNullException>(
                AssertionUtilities.PredicateName,
                () => System.Threading.Tasks.Task.FromResult("foo").ToAsyncResultError(null, _ => "ERROR")
            );
        }

        [Fact]
        public void Passing_Null_Selector_Throws() {
            Assert.Throws<ArgumentNullException>(
                AssertionUtilities.ValueSelectorName,
                () => System.Threading.Tasks.Task.FromResult("foo").ToAsyncResultError<int, string>(s => true, null)
            );
        }

        [Fact]
        public async System.Threading.Tasks.Task Task_With_Truthy_Predicate__Expects_Value() {
            await System.Threading.Tasks.Task
                .FromResult("foobar")
                .ToAsyncResultError(x => x == "foobar", _ => "ERROR")
                .AssertError("foobar");
        }

        [Fact]
        public async System.Threading.Tasks.Task Task_With_Falsy_Predicate__Expects_Value() {
            await System.Threading.Tasks.Task
                .FromResult("foobar")
                .ToAsyncResultError(x => x != "foobar", _ => "VALUE")
                .AssertValue("VALUE");
        }
    }
}