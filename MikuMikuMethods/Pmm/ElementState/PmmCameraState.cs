using System.Numerics;

namespace MikuMikuMethods.Pmm.ElementState;

/// <summary>
/// カメラの状態
/// </summary>
public class PmmCameraState : ICloneable
{
    /// <summary>
    /// カメラ位置
    /// </summary>
    public Vector3 EyePosition { get; set; }
    /// <summary>
    /// カメラ中心の位置
    /// </summary>
    public Vector3 TargetPosition { get; set; }
    /// <summary>
    /// カメラ回転
    /// </summary>
    public Vector3 Rotation { get; set; }
    /// <summary>
    /// パースのOn/Off
    /// </summary>
    public bool DisablePerspective { get; set; }
    /// <summary>
    /// 視点追従先モデル
    /// </summary>
    public PmmModel? FollowingModel { get; set; }
    /// <summary>
    /// 視点追従先ボーン
    /// </summary>
    public PmmBone? FollowingBone { get; set; }
    /// <summary>
    /// 視野角
    /// </summary>
    public int ViewAngle { get; set; } = 30;

    /// <summary>
    /// ディープコピー
    /// </summary>
    /// <returns>複製</returns>
    public PmmCameraState DeepCopy() => new()
    {
        EyePosition = EyePosition,
        TargetPosition = TargetPosition,
        Rotation = Rotation,
        DisablePerspective = DisablePerspective,
        FollowingBone = FollowingBone,
        FollowingModel = FollowingModel,
        ViewAngle = ViewAngle
    };


    /// <summary>
    /// PMM移行用ディープコピー
    /// </summary>
    /// <param name="followingModel">移行先の対応する参照親モデル</param>
    /// <param name="followingBone">移行先の対応する参照親ボーン</param>
    /// <returns>複製</returns>
    public PmmCameraState DeepCopy(PmmModel? followingModel, PmmBone? followingBone) => new()
    {
        EyePosition = EyePosition,
        TargetPosition = TargetPosition,
        Rotation = Rotation,
        DisablePerspective = DisablePerspective,
        FollowingBone = followingBone,
        FollowingModel = followingModel,
        ViewAngle = ViewAngle
    };

    /// <inheritdoc/>
    public object Clone() => DeepCopy();
}
