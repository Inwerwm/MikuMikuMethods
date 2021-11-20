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
    public Vector3 Color { get; set; }

    /// <summary>
    /// 照明位置
    /// </summary>
    public Vector3 Position { get; set; }

    /// <summary>
    /// コンストラクタ
    /// </summary>
    /// <param name="frame">フレーム</param>
    public VmdLightFrame(uint frame = 0):base("Light")
    {
        Frame = frame;
        Color = new Vector3(154f / 255, 154f / 255, 154f / 255);
        Position = new Vector3(-0.5f, -1.0f, 0.5f);
    }

    public override object Clone() => new VmdLightFrame(Frame)
    {
        Color = Color,
        Name = Name,
        Position = Position
    };
}
