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

    /// <summary>
    /// フレーム
    /// </summary>
    public List<PmmAccessoryFrame> Frames { get; private init; } = new();
    /// <summary>
    /// 現在の編集状態
    /// </summary>
    public PmmAccessoryState Current { get; private init; } = new();

    /// <summary>
    /// コンストラクタ
    /// </summary>
    /// <param name="name">名前</param>
    /// <param name="path">ファイルパス</param>
    public PmmAccessory(string name, string path)
    {
        Name = name;
        Path = path;
    }

    /// <summary>
    /// PMM移行用ディープコピー
    /// </summary>
    /// <param name="modelMap">移行前モデルと移行先モデルの対応辞書</param>
    /// <param name="boneMap">移行前ボーンと移行先ボーンの対応辞書</param>
    /// <returns>複製</returns>
    public PmmAccessory DeepCopy(Dictionary<PmmModel, PmmModel> modelMap, Dictionary<PmmBone, PmmBone> boneMap) => new PmmAccessory(Name, Path)
    {
        Frames = Frames.Select(f => f.DeepCopy(modelMap.GetOrDefault(f.ParentModel), boneMap.GetOrDefault(f.ParentBone))).ToList(),
        Current = Current.DeepCopy(modelMap.GetOrDefault(Current.ParentModel), boneMap.GetOrDefault(Current.ParentBone))
    };

    /// <inheritdoc/>
    public override string ToString() => Name;
}
