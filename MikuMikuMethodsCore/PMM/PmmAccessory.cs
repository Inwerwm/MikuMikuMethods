using MikuMikuMethods.Pmm.ElementState;
using MikuMikuMethods.Pmm.Frame;
using System.Collections.Generic;
using System.Linq;

namespace MikuMikuMethods.Pmm;

/// <summary>
/// PMM用アクセサリークラス
/// </summary>
public class PmmAccessory
{
    /// <summary>
    /// 名前
    /// </summary>
    public string Name { get; set; }
    /// <summary>
    /// ファイルパス
    /// </summary>
    public string Path { get; set; }

    /// <summary>
    /// 加算合成のOn/Off
    /// </summary>
    public bool EnableAlphaBlend { get; set; } = false;

    public List<PmmAccessoryFrame> Frames { get; } = new();
    public PmmAccessoryState Current { get; } = new();

    public override string ToString() => Name;
}
