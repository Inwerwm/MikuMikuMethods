namespace MikuMikuMethods.Extension;

internal static class ListControl
{
    public static void Move<T>(this IList<T> list, int index, T item) where T : class
    {
        if (list == null) return;
        if (!list.Any()) return;
        if (list[index] == item) return;

        if (list.Remove(item))
            list.Insert(index, item);
    }

    public static IEnumerable<TResult> Select<TSource, TResult>(this IEnumerable<TSource> source, Action<TSource> onSelected, Func<TSource, TResult> selector)
    {
        return source.Select(s =>
        {
            onSelected(s);
            return selector(s);
        });
    }
}
