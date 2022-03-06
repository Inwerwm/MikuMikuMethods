using MikuMikuMethods.Pmm.Frame;

namespace MikuMikuMethods.Pmm;

public class PmmSelfShadow
{
    /// <summary>
    /// セルフ影のOn/Off
    /// </summary>
    public bool EnableSelfShadow { get; set; } = true;
    /// <summary>
    /// 影範囲
    /// </summary>
    public float ShadowRange { get; set; } = 8875;

    public List<PmmSelfShadowFrame> Frames { get; } = new();
}
