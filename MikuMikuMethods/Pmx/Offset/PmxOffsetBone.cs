﻿using System.Numerics;

namespace MikuMikuMethods.Pmx;

/// <summary>
/// ボーンモーフのオフセット
/// </summary>
public class PmxOffsetBone : IPmxOffset
{
    /// <summary>
    /// 対象ボーン
    /// </summary>
    public PmxBone Target { get; set; }

    /// <summary>
    /// 移動量
    /// </summary>
    public Vector3 Offset { get; set; }
    /// <summary>
    /// 回転量
    /// </summary>
    public Quaternion Rotate { get; set; }

    /// <inheritdoc/>
    public override string ToString() => $"{Target.Name} : {{{Offset} - {Rotate}}}";

    /// <summary>
    /// コンストラクタ
    /// </summary>
    /// <param name="target">対象ボーン</param>
    /// <param name="offset">移動量</param>
    /// <param name="rotate">回転量</param>
    public PmxOffsetBone(PmxBone target, Vector3 offset = default, Quaternion rotate = default)
    {
        Target = target;
        Offset = offset;
        Rotate = rotate;
    }
}
