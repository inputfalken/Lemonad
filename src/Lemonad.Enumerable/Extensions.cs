using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace Lemonad.Enumerable {
    public static class Extensions {
        public static IEnumerable<IChunk<T>> ChunkBy<T>(this IEnumerable<T> source, int count) {
            return source
                .Select((x, y) => (Element: x, Index: y))
                .GroupBy<(T Element, int Index), int, IChunk<T>>(
                    x => x.Index / count,
                    (_, y) => new Chunk<T>(y.Select(x => x.Element), count)
                );
        }

        public interface IChunk<out T> : IEnumerable<T> {
            int MaxCount { get; }
        }

        private readonly struct Chunk<T> : IChunk<T> {
            private readonly IEnumerable<T> _chunk;

            public Chunk(IEnumerable<T> chunk, int count) {
                _chunk = chunk;
                MaxCount = count;
            }

            public IEnumerator<T> GetEnumerator() => _chunk.GetEnumerator();

            IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();

            public int MaxCount { get; }
        }
    }
}