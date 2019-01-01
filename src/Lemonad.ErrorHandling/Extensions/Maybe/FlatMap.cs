using System;
using Lemonad.ErrorHandling.Internal;

namespace Lemonad.ErrorHandling.Extensions.Maybe {
    public static partial class Index {
        /// <summary>
        ///     Flatmaps a <see cref="Nullable{T}" />.
        /// </summary>
        /// <param name="source">
        ///     The <see cref="IMaybe{T}" /> to flatmap a <see cref="Nullable{T}" /> with.
        /// </param>
        /// <param name="flatSelector">
        ///     A function who expects a <see cref="Nullable{T}" /> as its return type.
        /// </param>
        /// <typeparam name="T">
        ///     The type from <see cref="IMaybe{T}" />.
        /// </typeparam>
        /// <typeparam name="TResult">
        ///     The type <typeparamref name="T" /> returned from the <paramref name="flatSelector" /> function.
        /// </typeparam>
        public static IMaybe<TResult> FlatMap<T, TResult>(this IMaybe<T> source, Func<T, TResult?> flatSelector)
            where TResult : struct {
            if (flatSelector is null) throw new ArgumentNullException(nameof(flatSelector));
            if (source is null) throw new ArgumentNullException(nameof(source));
            if (!source.HasValue) return Maybe<TResult>.None;
            var selector = flatSelector(source.Value);
            return selector.HasValue ? Maybe<TResult>.Create(selector.Value) : Maybe<TResult>.None;
        }
    }
}