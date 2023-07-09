namespace MikuMikuMethods;

/// <summary>
/// 便利メソッド集
/// </summary>
internal static class Utility
{
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

    public static TValue? GetOrDefault<TKey, TValue>(this IDictionary<TKey, TValue> dictionary, TKey? key) => key is not null && dictionary.TryGetValue(key, out var value) ? value : default;
    public static Dictionary<TKey, TValue> SelectKeyValue<TKey, TValue>(this IDictionary<TKey, TValue> dictionary, IDictionary<TKey, TKey>? keyMap, IDictionary<TValue, TValue>? valueMap) where TKey : notnull => dictionary
            .Where(p => keyMap is null || keyMap.ContainsKey(p.Key))
            .Where(p => valueMap is null || p.Value is not null && valueMap.ContainsKey(p.Value))
            .ToDictionary(
                p => keyMap is null ? p.Key : keyMap[p.Key],
                p => valueMap is null ? p.Value : valueMap[p.Value]
            );
}
