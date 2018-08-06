using System;
using System.Threading.Tasks;
using Lemonad.Async.Extensions;
using Xunit;

namespace Lemonad.Async.Test {
    public class SelectManyTests {
        private const string Text = "Hello";

        private static Task<string> StringTask => Task.Run(async () => {
            await Task.Delay(100);
            return Text;
        });

        private int _flatMapCounter;

        private Task VoidTask => Task.Run(async () => {
            await Task.Delay(100);
            _flatMapCounter++;
        });

        [Fact]
        public async Task String_Task_Catch_Exception() {
            var select = StringTask.FlatMap(s => throw new Exception("Exception"));
            try {
                await select;
            }
            catch (Exception e) {
                Assert.Equal("Exception", e.Message);
            }
        }

        [Fact]
        public async Task String_Task_Catch_Exception_With_ResultSelector() {
            var select = StringTask.FlatMap(s => throw new Exception("Exception"), s => s.Length);
            try {
                await select;
            }
            catch (Exception e) {
                Assert.Equal("Exception", e.Message);
            }
        }

        [Fact]
        public async Task String_Task_FlatMap_String_Task() {
            var flatMapped = await StringTask.FlatMap(async s => {
                await Task.Delay(100);
                return s + "World";
            });
            Assert.Equal("HelloWorld", flatMapped);
        }

        [Fact]
        public async Task String_Task_FlatMap_String_Task_With_ResultSelector() {
            var flatMapped = await StringTask
                .FlatMap(async s => {
                    await VoidTask;
                    return s + "World";
                }, (s, s1) => s + s1);
            Assert.Equal("HelloHelloWorld", flatMapped);
        }

        [Fact]
        public async Task String_Task_FlatMap_Void_Task() {
            var flatMapped = StringTask.FlatMap(s => VoidTask);
            Assert.Equal(0, _flatMapCounter);
            await flatMapped;
            Assert.Equal(1, _flatMapCounter);
        }

        [Fact]
        public async Task String_Task_FlatMap_Void_Task_With_ResultSelector() {
            var flatMapped = StringTask.FlatMap(s => VoidTask, s => s + 1);
            Assert.Equal(0, _flatMapCounter);
            Assert.Equal("Hello1", await flatMapped);
            Assert.Equal(1, _flatMapCounter);
        }

        [Fact]
        public async Task String_Task_Run_Catch_Exception() {
            var select = StringTask.FlatMap<string, int>(s => throw new Exception("Exception"));
            var task = Task.Run(() => select);
            try {
                await task;
            }
            catch (Exception e) {
                Assert.Equal("Exception", e.Message);
            }
        }

        [Fact]
        public async Task String_Task_Run_Catch_Exception_With_ResultSelector() {
            var select = StringTask.FlatMap(s => throw new Exception("Exception"), s => s.Length);
            var task = Task.Run(() => select);
            try {
                await task;
            }
            catch (Exception e) {
                Assert.Equal("Exception", e.Message);
            }
        }

        [Fact]
        public async Task Void_Task_Catch_Exception() {
            var select = VoidTask.FlatMap(() => throw new Exception("Exception"));
            try {
                await select;
            }
            catch (Exception e) {
                Assert.Equal("Exception", e.Message);
            }
        }

        [Fact]
        public async Task Void_Task_Catch_Exception_With_ResultSelector() {
            var select = VoidTask.FlatMap(() => throw new Exception("Exception"), () => 5);
            try {
                await select;
            }
            catch (Exception e) {
                Assert.Equal("Exception", e.Message);
            }
        }

        [Fact]
        public async Task Void_Task_FlatMap_String_Task() {
            var flatMapped = VoidTask.FlatMap(() => StringTask);
            Assert.Equal(0, _flatMapCounter);
            Assert.Equal("Hello", await flatMapped);
            Assert.Equal(1, _flatMapCounter);
        }

        [Fact]
        public async Task Void_Task_FlatMap_String_Task_Using_ResultSelector() {
            var flatMapped = VoidTask.FlatMap(() => StringTask, s => s + 1);
            Assert.Equal(0, _flatMapCounter);
            Assert.Equal("Hello1", await flatMapped);
            Assert.Equal(1, _flatMapCounter);
        }

        [Fact]
        public async Task Void_Task_FlatMap_Void_Task() {
            var flatMapped = VoidTask.FlatMap(() => VoidTask);
            Assert.Equal(0, _flatMapCounter);
            await flatMapped;
            Assert.Equal(2, _flatMapCounter);
        }

        [Fact]
        public async Task Void_Task_FlatMap_Void_Task_Using_ResultSelector() {
            var flatMapped = VoidTask.FlatMap(() => VoidTask, () => "Hello");
            Assert.Equal(0, _flatMapCounter);
            Assert.Equal("Hello", await flatMapped);
            Assert.Equal(2, _flatMapCounter);
        }

        [Fact]
        public async Task Void_Task_Run_Catch_Exception() {
            var select = VoidTask.FlatMap(() => throw new Exception("Exception"));
            var task = Task.Run(() => select);
            try {
                await task;
            }
            catch (Exception e) {
                Assert.Equal("Exception", e.Message);
            }
        }

        [Fact]
        public async Task Void_Task_Run_Catch_Exception_With_ResultSelector() {
            var select = VoidTask.FlatMap(() => throw new Exception("Exception"), () => 5);
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