using MikuMikuMethods.Pmm.ElementState;
using MikuMikuMethods.Pmm.Frame;

namespace MikuMikuMethods.Pmm;

/// <summary>
/// PMM用アクセサリークラス
/// </summary>
public class PmmAccessory
{
    /// <summary>
    /// 名前
    /// </summary>
    public string Name { get; }
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

    public PmmAccessory(string name, string path)
    {
        Name = name;
        Path = path;
    }

    public override string ToString() => Name;
}
