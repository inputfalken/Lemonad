using System;
using Xunit;

namespace Lemonad.ErrorHandling.Test.Either.Tests {
    public class EitherMapTests {
        [Fact]
        public void Mapping_Left_Either__Expects_Not_To_Execute_RightSelector() {
            var exception = Record.Exception(() => {
                var either = "error".ToEitherLeft<string, int>()
                    .Map(s => s.ToUpper(), _ => {
                        throw new Exception();
                        return 0;
                    });
                Assert.True(either.IsLeft, "Should have a left value.");
                Assert.Equal("ERROR", either.Left);
            });
            Assert.Null(exception);
        }

        [Fact]
        public void Mapping_Right_Either__Expect_Not_To_Execute_LeftSelector() {
            var exception = Record.Exception(() => {
                var either = 20.ToEitherRight<string, int>()
                    .Map(s => {
                        throw new Exception();
                        return s;
                    }, i => i * 2);
                Assert.True(either.IsRight, "Should have a right value.");
                Assert.Equal(40, either.Right);
            });
            Assert.Null(exception);
        }
    }
}