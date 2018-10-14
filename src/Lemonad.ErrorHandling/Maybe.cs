using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.Linq;
using Lemonad.ErrorHandling.Internal;

namespace Lemonad.ErrorHandling {
    public static class Maybe {
        /// <summary>
        ///     Works just like <see cref="Enumerable.FirstOrDefault{TSource}(System.Collections.Generic.IEnumerable{TSource})" />
        ///     but returns a <see cref="Maybe{T}" />.
        /// </summary>
        /// <param name="source">
        ///     The <see cref="IEnumerable{T}" />.
        /// </param>
        /// <typeparam name="TSource">
        ///     The type of the <see cref="IEnumerable{T}" />.
        /// </typeparam>
        /// <returns>
        ///     Returns the first element of a sequence inside a <see cref="Maybe{T}" />.
        /// </returns>
        public static IMaybe<TSource> FirstMaybe<TSource>(this IEnumerable<TSource> source) {
            switch (source) {
                case IList<TSource> list when list.Count > 0:
                    return list[0].ToMaybe();
                case IReadOnlyList<TSource> readOnlyList when readOnlyList.Count > 0:
                    return readOnlyList[0].ToMaybe();
                default:
                    using (var e = source.GetEnumerator()) {
                        return e.MoveNext() ? e.Current.ToMaybe() : Maybe<TSource>.None;
                    }
            }
        }

        /// <summary>
        ///     Works just like <see cref="Enumerable.FirstOrDefault{TSource}(System.Collections.Generic.IEnumerable{TSource})" />
        ///     but returns a <see cref="Maybe{T}" />.
        /// </summary>
        /// <param name="source">
        ///     The <see cref="IEnumerable{T}" />.
        /// </param>
        /// <param name="predicate">
        ///     A function to test each element for a condition.
        /// </param>
        /// <typeparam name="TSource">
        ///     The type of the <see cref="IEnumerable{T}" />.
        /// </typeparam>
        /// <returns>
        ///     Returns the first element of a sequence who matches the <paramref name="predicate" /> inside a
        ///     <see cref="Maybe{T}" />.
        /// </returns>
        public static IMaybe<TSource> FirstMaybe<TSource>(this IEnumerable<TSource> source,
            Func<TSource, bool> predicate) {
            foreach (var element in source)
                if (predicate(element))
                    return element.ToMaybe();

            return Maybe<TSource>.None;
        }

        /// <summary>
        ///     Executes <see cref="Maybe{T}.Match" /> for each element in the sequence.
        /// </summary>
        public static IEnumerable<TResult> Match<TSource, TResult>(this IEnumerable<IMaybe<TSource>> source,
            Func<TSource, TResult> someSelector, Func<TResult> noneSelector) =>
            source.Select(x => x.Match(someSelector, noneSelector));

        /// <summary>
        ///     Converts an <see cref="IEnumerable{T}" /> of <see cref="Maybe{T}" /> into an <see cref="IEnumerable{T}" /> of
        ///     <typeparamref name="TResult" /> for each element which do not have a value.
        /// </summary>
        /// <param name="source">
        ///     The <see cref="IEnumerable{T}" /> of <see cref="Maybe{T}" />.
        /// </param>
        /// <param name="selector">
        ///     A function to return a value for each <see cref="Maybe{T}" />.
        /// </param>
        /// <typeparam name="TSource">
        ///     The type of the <see cref="Maybe{T}" />.
        /// </typeparam>
        /// <typeparam name="TResult">
        ///     The return type returned by the function <paramref name="selector" />.
        /// </typeparam>
        public static IEnumerable<TResult> NoValues<TSource, TResult>(this IEnumerable<IMaybe<TSource>> source,
            Func<TResult> selector) => source.Where(x => x.HasValue == false).Select(_ => selector());

        /// <summary>
        ///     Treat <typeparamref name="TSource" /> as enumerable with 0-1 elements in.
        ///     This is handy when combining <see cref="Maybe{T}" /> with LINQ's API.
        /// </summary>
        public static IEnumerable<TSource> ToEnumerable<TSource>(this IMaybe<TSource> source) {
            if (source.HasValue)
                yield return source.Value;
        }

        /// <summary>
        ///     Creates a <see cref="Maybe{T}" /> who will have the value <paramref name="item" />.
        /// </summary>
        /// <param name="item">
        ///     The value of <see cref="Maybe{T}" />.
        /// </param>
        /// <typeparam name="TSource">
        ///     The type of the <see cref="Maybe{T}" />.
        /// </typeparam>
        /// <returns>
        ///     A <see cref="Maybe{T}" /> whose value will be <paramref name="item" />.
        /// </returns>
        [Pure]
        public static IMaybe<TSource> ToMaybe<TSource>(this TSource item) => Maybe<TSource>.Create(item);

