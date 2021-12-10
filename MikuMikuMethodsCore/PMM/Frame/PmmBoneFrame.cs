using MikuMikuMethods.Pmm.ElementState;
using System.Collections.ObjectModel;

namespace MikuMikuMethods.Pmm.Frame;

public class PmmBoneFrame : PmmBoneState, IPmmFrame
{
    public int Frame { get; set; } = 0;
    public bool IsSelected { get; set; } = false;

    /// <summary>
    /// 補間曲線
    /// </summary>
    public ReadOnlyDictionary<InterpolationItem, InterpolationCurve> InterpolationCurves { get; private init; } = InterpolationCurve.CreateBoneCurves();

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

    public override string ToString() => Frame.ToString();
}
