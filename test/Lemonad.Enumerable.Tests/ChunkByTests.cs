using System;
using System.Collections.Generic;
using Xunit;

namespace Lemonad.Enumerable.Tests {
    public class ChunkBy {
        [Fact]
        public void Transform_Enumerable_Of_10_Into_Chunks_With_Size_3() {
            const int chunkSize = 3;
            var arr = System.Linq.Enumerable.Range(0, 10).ChunkBy(chunkSize);

            IEnumerable<IEnumerable<int>> expected = new[] {
                new[] {0, 1, 2},
                new[] {3, 4, 5},
                new[] {6, 7, 8},
                new[] {9},
            };
            Assert.Equal(expected, arr);
        }

        [Fact]
        public void Transform_Enumerable_Of_10_Into_Chunks_With_Size_10() {
            const int chunkSize = 10;
            var arr = System.Linq.Enumerable.Range(0, 100).ChunkBy(chunkSize);

            IEnumerable<int>[] expected = {
                System.Linq.Enumerable.Range(0, 10),
                System.Linq.Enumerable.Range(10, 10),
                System.Linq.Enumerable.Range(20, 10),
                System.Linq.Enumerable.Range(30, 10),
                System.Linq.Enumerable.Range(40, 10),
                System.Linq.Enumerable.Range(50, 10),
                System.Linq.Enumerable.Range(60, 10),
                System.Linq.Enumerable.Range(70, 10),
                System.Linq.Enumerable.Range(80, 10),
                System.Linq.Enumerable.Range(90, 10),
            };
            Assert.Equal(expected, arr);
        }

        [Fact]
        public void Transform_Enumerable_Of_100_Into_Chunks_With_Size_Eleven() {
            const int chunkSize = 11;
            var arr = System.Linq.Enumerable.Range(0, 100).ChunkBy(chunkSize);

            IEnumerable<int>[] expected = {
                System.Linq.Enumerable.Range(0, 11),
                System.Linq.Enumerable.Range(11, 11),
                System.Linq.Enumerable.Range(22, 11),
                System.Linq.Enumerable.Range(33, 11),
                System.Linq.Enumerable.Range(44, 11),
                System.Linq.Enumerable.Range(55, 11),
                System.Linq.Enumerable.Range(66, 11),
                System.Linq.Enumerable.Range(77, 11),
                System.Linq.Enumerable.Range(88, 11),
                System.Linq.Enumerable.Range(99, 1),
            };
            Assert.Equal(expected, arr);
        }

        [Fact]
        public void Supplying_Negative_Size_Throws_ArgumentException() => Assert.Throws<ArgumentException>(
            () => System.Linq.Enumerable.Range(0, 10).ChunkBy(-1));

        [Fact]
        public void Supplying_Null_Enumerable_Throws_ArgumentNullException() {
            IEnumerable<int> enumerable = null;
            Assert.Throws<ArgumentNullException>("source", () => enumerable.ChunkBy(4));
        }
    }
}