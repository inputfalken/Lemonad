using System;
using System.Linq;
using System.Linq.Expressions;
using Lemonad.ErrorHandling.Extensions.Result;
using Lemonad.ErrorHandling.Extensions.Task;
using Microsoft.EntityFrameworkCore;

namespace Lemonad.ErrorHandling.EntityFramework.Core {
    public static partial class Index {
        /// <summary>
        ///     Returns the only element of a sequence, or a <see cref="SingleOrErrorCase.NoElement" /> if the sequence is empty
        ///     and returns <see cref="SingleOrErrorCase.ManyElements" /> if more than one element was found.
        /// </summary>
        /// <param name="source">
        ///     A <see cref="IQueryable{T}" /> to return an <see cref="IResult{T,TError}" /> from.
        /// </param>
        /// <typeparam name="TSource">
        ///     The type of the elements in <see cref="IQueryable{T}" />.
        /// </typeparam>
        /// <returns>
        ///     The single element of the input sequence, or <see cref="SingleOrErrorCase" /> otherwise inside a
        ///     <see cref="IResult{T,TError}" />.
        /// </returns>
        public static IAsyncResult<TSource[], SingleOrErrorCase> SingleOrErrorAsync<TSource>(this IQueryable<TSource> source) {
            if (source is null) throw new ArgumentNullException(nameof(source));
            return source
                .Take(2)
                .ToArrayAsync()
                .ToAsyncResult(x => x.Length == 1, x => x.Length == 0
                    ? SingleOrErrorCase.NoElement
                    : SingleOrErrorCase.ManyElements
                );
        }

        /// <summary>
        ///     Returns the only element of a sequence, or a <see cref="SingleOrErrorCase.NoElement" /> if the sequence is empty
        ///     and returns <see cref="SingleOrErrorCase.ManyElements" /> if more than one element was found.
        /// </summary>
        /// <param name="source">
        ///     A <see cref="IQueryable{T}" /> to return an <see cref="IResult{T,TError}" /> from.
        /// </param>
        /// <param name="predicate">
        ///     A <see cref="Expression{TDelegate}" /> to test each element for a condition.
        /// </param>
        /// <typeparam name="TSource">
        ///     The type of the elements in <see cref="IQueryable{T}" />.
        /// </typeparam>
        /// <returns>
        ///     The single element of the input sequence, or <see cref="SingleOrErrorCase" /> otherwise inside a
        ///     <see cref="IResult{T,TError}" />.
        /// </returns>
        public static IAsyncResult<TSource[], SingleOrErrorCase> SingleOrErrorAsync<TSource>(
            this IQueryable<TSource> source,
            Expression<Func<TSource, bool>> predicate
        ) {
            if (source is null) throw new ArgumentNullException(nameof(source));
            if (predicate is null) throw new ArgumentNullException(nameof(predicate));
            return source.Where(predicate).SingleOrErrorAsync();
        }
    }
}