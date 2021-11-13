using MikuMikuMethods.Pmm.ElementState;
using MikuMikuMethods.Pmm.Frame;
using System.Collections.Generic;

namespace MikuMikuMethods.Pmm;

public class PmmBone : IPmmModelElement
{
    public string Name { get; }
    public List<PmmBoneFrame> Frames { get; } = new();
    /// <summary>
    /// IKボーンか
    /// </summary>
    public bool IsIK { get; set; }
    /// <summary>
    /// 外部親になれるか
    /// </summary>
    public bool CanBecomeOuterParent { get; set; }

    /// <summary>
    /// 現在の変形状態
    /// </summary>
    public PmmBoneState Current { get; } = new();
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

    public override string ToString() => Name;
}
