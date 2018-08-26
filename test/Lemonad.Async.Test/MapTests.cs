using System;
using System.Threading.Tasks;
using Lemonad.Async.Extensions;
using Xunit;

namespace Lemonad.Async.Test {
    public class SelectTests {
        private const string Text = "hello";

        private static Task<string> StringTask => Task.Run(async () => {
            await Task.Delay(100);
            return Text;
        });

        [Fact]
        public async Task Catch_Exception() {
            var select = StringTask.Map<string, int>(s => throw new Exception("Exception"));
            try {
                await select;
            }
            catch (Exception e) {
                Assert.Equal("Exception", e.Message);
            }
        }

        [Fact]
        public void Null_String_Task() {
            Task<string> task = null;
            Assert.ThrowsAsync<ArgumentNullException>(() => task.Map(s => s.Length));
        }

        [Fact]
        public void Null_Task() {
            Task task = null;
            Assert.ThrowsAsync<ArgumentNullException>(() => task.Map(() => Text));
        }

        [Fact]
        public void String_Task_Null_Selector() {
            Func<string, int> selector = null;
            Assert.ThrowsAsync<ArgumentNullException>(() => StringTask.Map(selector));
        }

        [Fact]
        public void Task_Null_Selector() {
            Func<int> selector = null;
            var task = Task.Delay(100);
            Assert.ThrowsAsync<ArgumentNullException>(() => task.Map(selector));
        }

        [Fact]
        public async Task Task_Run_Catch_Exception() {
            var select = StringTask.Map<string, int>(s => throw new Exception("Exception"));
            var task = Task.Run(() => select);
            try {
                await task;
            }
            catch (Exception e) {
                Assert.Equal("Exception", e.Message);
            }
        }

        [Fact]
        public async Task Transform_String_Task_To_Length() {
            var length = await StringTask.Map(s => s.Length);
            Assert.Equal(Text.Length, length);
        }

        [Fact]
        public async Task Transform_Void_Task_To_String_Task() {
            var text = await Task.Delay(100).Map(() => Text);
            Assert.Equal(Text, text);
        }
    }
}