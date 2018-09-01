using System;
using System.Diagnostics.Contracts;
using System.Threading.Tasks;

namespace Lemonad.ErrorHandling {
    public class Prospect<T> {
        private readonly Outcome<T, Unit> _outcome;

        public Prospect(Outcome<T, Unit> outcome) => _outcome = outcome;

        public Task<bool> HasValue => _outcome.HasValue;

        [Pure]
        public Prospect<TResult> Map<TResult>(Func<T, TResult> selector) =>
            new Prospect<TResult>(_outcome.Map(selector));
    }
}