        /// <summary>
        ///     Works like <see cref="ToMaybe{TSource}(TSource)" /> but with an <paramref name="predicate" /> to test the element.
        /// </summary>
        /// <param name="source">
        ///     The element to be passed into <see cref="Maybe{T}" />.
        /// </param>
        /// <param name="predicate">
        ///     A function to test the element.
        /// </param>
        /// <typeparam name="TSource">
        ///     The type of the <paramref name="source" />.
        /// </typeparam>
        [Pure]
        public static IMaybe<TSource> ToMaybe<TSource>(this TSource source, Func<TSource, bool> predicate) {
            if (predicate != null)
                return predicate(source) ? ToMaybe(source) : Maybe<TSource>.None;
            throw new ArgumentNullException(nameof(predicate));
        }

        /// <summary>
        ///     Converts an <see cref="Nullable{T}" /> to an <see cref="Maybe{T}" />.
        /// </summary>
        /// <param name="source">
        ///     The element from the <see cref="Nullable{T}" /> to be passed into <see cref="Maybe{T}" />.
        /// </param>
        /// <typeparam name="TSource">
        ///     The type of the <paramref name="source" />.
        /// </typeparam>
        [Pure]
        public static IMaybe<TSource> ToMaybe<TSource>(this TSource? source) where TSource : struct =>
            source.HasValue ? source.Value.ToMaybe() : ToMaybeNone<TSource>();

        /// <summary>
        ///     Creates a <see cref="Maybe{T}" /> who will have no value.
        /// </summary>
        /// <typeparam name="TSource">
        ///     The type of the <see cref="Maybe{T}" />.
        /// </typeparam>
        /// <returns>
        ///     A <see cref="Maybe{T}" /> who will have no value.
        /// </returns>
        [Pure]
        public static IMaybe<TSource> ToMaybeNone<TSource>() => Maybe<TSource>.None;

        /// <summary>
        ///     Creates a <see cref="Maybe{T}" /> who will have no value.
        /// </summary>
        /// <param name="item">
        ///     The value that will be considered to not have a value.
        /// </param>
        /// <typeparam name="TSource">
        ///     The type of the <see cref="Maybe{T}" />.
        /// </typeparam>
        /// <returns>
        ///     A <see cref="Maybe{T}" /> who will have no value.
        /// </returns>
        [Pure]
        public static IMaybe<TSource> ToMaybeNone<TSource>(this TSource item) => Maybe<TSource>.None;

        /// <summary>
        ///     Works like <see cref="ToMaybeNone{TSource}(TSource)" /> but with an <paramref name="predicate" /> to test the
        ///     element.
        /// </summary>
        /// <param name="source">
        ///     The element to be passed into <see cref="Maybe{T}" />.
        /// </param>
        /// <param name="predicate">
        ///     A function to test the element.
        /// </param>
        /// <typeparam name="TSource">
        ///     The type of the <paramref name="source" />.
        /// </typeparam>
        [Pure]
        public static IMaybe<TSource> ToMaybeNone<TSource>(this TSource source, Func<TSource, bool> predicate) {
            if (predicate == null) throw new ArgumentNullException(nameof(predicate));
            return predicate(source) ? Maybe<TSource>.None : source.ToMaybe();
        }

        /// <summary>
        ///     Converts an <see cref="Maybe{T}" /> to an <see cref="Result{T,TError}" />.
        /// </summary>
        /// <param name="source">
        ///     The element from the <see cref="Maybe{T}" /> to be passed into <see cref="Result{T,TError}" />.
        /// </param>
        /// <param name="errorSelector">
        ///     A function to be executed if there is no value inside the <see cref="Maybe{T}" />.
        /// </param>
        /// <typeparam name="T">
        ///     The type inside the <see cref="Maybe{T}" />.
        /// </typeparam>
        /// <typeparam name="TError">
        ///     The type representing an error for the <see cref="Result{T,TError}" />.
        /// </typeparam>
        /// <returns></returns>
        [Pure]
        public static IResult<T, TError> ToResult<T, TError>(this IMaybe<T> source, Func<TError> errorSelector) =>
            source.ToResult(x => x.HasValue, x => errorSelector == null
                ? throw new ArgumentNullException(nameof(errorSelector))
                : errorSelector()).Map(x => x.Value);

        /// <summary>
        ///     Converts an <see cref="IEnumerable{T}" /> of <see cref="Maybe{T}" /> into an <see cref="IEnumerable{T}" /> with the
        ///     value of the <see cref="Maybe{T}" />.
        /// </summary>
        /// <param name="source">
        ///     The <see cref="IEnumerable{T}" /> of <see cref="Maybe{T}" />.
        /// </param>
        /// <typeparam name="TSource">
        ///     The type inside the <see cref="Maybe{T}" />.
        /// </typeparam>
        /// <returns>
        ///     A sequence which can contain 0-n amount of values.
        /// </returns>
        public static IEnumerable<TSource> Values<TSource>(this IEnumerable<IMaybe<TSource>> source) =>
            source.SelectMany(x => x.ToEnumerable());
    }
}