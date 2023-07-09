using MikuMikuMethods.Pmm.ElementState;
using MikuMikuMethods.Pmm.Frame;

namespace MikuMikuMethods.Pmm;

/// <summary>
/// 重力
/// </summary>
public class PmmPhysics : ICloneable
{
    /// <summary>
    /// 現在の重力の状態
    /// </summary>
    public PmmGravityState CurrentGravity { get; private init; } = new()
    {
        Noize = null,
        Acceleration = 9.8f,
        Direction = new(0, -1, 0)
    };

    /// <summary>
    /// 重力フレーム
    /// </summary>
    public List<PmmGravityFrame> GravityFrames { get; private init; } = new();

    /// <summary>
    /// 物理演算モード
    /// </summary>
    public enum PhysicsMode : byte
    {
        /// <summary>
        /// 演算しない
        /// </summary>
        Disable,
        /// <summary>
        /// 常に演算
        /// </summary>
        Always,
        /// <summary>
        /// オン/オフモード
        /// </summary>
        Switchable,
        /// <summary>
        /// トレースモード
        /// </summary>
        Trace
    }
    /// <summary>
    /// 物理演算モード
    /// </summary>
    public PhysicsMode CalculationMode { get; set; } = PhysicsMode.Always;
    /// <summary>
    /// 物理床のOn/Off
    /// </summary>
    public bool EnableGroundPhysics { get; set; } = true;

    /// <summary>
    /// ディープコピー
    /// </summary>
    /// <returns>複製</returns>
    public PmmPhysics DeepCopy() => new()
    {
        CurrentGravity = CurrentGravity.DeepCopy(),
        GravityFrames = GravityFrames.Select(f => f.DeepCopy()).ToList(),
        CalculationMode = CalculationMode,
        EnableGroundPhysics = EnableGroundPhysics,
    };

    /// <inheritdoc/>
    public object Clone() => DeepCopy();
}
