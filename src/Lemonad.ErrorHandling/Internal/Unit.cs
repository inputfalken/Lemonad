using System;
using System.Diagnostics.Contracts;

namespace Lemonad.ErrorHandling.Internal {
    /// <summary>
    ///     Represents void but in a returnable fashion.
    /// </summary>
    [Serializable]
    internal readonly struct Unit : IEquatable<Unit>, IComparable<Unit> {
        public static readonly Func<Unit> Selector = () => Default;
        public static readonly Func<Unit, Unit> AlternativeSelector = x => x;
        public static readonly Unit Default = new Unit();

        public override int GetHashCode() => 0;

        public override bool Equals(object obj) => obj is Unit;

        public override string ToString() => "()";

        public bool Equals(Unit other) => true;

        public static bool operator ==(Unit left, Unit right) => true;

        public static bool operator !=(Unit left, Unit right) => false;

        public static bool operator >(Unit left, Unit right) => false;

        public static bool operator >=(Unit left, Unit right) => true;

        public static bool operator <(Unit left, Unit right) => false;

        public static bool operator <=(Unit left, Unit right) => true;

        /// <summary>
        ///     Is Always equal
        /// </summary>
        public int CompareTo(Unit other) => 0;
    }
}
