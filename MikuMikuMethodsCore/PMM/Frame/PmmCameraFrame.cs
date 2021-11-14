using MikuMikuMethods.Pmm.ElementState;
using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace MikuMikuMethods.Pmm.Frame;

public class PmmCameraFrame : PmmCameraState, IPmmFrame
{
    public int Frame { get; set; } = 0;
    public bool IsSelected { get; set; } = false;

    /// <summary>
    /// カメラの距離
    /// </summary>
    public float Distance { get; set; } = 45;
    /// <summary>
    /// 補間曲線
    /// </summary>
    public ReadOnlyDictionary<InterpolationItem, InterpolationCurve> InterpolationCurves { get; private init; } = new(new Dictionary<InterpolationItem, InterpolationCurve>()
        {
            { InterpolationItem.XPosition, new() },
            { InterpolationItem.YPosition, new() },
            { InterpolationItem.ZPosition, new() },
            { InterpolationItem.Rotation, new() },
            { InterpolationItem.Distance, new() },
            { InterpolationItem.ViewAngle, new() },
        });

    public new PmmCameraFrame DeepCopy() => new()
    {
        Frame = Frame,
        IsSelected = IsSelected,
        Distance = Distance,
        EnablePerspective = EnablePerspective,
        EyePosition = EyePosition,
        FollowingBone = FollowingBone,
        FollowingModel = FollowingModel,
        Rotation = Rotation,
        TargetPosition = TargetPosition,
        ViewAngle = ViewAngle,
        InterpolationCurves = new(new Dictionary<InterpolationItem, InterpolationCurve>()
            {
                { InterpolationItem.XPosition, InterpolationCurves[InterpolationItem.XPosition].Clone() as InterpolationCurve },
                { InterpolationItem.YPosition, InterpolationCurves[InterpolationItem.YPosition].Clone() as InterpolationCurve },
                { InterpolationItem.ZPosition, InterpolationCurves[InterpolationItem.ZPosition].Clone() as InterpolationCurve },
                { InterpolationItem.Rotation, InterpolationCurves[InterpolationItem.Rotation].Clone() as InterpolationCurve },
                { InterpolationItem.Distance, InterpolationCurves[InterpolationItem.Distance].Clone() as InterpolationCurve },
                { InterpolationItem.ViewAngle, InterpolationCurves[InterpolationItem.ViewAngle].Clone() as InterpolationCurve },
            })
    };

    IPmmFrame IPmmFrame.DeepCopy() => DeepCopy();

    public override string ToString() => Frame.ToString();
}
