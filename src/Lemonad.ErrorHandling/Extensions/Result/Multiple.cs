using System;
using System.Collections.Generic;
using System.Linq;
using Lemonad.ErrorHandling.Internal;
using Lemonad.ErrorHandling.Internal.Either;

namespace Lemonad.ErrorHandling.Extensions.Result {
    public static partial class Index {
        /// <summary>
        ///     Executes each function and saves all potential errors to a list which will be the <typeparamref name="TError" />.
        /// </summary>
        /// <param name="source"></param>
        /// <param name="validations">
        ///     A <see cref="IReadOnlyList{T}" /> containing <typeparamref name="TError" />.
        /// </param>
        public static IResult<T, IReadOnlyList<TError>> Multiple<T, TError>(
            this IResult<T, TError> source,
            params Func<IResult<T, TError>, IResult<T, TError>>[] validations
        ) {
            //TODO add an argument with IResult<T, TError>, IResult<T, TError>, in order to force atleast one argument to be provided except the source parameter.
            if (source is null) throw new ArgumentNullException(nameof(source));
            if (validations is null) throw new ArgumentNullException(nameof(validations));
            var either = EitherMethods.Multiple(
                source.Either,
                validations.Select(x =>
                    x.Compose(y => y.Either)(source)
                )
            );
            return either.HasValue
                ? ErrorHandling.Result.Value<T, IReadOnlyList<TError>>(either.Value)
                : ErrorHandling.Result.Error<T, IReadOnlyList<TError>>(either.Error);
        }
    }
}