using System.Numerics;

namespace MikuMikuMethods.Pmm.ElementState;

/// <summary>
/// ボーンの状態
/// </summary>
public class PmmBoneState : ICloneable
{
    /// <summary>
    /// 移動量
    /// </summary>
    public Vector3 Movement { get; set; }

    /// <summary>
    /// 回転量
    /// </summary>
    public Quaternion Rotation { get; set; } = Quaternion.Identity;

    /// <summary>
    /// 物理が有効か
    /// </summary>
    public bool EnablePhysic { get; set; } = true;

    /// <summary>
    /// ディープコピー
    /// </summary>
    /// <returns>複製</returns>
    public PmmBoneState DeepCopy() => new()
    {
        Movement = Movement,
        Rotation = Rotation,
        EnablePhysic = EnablePhysic
    };

    /// <inheritdoc/>
    public object Clone() => DeepCopy();
}
