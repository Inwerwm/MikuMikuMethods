namespace MikuMikuMethods.Common;

/// <summary>
/// 名前、インデックス、および説明からなるデータセクションを表すクラス。
/// </summary>
public record DataSection(string Name, int? Index, string Description)
{
    /// <summary>
    /// 整数を序数形式の文字列（1st, 2nd, 3rd, etc）に変換します。
    /// </summary>
    /// <param name="number">序数形式に変換する整数</param>
    /// <returns>序数形式の文字列</returns>
    internal static string GetOrdinal(int number) => number switch
    {
        int num when num % 100 is 11 or 12 or 13 => $"{number}th",
        int num when num % 10 is 1 => $"{number}st",
        int num when num % 10 is 2 => $"{number}nd",
        int num when num % 10 is 3 => $"{number}rd",
        _ => $"{number}th"
    };

    /// <inheritdoc/>
    public override string ToString()
    {
        return $"## {Name}{(Index is null ? "" : ":")}{Index} - {Description}";
    }
}
