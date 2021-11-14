﻿using System.Numerics;

namespace MikuMikuMethods.Pmm.ElementState;

public class PmmBoneState
{
    /// <summary>
    /// 移動量
    /// </summary>
    public Vector3 Movement { get; set; }

    /// <summary>
    /// 回転量
    /// </summary>
    public Quaternion Rotation { get; set; }

    /// <summary>
    /// 物理が有効か
    /// </summary>
    public bool EnablePhysic { get; set; }

    public PmmBoneState DeepCopy() => new()
    {
        Movement = Movement,
        Rotation = Rotation,
        EnablePhysic = EnablePhysic
    };
}
