﻿namespace MikuMikuMethods.Vmd;

/// <summary>
/// フレームの種類
/// </summary>
public enum VmdFrameKind
{
    /// <summary>
    /// カメラ
    /// </summary>
    Camera,
    /// <summary>
    /// 照明
    /// </summary>
    Light,
    /// <summary>
    /// 影
    /// </summary>
    Shadow,
    /// <summary>
    /// 表示・IK
    /// </summary>
    Property,
    /// <summary>
    /// モーフ
    /// </summary>
    Morph,
    /// <summary>
    /// モーション
    /// </summary>
    Motion,
}
