using System;
using Xunit;

namespace Lemonad.ErrorHandling.Test.Either.Tests {
    public class EitherLeftMapTests {
        [Fact]
        public void Eiteher_With_Right_Value__MapLeft__Expects_No_Exception() {
            var mapLeft = "success".ToEitherLeft<string, string>()
                .Map(s => s.Length, s => s.Length)
                .RightWhen(s => s >= int.MaxValue, "foo");
            Console.WriteLine();
        }
    }
}