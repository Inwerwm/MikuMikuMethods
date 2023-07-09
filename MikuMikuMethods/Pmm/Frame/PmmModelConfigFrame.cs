using MikuMikuMethods.Pmm.ElementState;

namespace MikuMikuMethods.Pmm.Frame;

/// <summary>
/// モデル設定フレーム
/// </summary>
public class PmmModelConfigFrame : PmmModelConfigState, IPmmFrame
{
    /// <inheritdoc/>
    public int Frame { get; set; }
    /// <inheritdoc/>
    public bool IsSelected { get; set; }

    /// <summary>
    /// ディープコピー
    /// </summary>
    /// <returns>複製</returns>
    public new PmmModelConfigFrame DeepCopy() => new()
    {
        Frame = Frame,
        IsSelected = IsSelected,
        EnableIK = new(EnableIK),
        OutsideParent = OutsideParent.ToDictionary(p => p.Key, p => p.Value.DeepCopy()),
        Visible = Visible
    };

    /// <summary>
    /// モデル移行用ディープコピー
    /// </summary>
    /// <param name="boneMap">移行前ボーンと移行先ボーンの対応辞書</param>
    /// <returns>複製</returns>
    public new PmmModelConfigFrame DeepCopy(Dictionary<PmmBone, PmmBone> boneMap) => new()
    {
        Frame = Frame,
        IsSelected = IsSelected,
        EnableIK = EnableIK.Where(p => boneMap.ContainsKey(p.Key)).ToDictionary(p => boneMap[p.Key], p => p.Value),
        OutsideParent = OutsideParent.SelectKeyValue(boneMap, null).ToDictionary(p => p.Key, p => p.Value.DeepCopy()),
        Visible = Visible
    };

    IPmmFrame IPmmFrame.DeepCopy() => DeepCopy();

    /// <inheritdoc/>
    public override string ToString() => Frame.ToString();
}
