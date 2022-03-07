using MikuMikuMethods.Common;

namespace MikuMikuMethods.Vmd;

/// <summary>
/// 影フレーム
/// </summary>
public class VmdShadowFrame : VmdCameraTypeFrame
{
    /// <summary>
    /// フレームの種類
    /// </summary>
    public override VmdFrameKind FrameKind => VmdFrameKind.Shadow;

    /// <summary>
    /// セルフ影モード
    /// </summary>
    public SelfShadow Mode { get; set; }

    /// <summary>
    /// 影範囲
    /// </summary>
    public float Range { get; set; }

    /// <summary>
    /// コンストラクタ
    /// </summary>
    /// <param name="frame">フレーム</param>
    public VmdShadowFrame(uint frame = 0) : base("SelfShadow")
    {
        Frame = frame;
        Mode = SelfShadow.Mode1;
        Range = 8875 * 0.00001f;
    }

    public override object Clone() => new VmdShadowFrame(Frame)
    {
        Mode = Mode,
        Name = Name,
        Range = Range
    };
}
