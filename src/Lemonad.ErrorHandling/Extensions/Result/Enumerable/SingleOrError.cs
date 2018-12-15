using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;

namespace Lemonad.ErrorHandling.Extensions.Result.Enumerable {
    public static partial class ResultEnumerable {
        /// <summary>
        ///  Returns the only element of a sequence, or a <see cref="SingleOrErrorCase.NoElement"/> if the sequence is empty and returns <see cref="SingleOrErrorCase.ManyElements"/> if more than one element was found.
        /// </summary>
        /// <param name="source">
        /// A <see cref="IQueryable{T}"/> to return an <see cref="IResult{T,TError}"/> from.
        /// </param>
        /// <typeparam name="TSource">
        /// The type of the elements in <see cref="IQueryable{T}"/>.
        /// </typeparam>
        /// <returns>
        /// The single element of the input sequence, or <see cref="SingleOrErrorCase"/> otherwise inside a <see cref="IResult{T,TError}"/>.
        /// </returns>
        public static IResult<TSource, SingleOrErrorCase> SingleOrError<TSource>(this IEnumerable<TSource> source) {
            var sources = source
                .Take(2)
                .ToArray();
            if (sources.Length == 1) return ErrorHandling.Result.Value<TSource, SingleOrErrorCase>(sources[0]);
            return ErrorHandling.Result.Error<TSource, SingleOrErrorCase>(sources.Length == 0
                ? SingleOrErrorCase.NoElement
                : SingleOrErrorCase.ManyElements
            );
        }

        /// <summary>
        ///  Returns the only element of a sequence, or a <see cref="SingleOrErrorCase.NoElement"/> if the sequence is empty and returns <see cref="SingleOrErrorCase.ManyElements"/> if more than one element was found.
        /// </summary>
        /// <param name="source">
        /// A <see cref="IQueryable{T}"/> to return an <see cref="IResult{T,TError}"/> from.
        /// </param>
        /// <param name="predicate">
        /// A function to test each element for a condition.
        /// </param>
        /// <typeparam name="TSource">
        /// The type of the elements in <see cref="IQueryable{T}"/>.
        /// </typeparam>
        /// <returns>
        /// The single element of the input sequence, or <see cref="SingleOrErrorCase"/> otherwise inside a <see cref="IResult{T,TError}"/>.
        /// </returns>
        public static IResult<TSource, SingleOrErrorCase> SingleOrError<TSource>(
            this IEnumerable<TSource> source,
            Func<TSource, bool> predicate
        ) => source.Where(predicate).SingleOrError();

        /// <summary>
        ///  Returns the only element of a sequence, or a <see cref="SingleOrErrorCase.NoElement"/> if the sequence is empty and returns <see cref="SingleOrErrorCase.ManyElements"/> if more than one element was found.
        /// </summary>
        /// <param name="source">
        /// A <see cref="IQueryable{T}"/> to return an <see cref="IResult{T,TError}"/> from.
        /// </param>
        /// <typeparam name="TSource">
        /// The type of the elements in <see cref="IQueryable{T}"/>.
        /// </typeparam>
        /// <returns>
        /// The single element of the input sequence, or <see cref="SingleOrErrorCase"/> otherwise inside a <see cref="IResult{T,TError}"/>.
        /// </returns>
        public static IResult<TSource, SingleOrErrorCase> SingleOrError<TSource>(this IQueryable<TSource> source) {
            var sources = source
                .Take(2)
                .ToArray();
            if (sources.Length == 1) return ErrorHandling.Result.Value<TSource, SingleOrErrorCase>(sources[0]);
            return ErrorHandling.Result.Error<TSource, SingleOrErrorCase>(sources.Length == 0
                ? SingleOrErrorCase.NoElement
                : SingleOrErrorCase.ManyElements
            );
        }

        /// <summary>
        ///  Returns the only element of a sequence, or a <see cref="SingleOrErrorCase.NoElement"/> if the sequence is empty and returns <see cref="SingleOrErrorCase.ManyElements"/> if more than one element was found.
        /// </summary>
        /// <param name="source">
        /// A <see cref="IQueryable{T}"/> to return an <see cref="IResult{T,TError}"/> from.
        /// </param>
        /// <param name="predicate">
        /// A <see cref="Expression{TDelegate}"/> to test each element for a condition.
        /// </param>
        /// <typeparam name="TSource">
        /// The type of the elements in <see cref="IQueryable{T}"/>.
        /// </typeparam>
        /// <returns>
        /// The single element of the input sequence, or <see cref="SingleOrErrorCase"/> otherwise inside a <see cref="IResult{T,TError}"/>.
        /// </returns>
        public static IResult<TSource, SingleOrErrorCase> SingleOrError<TSource>(
            this IQueryable<TSource> source,
            Expression<Func<TSource, bool>> predicate
        ) => source.Where(predicate).SingleOrError();
    }

    /// <summary>
    /// Represents the cases in the SingleOrError extension method for <see cref="System.Linq.Enumerable"/>.
    /// </summary>
    public enum SingleOrErrorCase {
        NoElement,
        ManyElements
    }
}