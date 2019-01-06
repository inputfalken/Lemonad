using System;
using System.Linq;
using Assertion;
using Lemonad.ErrorHandling.Extensions.AsyncResult;
using Xunit;

namespace Lemonad.ErrorHandling.Unit.ExtensionTests.IAsyncResult {
    public class Multiple {
        [Fact]
        public async System.Threading.Tasks.Task Four_Failed_Predicates() {
            await AssertionUtilities.DivisionAsync(10, 2).Multiple(
                x => x.Filter(y => y == 4, _ => "Value is not equal to 4."),
                x => x.Filter(y => y == 0, _ => "Value is not equal to 0."),
                x => x.Filter(y => y == 20, _ => "Value is not equal to 20."),
                x => x.Filter(y => y == 21, _ => "Value is not equal to 21.")
            ).AssertError(new[] {
                "Value is not equal to 4.",
                "Value is not equal to 0.",
                "Value is not equal to 20.",
                "Value is not equal to 21."
            });
        }

        [Fact]
        public async System.Threading.Tasks.Task Four_Successful_Predicates() {
            await AssertionUtilities.DivisionAsync(10, 2).Multiple(
                x => x.Filter(y => y == 5, _ => "Value is not equal to 4."),
                x => x.Filter(y => true, _ => "This should never happen!"),
                x => x.Filter(y => true, _ => "This should never happen!"),
                x => x.Filter(y => true, _ => "This should never happen!")
            ).AssertValue(5);
        }

        [Fact(Timeout = 2000)]
        public async System.Threading.Tasks.Task Hundred_Thousand_Does_Not_Take_Long_Time() {
            await AssertionUtilities.DivisionAsync(10, 2).Multiple(
                Enumerable.Range(0, 100000).Select(x =>
                    new Func<IAsyncResult<double, string>, IAsyncResult<double, string>>(
                        y => y.Filter(d => true, _ => "This should never happen!")
                    )
                ).ToArray()
            ).AssertValue(5);
        }

        [Fact]
        public async System.Threading.Tasks.Task One_Successful_Predicate_And_One_Failed_Predicate() {
            await AssertionUtilities.DivisionAsync(10, 2).Multiple(
                x => x.Filter(y => y == 5, _ => "Value is not equal to 5."),
                x => x.Filter(y => y == 0, _ => "Value is not equal to 0.")
            ).AssertError(new[] {"Value is not equal to 0."});
        }

        [Fact]
        public void Passing_Null_Result_Throws()
            => Assert.Throws<ArgumentNullException>(
                AssertionUtilities.ExtensionParameterName,
                () => ((IAsyncResult<string, int>) null).Multiple()
            );

        [Fact]
        public async System.Threading.Tasks.Task Three_Failed_Predicates() {
            await AssertionUtilities.DivisionAsync(10, 2).Multiple(
                x => x.Filter(y => y == 4, _ => "Value is not equal to 4."),
                x => x.Filter(y => y == 0, _ => "Value is not equal to 0."),
                x => x.Filter(y => y == 20, _ => "Value is not equal to 20.")
            ).AssertError(new[] {
                "Value is not equal to 4.",
                "Value is not equal to 0.",
                "Value is not equal to 20."
            });
        }

        [Fact]
        public async System.Threading.Tasks.Task Three_Successful_Predicates() {
            await AssertionUtilities.DivisionAsync(10, 2).Multiple(
                x => x.Filter(y => y == 5, _ => "Value is not equal to 4."),
                x => x.Filter(y => true, _ => "This should never happen!"),
                x => x.Filter(y => true, _ => "This should never happen!")
            ).AssertValue(5);
        }

        [Fact]
        public async System.Threading.Tasks.Task Two_Failed_Predicates() {
            await AssertionUtilities.DivisionAsync(10, 2).Multiple(
                x => x.Filter(y => y == 4, _ => "Value is not equal to 4."),
                x => x.Filter(y => y == 0, _ => "Value is not equal to 0.")
            ).AssertError(new[] {"Value is not equal to 4.", "Value is not equal to 0."});
        }

        [Fact]
        public async System.Threading.Tasks.Task Two_Successful_Predicates() {
            await AssertionUtilities.DivisionAsync(10, 2).Multiple(
                x => x.Filter(y => y == 5, _ => "Value is not equal to 4."),
                x => x.Filter(y => true, _ => "This should never happen!")
            ).AssertValue(5);
        }
    }
}