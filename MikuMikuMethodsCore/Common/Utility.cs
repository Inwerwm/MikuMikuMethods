namespace MikuMikuMethods;

/// <summary>
/// 便利メソッド集
/// </summary>
public static class Utility
{
    /// <summary>
    /// queryを新しい連番で整列する
    /// </summary>
    /// <param name="map">もとの連番と新しい連番の変換マップリスト</param>
    /// <param name="query">変換対象リスト</param>
    /// <returns>変換されたリスト</returns>
    public static List<T> OrderByMap<T>(List<int> map, List<T> query) =>
        query.Zip(map, (t, i) => (Value: t, Index: i)).OrderBy(pair => pair.Index).Select(pair => pair.Value).ToList();

    /// <summary>
    /// 引数の中身を入れ替える
    /// </summary>
    public static void Swap<T>(ref T first, ref T second)
    {
        T tmp = first;
        first = second;
        second = tmp;
    }
}
