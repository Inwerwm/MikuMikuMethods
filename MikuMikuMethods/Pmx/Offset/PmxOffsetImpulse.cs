﻿using System.Numerics;

namespace MikuMikuMethods.Pmx;

/// <summary>
/// インパルスモーフのオフセット
/// </summary>
public class PmxOffsetImpulse : IPmxOffset
{
    /// <summary>
    /// 対象剛体
    /// </summary>
    public PmxBody Target { get; set; }
    /// <summary>
    /// ローカルフラグ
    /// </summary>
    public bool IsLocal { get; set; }
    /// <summary>
    /// 移動速度
    /// </summary>
    public Vector3 MovingSpead { get; set; }
    /// <summary>
    /// 回転トルク
    /// </summary>
    public Vector3 RotationTorque { get; set; }

    public override string ToString() => $"{Target.Name} : {{{MovingSpead} - {RotationTorque}}}";

    public PmxOffsetImpulse(PmxBody target, bool isLocal = default, Vector3 movingSpead = default, Vector3 rotationTorque = default)
    {
        Target = target;
        IsLocal = isLocal;
        MovingSpead = movingSpead;
        RotationTorque = rotationTorque;
    }

    internal PmxOffsetImpulse()
    {
        Target = null!;
    }
}