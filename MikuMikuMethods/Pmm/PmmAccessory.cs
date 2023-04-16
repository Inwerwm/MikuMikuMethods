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

    public List<PmmAccessoryFrame> Frames { get; private init; } = new();
    public PmmAccessoryState Current { get; private init; } = new();

    public PmmAccessory(string name, string path)
    {
        Name = name;
        Path = path;
    }

    public PmmAccessory DeepCopy(Dictionary<PmmModel, PmmModel> modelMap, Dictionary<PmmBone, PmmBone> boneMap) => new PmmAccessory(Name, Path)
    {
        Frames = Frames.Select(f => f.DeepCopy(modelMap.GetOrDefault(f.ParentModel), boneMap.GetOrDefault(f.ParentBone))).ToList(),
        Current = Current.DeepCopy(modelMap.GetOrDefault(Current.ParentModel), boneMap.GetOrDefault(Current.ParentBone))
    };

    public override string ToString() => Name;
}
