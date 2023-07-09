using System.Collections.Immutable;

namespace MikuMikuMethods.Pmm;

/// <summary>
/// 表示枠
/// </summary>
public class PmmNode
{
    /// <summary>
    /// 名前
    /// </summary>
    public string Name { get; internal set; }
    /// <summary>
    /// 展開されているか
    /// </summary>
    public bool DoesOpen { get; set; } = false;
    /// <summary>
    /// 所属しているモデル要素
    /// </summary>
    public ImmutableArray<IPmmModelElement> Elements { get; init; } = new();

    /// <inheritdoc/>
    public override string ToString() => Name;

    internal PmmNode()
    {
        Name = "";
    }

    /// <summary>
    /// PMM移行用ディープコピー
    /// </summary>
    /// <param name="elementMap">移行前モデル要素と移行先モデル要素の対応辞書</param>
    /// <returns>複製</returns>
    public PmmNode DeepCopy(Dictionary<IPmmModelElement, IPmmModelElement> elementMap) => new()
    {
        Name = Name,
        DoesOpen = DoesOpen,
        Elements = ImmutableArray.Create(Elements.Select(e => elementMap.GetOrDefault(e)).Where(e => e is not null).Cast<IPmmModelElement>().ToArray())
    };
}
