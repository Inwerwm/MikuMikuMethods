﻿using System.Numerics;

namespace MikuMikuMethods.Pmm.ElementState;

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