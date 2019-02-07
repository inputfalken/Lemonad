using System;
using System.Threading.Tasks;
using Lemonad.ErrorHandling.Exceptions;
using Lemonad.ErrorHandling.Extensions.AsyncResult;
using Lemonad.ErrorHandling.Extensions.Maybe;
using Lemonad.ErrorHandling.Extensions.Result;
using Index = Lemonad.ErrorHandling.Extensions.Maybe.Index;

namespace Lemonad.ErrorHandling.Internal {
    internal readonly struct Maybe<T> : IMaybe<T> {
        internal static IMaybe<T> None { get; } = new Maybe<T>(default, false);
        internal static IMaybe<T> Create(in T value) => new Maybe<T>(in value, true);

        public bool HasValue => _result.Either.HasValue;
        public T Value => _result.Either.Value;

        private readonly IResult<T, Unit> _result;

        private Maybe(in T value, bool hasValue) {
            // Is needed to not expose the private _result exception.
            if (hasValue && value.IsNull())
                throw new InvalidMaybeStateException(
                    $"{nameof(IMaybe<T>)} property \"{nameof(Value)}\" cannot be null when property \"{nameof(HasValue)}\" is expected to be true."
                );
            _result = hasValue ? Result.Value<T, Unit>(value) : Result.Error<T, Unit>(Unit.Default);
        }

        public override string ToString() =>
            $"{(HasValue ? "Some" : "None")} ==> {typeof(Maybe<T>).ToHumanString()}{StringFunctions.PrettyTypeString(Value)}";

        public IAsyncMaybe<TResult> MapAsync<TResult>(Func<T, Task<TResult>> selector) =>
            _result.MapAsync(selector).ToAsyncMaybe();

        public void Match(Action<T> someAction, Action noneAction) {
            if (noneAction is null) throw new ArgumentNullException(nameof(noneAction));
            _result.Match(someAction, _ => noneAction());
        }

        public IMaybe<T> DoWith(Action<T> action) {
            if (action is null) throw new ArgumentNullException(nameof(action));
            return _result.DoWith(action).ToMaybe();
        }

        public IAsyncMaybe<T> DoWithAsync(Func<T, Task> action) {
            if (action is null) throw new ArgumentNullException(nameof(action));
            return _result.DoWithAsync(action).ToAsyncMaybe();
        }

        public IMaybe<T> Do(Action action) {
            if (action is null) throw new ArgumentNullException(nameof(action));
            return _result.Do(action).ToMaybe();
        }

        public TResult Match<TResult>(Func<T, TResult> someSelector, Func<TResult> noneSelector) {
            if (noneSelector is null) throw new ArgumentNullException(nameof(noneSelector));
            return _result.Match(someSelector, _ => noneSelector());
        }

        public IAsyncMaybe<T> DoAsync(Func<Task> func) => _result.DoAsync(func).ToAsyncMaybe();

        public IAsyncMaybe<T> IsNoneWhenAsync(Func<T, Task<bool>> predicate) =>
            _result.IsErrorWhenAsync(predicate, _ => Unit.Default).ToAsyncMaybe();

        public IMaybe<TResult> Map<TResult>(Func<T, TResult> selector) => _result.Map(selector).ToMaybe();

        public IMaybe<T> Filter(Func<T, bool> predicate) => _result.Filter(predicate, arg => Unit.Default).ToMaybe();

        public IAsyncMaybe<T> FilterAsync(Func<T, Task<bool>> predicate) => predicate is null
            ? throw new ArgumentNullException(nameof(predicate))
            : _result.FilterAsync(predicate, _ => Unit.Default).ToAsyncMaybe();

        public IMaybe<TResult> FlatMap<TResult>(Func<T, IMaybe<TResult>> selector) => selector is null
            ? throw new ArgumentNullException(nameof(selector))
            : _result
                .FlatMap(x => Index.ToResult(selector(x), Unit.Selector)).ToMaybe();

        public IAsyncMaybe<TResult> FlatMapAsync<TResult>(Func<T, IAsyncMaybe<TResult>> selector)
            => _result.FlatMapAsync(
                selector.Compose(y => Extensions.AsyncMaybe.Index.ToAsyncResult(y, Unit.Selector))
            ).ToAsyncMaybe();

        public IMaybe<TResult> FlatMap<TFlatMap, TResult>(
            Func<T, IMaybe<TFlatMap>> selector,
            Func<T, TFlatMap, TResult> resultSelector
        ) {
            if (selector is null) throw new ArgumentNullException(nameof(selector));
            if (resultSelector is null) throw new ArgumentNullException(nameof(resultSelector));
            return _result.FlatMap(
                    x => selector.Compose(y => y.ToResult(Unit.Selector))(x).Map(y => resultSelector(x, y))
                )
                .ToMaybe();
        }

        public IAsyncMaybe<TResult> FlatMapAsync<TSelector, TResult>(
            Func<T, IAsyncMaybe<TSelector>> selector,
            Func<T, TSelector, TResult> resultSelector
        ) => _result
            .FlatMapAsync(
                selector.Compose(y => Extensions.AsyncMaybe.Index.ToAsyncResult(y, Unit.Selector)),
                resultSelector
            ).ToAsyncMaybe();

        public IAsyncMaybe<TResult> FlatMapAsync<TSelector, TResult>(Func<T, Task<TSelector?>> selector,
            Func<T, TSelector, TResult> resultSelector) where TSelector : struct
            => _result.FlatMapAsync(selector, resultSelector, Unit.Selector).ToAsyncMaybe();

        public IMaybe<TResult> FlatMap<TResult>(Func<T, TResult?> selector) where TResult : struct =>
            selector is null
                ? throw new ArgumentNullException(nameof(selector))
                : _result.FlatMap(selector, Unit.Selector).ToMaybe();

        public IAsyncMaybe<TResult> FlatMapAsync<TResult>(Func<T, Task<TResult?>> selector) where TResult : struct
            => _result.FlatMapAsync(selector, Unit.Selector).ToAsyncMaybe();

        public IAsyncMaybe<T> FlattenAsync<TResult>(Func<T, IAsyncMaybe<TResult>> selector)
            => _result.FlattenAsync(
                selector.Compose(x => Extensions.AsyncMaybe.Index.ToAsyncResult(x, Unit.Selector))
            ).ToAsyncMaybe();

        public IMaybe<T> IsNoneWhen(Func<T, bool> predicate) => predicate is null
            ? throw new ArgumentNullException(nameof(predicate))
            : _result.IsErrorWhen(predicate, _ => Unit.Default).ToMaybe();

        public IMaybe<T> Flatten<TResult>(Func<T, IMaybe<TResult>> selector) => selector is null
            ? throw new ArgumentNullException(nameof(selector))
            : _result.Flatten(x =>
                Index.ToResult(selector(x), Unit.Selector)).ToMaybe();

        public IMaybe<TResult> FlatMap<TSelector, TResult>(
            Func<T, TSelector?> selector,
            Func<T, TSelector, TResult> resultSelector
        ) where TSelector : struct {
            if (selector is null) throw new ArgumentNullException(nameof(selector));
            if (resultSelector is null) throw new ArgumentNullException(nameof(resultSelector));
            return _result.FlatMap(selector, resultSelector, Unit.Selector).ToMaybe();
        }
    }
}