using MikuMikuMethods.Pmm.ElementState;
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
    public ReadOnlyDictionary<InterpolationItem, InterpolationCurve> InterpolationCurves { get; private init; } = InterpolationCurve.CreateCameraCurves();

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
        InterpolationCurves = InterpolationCurve.Clone(InterpolationCurves),
    };

    IPmmFrame IPmmFrame.DeepCopy() => DeepCopy();

    public override string ToString() => Frame.ToString();
}
