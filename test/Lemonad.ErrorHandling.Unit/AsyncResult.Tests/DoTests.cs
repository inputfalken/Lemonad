﻿using System;
using System.Threading.Tasks;
using Assertion;
using Lemonad.ErrorHandling.Extensions.AsyncResult;
using Xunit;

namespace Lemonad.ErrorHandling.Unit.AsyncResult.Tests {
    public class DoTests {
        [Fact]
        public void Passing_Null_Task_Throws()
            => Assert.Throws<ArgumentNullException>(AssertionUtilities.ActionParamName,
                () => AssertionUtilities.DivisionAsync(10, 2).Do(null));

        [Fact]
        public async Task Result_With_Error__Expects_Action_To_Be_Invoked() {
            var actionExectued = false;
            await AssertionUtilities
                .DivisionAsync(10, 0)
                .Do(() => actionExectued = true)
                .AssertError("Can not divide '10' with '0'.");

            Assert.True(actionExectued, "Should not get exectued since there's an error.");
        }

        [Fact]
        public async Task Result_With_Value__Expects_Action_To_be_Invoked() {
            var actionExectued = false;
            await AssertionUtilities
                .DivisionAsync(10, 2)
                .Do(() => actionExectued = true)
                .AssertValue(5);

            Assert.True(actionExectued, "Should not get exectued since there's an error.");
        }
    }
}