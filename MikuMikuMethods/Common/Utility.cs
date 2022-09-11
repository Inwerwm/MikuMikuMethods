namespace MikuMikuMethods;

/// <summary>
/// 便利メソッド集
/// </summary>
internal static class Utility
{
    /// <summary>
    /// 引数の中身を入れ替える
    /// </summary>
    public static void Swap<T>(ref T first, ref T second)
    {
        T tmp = first;
        first = second;
        second = tmp;
    }

    public static void AddOrOverWrite<T>(this IList<T> ts, T item, Func<T, T, bool> equalityComparer)
    {
        if (ts.FirstOrDefault(t => equalityComparer(t, item)) is { } existingFrame)
        {
            var index = ts.IndexOf(existingFrame);
            ts.Remove(existingFrame);
            ts.Insert(index, item);
        }
        else
        {
            ts.Add(item);
        }
    }
}
