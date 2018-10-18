using System;
using System.Linq;
using System.Linq.Expressions;

namespace Lemonad.ErrorHandling.EnumerableExtensions {
    public static class ResultQueryable {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="source"></param>
        /// <param name="errorSelector"></param>
        /// <typeparam name="TSource"></typeparam>
        /// <typeparam name="TError"></typeparam>
        /// <returns></returns>
        public static IResult<TSource, TError> FirstOrError<TSource, TError>(this IQueryable<TSource> source,
            Func<TError> errorSelector) where TSource : class =>
            source.FirstOrDefault().ToResult(x => x != null, _ => errorSelector());

        /// <summary>
        /// 
        /// </summary>
        /// <param name="source"></param>
        /// <param name="errorSelector"></param>
        /// <typeparam name="TSource"></typeparam>
        /// <typeparam name="TError"></typeparam>
        /// <returns></returns>
        public static IResult<TSource, TError> SingleOrError<TSource, TError>(this IQueryable<TSource> source,
            Func<TError> errorSelector) where TSource : class =>
            source.SingleOrDefault().ToResult(x => x != null, _ => errorSelector());

        /// <summary>
        /// 
        /// </summary>
        /// <param name="source"></param>
        /// <param name="predicate"></param>
        /// <param name="errorSelector"></param>
        /// <typeparam name="TSource"></typeparam>
        /// <typeparam name="TError"></typeparam>
        /// <returns></returns>
        public static IResult<TSource, TError> FirstOrError<TSource, TError>(this IQueryable<TSource> source,
            Expression<Func<TSource, bool>> predicate, Func<TError> errorSelector) where TSource : class =>
            source.FirstOrDefault(predicate).ToResult(x => x != null, _ => errorSelector());

        /// <summary>
        /// 
        /// </summary>
        /// <param name="source"></param>
        /// <param name="predicate"></param>
        /// <param name="errorSelector"></param>
        /// <typeparam name="TSource"></typeparam>
        /// <typeparam name="TError"></typeparam>
        /// <returns></returns>
        public static IResult<TSource, TError> SingleOrError<TSource, TError>(this IQueryable<TSource> source,
            Expression<Func<TSource, bool>> predicate, Func<TError> errorSelector) where TSource : class =>
            source.SingleOrDefault(predicate).ToResult(x => x != null, _ => errorSelector());
        
        /// <summary>
        /// 
        /// </summary>
        /// <param name="source"></param>
        /// <param name="errorSelector"></param>
        /// <typeparam name="TSource"></typeparam>
        /// <typeparam name="TError"></typeparam>
        /// <returns></returns>
        public static IResult<TSource, TError> FirstOrError<TSource, TError>(this IQueryable<TSource?> source,
            Func<TError> errorSelector) where TSource : struct =>
            source.FirstOrDefault().ToResult(x => x.HasValue, _ => errorSelector()).Map(x => x.Value);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="source"></param>
        /// <param name="errorSelector"></param>
        /// <typeparam name="TSource"></typeparam>
        /// <typeparam name="TError"></typeparam>
        /// <returns></returns>
        public static IResult<TSource, TError> SingleOrError<TSource, TError>(this IQueryable<TSource?> source,
            Func<TError> errorSelector) where TSource : struct =>
            source.SingleOrDefault().ToResult(x => x.HasValue, _ => errorSelector()).Map(x => x.Value);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="source"></param>
        /// <param name="predicate"></param>
        /// <param name="errorSelector"></param>
        /// <typeparam name="TSource"></typeparam>
        /// <typeparam name="TError"></typeparam>
        /// <returns></returns>
        public static IResult<TSource, TError> FirstOrError<TSource, TError>(this IQueryable<TSource?> source,
            Expression<Func<TSource, bool>> predicate, Func<TError> errorSelector) where TSource : struct =>
            source.FirstOrDefault(predicate).ToResult(x => x.HasValue, _ => errorSelector()).Map(x => x.Value);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="source"></param>
        /// <param name="predicate"></param>
        /// <param name="errorSelector"></param>
        /// <typeparam name="TSource"></typeparam>
        /// <typeparam name="TError"></typeparam>
        /// <returns></returns>
        public static IResult<TSource, TError> SingleOrError<TSource, TError>(this IQueryable<TSource?> source,
            Expression<Func<TSource, bool>> predicate, Func<TError> errorSelector) where TSource : struct =>
            source.SingleOrDefault(predicate).ToResult(x => x.HasValue, _ => errorSelector()).Map(x => x.Value);
    }
}