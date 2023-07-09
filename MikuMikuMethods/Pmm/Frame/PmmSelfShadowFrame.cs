using MikuMikuMethods.Common;

namespace MikuMikuMethods.Pmm.Frame;

/// <summary>
/// セルフ影フレーム
/// </summary>
public partial class PmmSelfShadowFrame : IPmmFrame
{
    /// <inheritdoc/>
    public int Frame { get; set; }
    /// <inheritdoc/>
    public bool IsSelected { get; set; }
    /// <summary>
    /// 影モード
    /// </summary>
    public SelfShadow ShadowMode { get; set; }

    /// <summary>
    /// 影範囲
    /// </summary>
    public float ShadowRange { get; set; }

    /// <summary>
    /// ディープコピー
    /// </summary>
    /// <returns>複製</returns>
    public PmmSelfShadowFrame DeepCopy() => new()
    {
        Frame = Frame,
        IsSelected = IsSelected,
        ShadowRange = ShadowRange,
        ShadowMode = ShadowMode
    };

    IPmmFrame IPmmFrame.DeepCopy() => DeepCopy();

    /// <inheritdoc/>
    public override string ToString() => Frame.ToString();
}
