using System.Numerics;

namespace MikuMikuMethods.Pmm.ElementState;

/// <summary>
/// 重力の状態
/// </summary>
public class PmmGravityState : ICloneable
{

    /// <summary>
    /// ノイズ量
    /// nullならなし
    /// </summary>
    public int? Noize { get; set; } = null;
    /// <summary>
    /// 加速度
    /// </summary>
    public float Acceleration { get; set; } = 9.8f;
    /// <summary>
    /// 方向
    /// </summary>
    public Vector3 Direction { get; set; } = new(0, -1, 0);

    /// <summary>
    /// ディープコピー
    /// </summary>
    /// <returns>複製</returns>
    public PmmGravityState DeepCopy() => new()
    {
        Noize = Noize,
        Acceleration = Acceleration,
        Direction = Direction
    };

    /// <inheritdoc/>
    public object Clone() => DeepCopy();
}
