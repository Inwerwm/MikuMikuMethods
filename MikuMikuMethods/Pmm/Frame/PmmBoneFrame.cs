using MikuMikuMethods.Pmm.ElementState;
using System.Collections.ObjectModel;

namespace MikuMikuMethods.Pmm.Frame;

/// <summary>
/// ボーンフレーム
/// </summary>
public class PmmBoneFrame : PmmBoneState, IPmmFrame
{
    /// <inheritdoc/>
    public int Frame { get; set; } = 0;
    /// <inheritdoc/>
    public bool IsSelected { get; set; } = false;

    /// <summary>
    /// 補間曲線
    /// </summary>
    public ReadOnlyDictionary<InterpolationItem, InterpolationCurve> InterpolationCurves { get; internal init; } = InterpolationCurve.CreateBoneCurves();

    /// <summary>
    /// ディープコピー
    /// </summary>
    /// <returns>複製</returns>
    public new PmmBoneFrame DeepCopy() => new()
    {
        Frame = Frame,
        IsSelected = IsSelected,
        EnablePhysic = EnablePhysic,
        Rotation = Rotation,
        Movement = Movement,
        InterpolationCurves = InterpolationCurve.Clone(InterpolationCurves),
    };

    IPmmFrame IPmmFrame.DeepCopy() => DeepCopy();

    /// <inheritdoc/>
    public override string ToString() => Frame.ToString();
}
