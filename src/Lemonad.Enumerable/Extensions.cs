using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace Lemonad.Enumerable {
    public static partial class Enumerable {
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
            if (source == null) {
                throw new ArgumentNullException(nameof(source));
            }

            if (count < 1) {
                throw new ArgumentException($"Parameter: '{nameof(count)}' ({count}) can not be below 1.");
            }

            foreach (IEnumerable<TSource> enumerable in CreateChunks(source, count)) {
                yield return new Chunk<TSource>(enumerable, count);
            }
        }

        private static IEnumerable<IEnumerable<TSource>> CreateChunks<TSource>(IEnumerable<TSource> source, int count) {
            var index = 0;
            var elements = new Queue<TSource>();
            foreach (var element in source) {
                elements.Enqueue(element);
                checked {
                    index++;
                }
                if (index % count == 0) {
                    yield return elements;
                    elements = new Queue<TSource>();
                }
            }

            yield return elements;
        }
    }

    public sealed class Chunk<T> : IChunk<T> {
        private readonly IEnumerable<T> _chunk;

        public Chunk(IEnumerable<T> chunk, int count) {
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

    /// <summary>
    /// Represents a collection of elements that have a common max count.
    /// </summary>
    /// <typeparam name="T">The type of the elements.</typeparam>
    public interface IChunk<out T> : IEnumerable<T> {
        int MaxCount { get; }
    }
}