using System.Diagnostics.CodeAnalysis;

namespace UnitTest;
internal class EqualityComparer<T> : IEqualityComparer<T>
{
    private readonly Func<T, T, bool> _comparer;

    public EqualityComparer(Func<T, T, bool> comparer)
    {
        _comparer = comparer;
    }

    public bool Equals(T x, T y) => _comparer(x, y);

    public int GetHashCode([DisallowNull] T obj) => obj.GetHashCode();
}
