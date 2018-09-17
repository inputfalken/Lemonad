using System;
using System.Collections.Generic;
using System.Linq;
using Xunit;

namespace Lemonad.Enumerable.Tests {
    public class ChunkByTests {
        [Fact]
        public void Chunk_Enumerable_Of_10_Into_Four_Chunks() {
            const int chunkSize = 3;
            var chunks = System.Linq.Enumerable.Range(0, 10).ChunkBy(chunkSize).Select(x => x).ToArray();
            // Verify all chunks have equal max size.
            Assert.All(chunks, x => Assert.Equal(chunkSize, x.MaxCount));
            var arr = chunks.Select(x => x.ToArray()).ToArray();

            Assert.Equal(4, arr.Length);

            // First chunk
            Assert.Equal(0, arr[0][0]);
            Assert.Equal(1, arr[0][1]);
            Assert.Equal(2, arr[0][2]);

            // Second chunk
            Assert.Equal(3, arr[1][0]);
            Assert.Equal(4, arr[1][1]);
            Assert.Equal(5, arr[1][2]);

            // Third chunk
            Assert.Equal(6, arr[2][0]);
            Assert.Equal(7, arr[2][1]);
            Assert.Equal(8, arr[2][2]);

            // Fourth chunk
            Assert.Equal(9, arr[3][0]);
        }

        [Fact]
        public void Chunk_Enumerable_Of_10_With_Negative_Integer_Throws() => Assert.Throws<ArgumentException>(
            () => System.Linq.Enumerable.Range(0, 10).ChunkBy(-1).Select(x => x.ToArray()).ToArray());

        [Fact]
        public void Chunk_Null_Enumerable_Throws() {
            IEnumerable<int> enumerable = null;
            Assert.Throws<ArgumentNullException>("source", () => enumerable.ChunkBy(4));
        }
    }
}