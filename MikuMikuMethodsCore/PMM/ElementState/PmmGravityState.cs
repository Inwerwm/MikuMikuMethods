using System.Numerics;

namespace MikuMikuMethods.Pmm.ElementState;

public class PmmGravityState
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

    public PmmGravityState DeepCopy() => new()
    {
        Noize = Noize,
        Acceleration = Acceleration,
        Direction = Direction
    };
}
