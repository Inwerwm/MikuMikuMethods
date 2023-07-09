using MikuMikuMethods.Pmm.ElementState;
using System.Collections.ObjectModel;

namespace MikuMikuMethods.Pmm.Frame;

/// <summary>
/// カメラフレーム
/// </summary>
public class PmmCameraFrame : PmmCameraState, IPmmFrame
{
    /// <inheritdoc/>
    public int Frame { get; set; } = 0;
    /// <inheritdoc/>
    public bool IsSelected { get; set; } = false;

    /// <summary>
    /// カメラの距離
    /// </summary>
    public float Distance { get; set; } = 45;
    /// <summary>
    /// 補間曲線
    /// </summary>
    public ReadOnlyDictionary<InterpolationItem, InterpolationCurve> InterpolationCurves { get; internal init; } = InterpolationCurve.CreateCameraCurves();

    /// <summary>
    /// ディープコピー
    /// </summary>
    /// <returns>複製</returns>
    public new PmmCameraFrame DeepCopy() => new()
    {
        Frame = Frame,
        IsSelected = IsSelected,
        Distance = Distance,
        DisablePerspective = DisablePerspective,
        EyePosition = EyePosition,
        FollowingBone = FollowingBone,
        FollowingModel = FollowingModel,
        Rotation = Rotation,
        TargetPosition = TargetPosition,
        ViewAngle = ViewAngle,
        InterpolationCurves = InterpolationCurve.Clone(InterpolationCurves),
    };

    /// <summary>
    /// PMM移行用ディープコピー
    /// </summary>
    /// <param name="followingModel">移行対象側の対応する参照親モデル</param>
    /// <param name="followingBone">移行対象側の対応する参照親ボーン</param>
    /// <returns>複製</returns>
    public new PmmCameraFrame DeepCopy(PmmModel? followingModel, PmmBone? followingBone) => new()
    {
        Frame = Frame,
        IsSelected = IsSelected,
        Distance = Distance,
        DisablePerspective = DisablePerspective,
        EyePosition = EyePosition,
        FollowingBone = followingBone,
        FollowingModel = followingModel,
        Rotation = Rotation,
        TargetPosition = TargetPosition,
        ViewAngle = ViewAngle,
        InterpolationCurves = InterpolationCurve.Clone(InterpolationCurves),
    };

    IPmmFrame IPmmFrame.DeepCopy() => DeepCopy();

    /// <inheritdoc/>
    public override string ToString() => Frame.ToString();
}
