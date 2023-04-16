using MikuMikuMethods.Pmm.Frame;

namespace MikuMikuMethods.Pmm;

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

    public List<PmmSelfShadowFrame> Frames { get; private init; } = new();

    public PmmSelfShadow DeepCopy() => new()
    {
        EnableSelfShadow = EnableSelfShadow,
        ShadowRange = ShadowRange,
        Frames = Frames.Select(f => f.DeepCopy()).ToList()
    };

    public object Clone()
    {
        throw new NotImplementedException();
    }
}
