using MikuMikuMethods.Pmm.Frame;

namespace MikuMikuMethods.Pmm;

/// <summary>
/// セルフ影
/// </summary>
public class PmmSelfShadow : ICloneable
{
    /// <summary>
    /// セルフ影のOn/Off
    /// </summary>
    public bool EnableSelfShadow { get; set; } = true;
    /// <summary>
    /// 影範囲
    /// </summary>
    public float ShadowRange { get; set; } = 8875;

    /// <summary>
    /// キーフレーム
    /// </summary>
    public List<PmmSelfShadowFrame> Frames { get; private init; } = new();

    /// <summary>
    /// ディープコピー
    /// </summary>
    /// <returns>複製</returns>
    public PmmSelfShadow DeepCopy() => new()
    {
        EnableSelfShadow = EnableSelfShadow,
        ShadowRange = ShadowRange,
        Frames = Frames.Select(f => f.DeepCopy()).ToList()
    };

    /// <inheritdoc/>
    public object Clone()
    {
        throw new NotImplementedException();
    }
}
