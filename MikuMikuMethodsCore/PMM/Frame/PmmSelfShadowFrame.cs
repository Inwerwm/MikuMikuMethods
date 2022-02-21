using MikuMikuMethods.Common;

namespace MikuMikuMethods.Pmm.Frame;

public partial class PmmSelfShadowFrame : IPmmFrame
{
    public int Frame { get; set; }
    public bool IsSelected { get; set; }
    /// <summary>
    /// 影モード
    /// </summary>
    public SelfShadow ShadowMode { get; set; }

    /// <summary>
    /// 影範囲
    /// </summary>
    public float ShadowRange { get; set; }

    public PmmSelfShadowFrame DeepCopy() => new()
    {
        Frame = Frame,
        IsSelected = IsSelected,
        ShadowRange = ShadowRange,
        ShadowMode = ShadowMode
    };

    IPmmFrame IPmmFrame.DeepCopy() => DeepCopy();

    public override string ToString() => Frame.ToString();
}
