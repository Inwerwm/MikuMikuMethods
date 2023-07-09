using MikuMikuMethods.Pmm.ElementState;
using MikuMikuMethods.Pmm.Frame;

namespace MikuMikuMethods.Pmm;

/// <summary>
/// ボーン
/// </summary>
public class PmmBone : IPmmModelElement
{
    /// <inheritdoc/>
    public string Name { get; }
    /// <summary>
    /// フレーム
    /// </summary>
    public List<PmmBoneFrame> Frames { get; private init; } = new();
    /// <summary>
    /// IKボーンか
    /// </summary>
    public bool IsIK { get; set; }
    /// <summary>
    /// 外部親を設定できるか
    /// </summary>
    public bool CanSetOutsideParent { get; set; }

    /// <summary>
    /// 現在の変形状態
    /// </summary>
    public PmmBoneState Current { get; private init; } = new();
    /// <summary>
    /// 編集画面で選択されているか
    /// </summary>
    public bool IsSelected { get; set; }
    /// <summary>
    /// 変形状態が確定されているか
    /// </summary>
    public bool IsCommitted { get; set; }

    internal PmmBone(string name)
    {
        Name = name;
    }

    /// <inheritdoc/>
    public override string ToString() => Name;

    /// <summary>
    /// ディープコピー
    /// </summary>
    /// <returns>複製</returns>
    public PmmBone DeepCopy() => new(this.Name)
    {
        Frames = this.Frames.Select(f => f.DeepCopy()).ToList(),
        IsIK = this.IsIK,
        CanSetOutsideParent = this.CanSetOutsideParent,
        Current = this.Current.DeepCopy(),
        IsSelected = this.IsSelected,
        IsCommitted = this.IsCommitted
    };

    /// <inheritdoc/>
    public object Clone() => DeepCopy();
}
