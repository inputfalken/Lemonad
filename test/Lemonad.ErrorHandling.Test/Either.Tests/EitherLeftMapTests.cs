using System;
using Xunit;

namespace Lemonad.ErrorHandling.Test.Either.Tests {
    public class EitherLeftMapTests {
        [Fact]
        public void Eiteher_With_Right_Value__MapLeft__Expects_No_Exception() {
            string x = null;
            var mapLeft = x.ToEitherRight<string, string>()
                .RightWhen(s => false, "");
            Console.WriteLine();
        }
    }
}