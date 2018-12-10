namespace Lemonad.ErrorHandling.EnumerableExtensions {
    /// <summary>
    /// Represents the cases in the SingleOrError extension method for <see cref="System.Linq.Enumerable"/>.
    /// </summary>
    public enum SingleOrErrorCase {
        NoElement,
        ManyElements
    }
}