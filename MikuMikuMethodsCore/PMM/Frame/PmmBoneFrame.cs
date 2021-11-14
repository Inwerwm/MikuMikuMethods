using MikuMikuMethods.Pmm.ElementState;
using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace MikuMikuMethods.Pmm.Frame;

public class PmmBoneFrame : PmmBoneState, IPmmFrame
{
    public int Frame { get; set; } = 0;
    public bool IsSelected { get; set; } = false;

    /// <summary>
    /// 補間曲線
    /// </summary>
    public ReadOnlyDictionary<InterpolationItem, InterpolationCurve> InterpolationCurves { get; private init; } = new(new Dictionary<InterpolationItem, InterpolationCurve>()
        {
            { InterpolationItem.XPosition, new() },
            { InterpolationItem.YPosition, new() },
            { InterpolationItem.ZPosition, new() },
            { InterpolationItem.Rotation, new() },
        });

    public new PmmBoneFrame DeepCopy() => new()
    {
        Frame = Frame,
        IsSelected = IsSelected,
        EnablePhysic = EnablePhysic,
        Rotation = Rotation,
        Movement = Movement,
        InterpolationCurves = new(new Dictionary<InterpolationItem, InterpolationCurve>()
            {
                { InterpolationItem.XPosition, (InterpolationCurve)InterpolationCurves[InterpolationItem.XPosition].Clone() },
                { InterpolationItem.YPosition, (InterpolationCurve)InterpolationCurves[InterpolationItem.YPosition].Clone() },
                { InterpolationItem.ZPosition, (InterpolationCurve)InterpolationCurves[InterpolationItem.ZPosition].Clone() },
                { InterpolationItem.Rotation, (InterpolationCurve)InterpolationCurves[InterpolationItem.Rotation].Clone() },
            })
    };

    IPmmFrame IPmmFrame.DeepCopy() => DeepCopy();

    public override string ToString() => Frame.ToString();
}
