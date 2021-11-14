using MikuMikuMethods.Extension;
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
    public override VmdFrameType FrameType => VmdFrameType.Light;

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

    /// <summary>
    /// バイナリから読み込むコンストラクタ
    /// </summary>
    public VmdLightFrame(BinaryReader reader):base("Light")
    {
        Read(reader);
    }

    /// <summary>
    /// VMD形式から読み込み
    /// </summary>
    public override void Read(BinaryReader reader)
    {
        Frame = reader.ReadUInt32();
        Color = reader.ReadVector3();
        Position = reader.ReadVector3();
    }

    /// <summary>
    /// VMD形式に書き込み
    /// </summary>
    public override void Write(BinaryWriter writer)
    {
        writer.Write(Frame);
        writer.Write(Color);
        writer.Write(Position);
    }

    public override object Clone() => new VmdLightFrame(Frame)
    {
        Color = Color,
        Name = Name,
        Position = Position
    };
}
