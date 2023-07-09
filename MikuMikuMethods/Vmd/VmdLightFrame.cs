using System.Numerics;

namespace MikuMikuMethods.Vmd;

/// <summary>
/// 照明フレーム
/// </summary>
public class VmdLightFrame : VmdCameraTypeFrame
{
    /// <summary>
    /// フレームの種類
    /// </summary>
    public override VmdFrameKind FrameKind => VmdFrameKind.Light;

    /// <summary>
    /// 照明色
    /// </summary>
    public ColorF Color { get; set; }

    /// <summary>
    /// 照明位置
    /// </summary>
    public Vector3 Position { get; set; }

    /// <summary>
    /// フレーム時間を指定して <see cref="VmdLightFrame"/> クラスの新しいインスタンスを初期化します。
    /// </summary>
    /// <param name="frame">フレーム時間</param>
    public VmdLightFrame(uint frame = 0) : base("Light")
    {
        Frame = frame;
        Color = new(154, 154, 154);
        Position = new Vector3(-0.5f, -1.0f, 0.5f);
    }

    /// <inheritdoc/>
    public override object Clone() => new VmdLightFrame(Frame)
    {
        Color = Color with { },
        Name = Name,
        Position = Position
    };
}
