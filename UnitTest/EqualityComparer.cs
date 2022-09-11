using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UnitTest;
internal class EqualityComparer<T> : IEqualityComparer<T>
{
    Func<T, T, bool> _comparer;

    public EqualityComparer(Func<T, T, bool> comparer)
    {
        _comparer = comparer;
    }

    public bool Equals(T x, T y) => _comparer(x, y);

    public int GetHashCode([DisallowNull] T obj) => obj.GetHashCode();
}
