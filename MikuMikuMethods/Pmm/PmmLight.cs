using MikuMikuMethods.Pmm.ElementState;
using MikuMikuMethods.Pmm.Frame;

namespace MikuMikuMethods.Pmm;

/// <summary>
/// 照明
/// </summary>
public class PmmLight : ICloneable
{
    /// <inheritdoc/>
    public List<PmmLightFrame> Frames { get; private init; } = new();
    /// <summary>
    /// 現在の状態
    /// </summary>
    public PmmLightState Current { get; private init; } = new();

    /// <inheritdoc/>
    public object Clone() => DeepCopy();

    /// <summary>
    /// ディープコピー
    /// </summary>
    /// <returns>複製</returns>
    public PmmLight DeepCopy() => new()
    {
        Frames = this.Frames.Select(f => f.DeepCopy()).ToList(),
        Current = this.Current.DeepCopy()
    };
}
