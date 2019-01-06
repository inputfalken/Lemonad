using System;
using System.Threading.Tasks;
using Assertion;
using Lemonad.ErrorHandling.Internal.TaskExtensions;
using Xunit;

namespace Lemonad.ErrorHandling.Unit.Internal.Tests.TaskExtensions.Tests {
    public class DoTests {
        private const string Text = "Hello";

        private static Task<string> StringTask => Task.Run(async () => {
            await Task.Delay(100);
            return Text;
        });

        private static Task VoidTask => Task.Run(async () => { await Task.Delay(100); });

        [Fact]
        public Task Null_Action_Throws() =>
            Assert.ThrowsAsync<ArgumentNullException>(AssertionUtilities.ActionParamName, () => StringTask.Do(null)
            );

        [Fact]
        public Task Null_String_Task_Throws() {
            var stringTask = StringTask;
            stringTask = null;
            return Assert.ThrowsAsync<ArgumentNullException>(() => stringTask.Do(s => { }));
        }

        [Fact]
        public Task Null_Void_Task_Throws() {
            var stringTask = VoidTask;
            stringTask = null;
            return Assert.ThrowsAsync<ArgumentNullException>(() => stringTask.Do(() => { }));
        }

        [Fact]
        public async Task String_Task_Assign_Variable_In_Outer_Scope() {
            var stringLength = 0;
            var task = StringTask.Do(s => stringLength = s.Length);
            Assert.Equal(0, stringLength);
            var str = await task;
            Assert.Equal(str.Length, stringLength);
        }

        [Fact]
        public async Task String_Task_Catch_Exception() {
            var select = StringTask.Do(s => throw new Exception("Exception"));
            try {
                await select;
            }
            catch (Exception e) {
                Assert.Equal("Exception", e.Message);
            }
        }

        [Fact]
        public Task String_Task_Null_Action_Throws() {
            Action<string> action = null;
            return Assert.ThrowsAsync<ArgumentNullException>(() => StringTask.Do(action));
        }

        [Fact]
        public async Task String_Task_Run_Catch_Exception() {
            var select = StringTask.Do(s => throw new Exception("Exception"));
            var task = Task.Run(() => select);
            try {
                await task;
            }
            catch (Exception e) {
                Assert.Equal("Exception", e.Message);
            }
        }

        [Fact]
        public async Task Void_Task_Assign_Variable_In_Outer_Scope() {
            var stringLength = 0;
            var task = VoidTask.Do(() => stringLength = 5);
            Assert.Equal(0, stringLength);
            await task;
            Assert.Equal(5, stringLength);
        }

        [Fact]
        public async Task Void_Task_Catch_Exception() {
            var select = StringTask.Do(() => throw new Exception("Exception"));
            try {
                await select;
            }
            catch (Exception e) {
                Assert.Equal("Exception", e.Message);
            }
        }

        [Fact]
        public Task Void_Task_Null_Action_Throws() {
            Action action = null;
            return Assert.ThrowsAsync<ArgumentNullException>(() => VoidTask.Do(action));
        }

        [Fact]
        public async Task Void_Task_Run_Catch_Exception() {
            var select = StringTask.Do(() => throw new Exception("Exception"));
            var task = Task.Run(() => select);
            try {
                await task;
            }
            catch (Exception e) {
                Assert.Equal("Exception", e.Message);
            }
        }
    }
}