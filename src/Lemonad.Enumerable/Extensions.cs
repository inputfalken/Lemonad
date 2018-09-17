using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace Lemonad.Enumerable {
    public static class Extensions {
        /// <summary>
        /// Chunks the elements of a sequence according to a amount and creates a result value from each group and its max capacity.
        /// </summary>
        /// <param name="source">An <see cref="IEnumerable{T}"/> whose elements to chunk.</param>
        /// <param name="count">The max capacity for each <see cref="IChunk{T}"/>.</param>
        /// <typeparam name="TSource">The type of the elements in <paramref name="source"/>.</typeparam>
        /// <returns>The type of the elements <paramref name="source"/>.</returns>
        /// <exception cref="ArgumentNullException">When <paramref name="source"/> is null.</exception>
        /// <exception cref="ArgumentException">When <paramref name="count"/> is below 1.</exception>
        public static IEnumerable<IChunk<TSource>> ChunkBy<TSource>(this IEnumerable<TSource> source, int count) {
            if (source == null) throw new ArgumentNullException(nameof(source));
            if (count < 1) throw new ArgumentException($"Parameter: '{nameof(count)}' ({count}) can not be below 1.");
            return source
                .Select((x, y) => (Element: x, Index: y))
                .GroupBy<(TSource Element, int Index), int, IChunk<TSource>>(
                    x => x.Index / count,
                    (_, y) => new Chunk<TSource>(y.Select(x => x.Element), count)
                );
        }

        private readonly struct Chunk<T> : IChunk<T> {
            private readonly IEnumerable<T> _chunk;

            public Chunk(in IEnumerable<T> chunk, int count) {
                _chunk = chunk;
                MaxCount = count;
            }

            public IEnumerator<T> GetEnumerator() => _chunk.GetEnumerator();

            IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();

            /// <summary>
            /// The max amount of allowed elements in <see cref="IChunk{T}"/>.
            /// </summary>
            public int MaxCount { get; }
        }
    }

    /// <summary>
    /// Represents a collection of elements that have a common max count.
    /// </summary>
    /// <typeparam name="T">The type of the elements.</typeparam>
    public interface IChunk<out T> : IEnumerable<T> {
        int MaxCount { get; }
    }
}