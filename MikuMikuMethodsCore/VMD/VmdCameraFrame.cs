using MikuMikuMethods.Extension;
using System.Collections.Generic;
using System.IO;
using System.Numerics;

namespace MikuMikuMethods.Vmd;

/// <summary>
/// カメラフレーム
/// </summary>
public class VmdCameraFrame : VmdCameraTypeFrame, IVmdInterpolatable
{
    /// <summary>
    /// フレームの種類
    /// </summary>
    public override VmdFrameType FrameType => Vmd.VmdFrameType.Camera;

    /// <summary>
    /// 距離
    /// </summary>
    public float Distance { get; set; }

    /// <summary>
    /// 移動量
    /// </summary>
    public Vector3 Position { get; set; }

    /// <summary>
    /// 回転量
    /// </summary>
    public Vector3 Rotation { get; set; }

    /// <summary>
    /// 視野角
    /// </summary>
    public uint ViewAngle { get; set; }

    /// <summary>
    /// パースの切/入 trueで切
    /// </summary>
    public bool IsPerspectiveOff { get; set; }

    /// <summary>
    /// 補間曲線
    /// </summary>
    public Dictionary<InterpolationItem, InterpolationCurve> InterpolationCurves { get; private set; }

    /// <summary>
    /// コンストラクタ
    /// </summary>
    /// <param name="frame">フレーム時間</param>
    public VmdCameraFrame(uint frame = 0) : base("Camera")
    {
        Frame = frame;
        ViewAngle = 30;
        InterpolationCurves = new();
        InitializeInterpolationCurves();
    }

    /// <summary>
    /// バイナリから読み込むコンストラクタ
    /// </summary>
    public VmdCameraFrame(BinaryReader reader) : base("Camera")
    {
        InterpolationCurves = new();
        Read(reader);
    }

    private void InitializeInterpolationCurves()
    {
        InterpolationCurves.Add(InterpolationItem.XPosition, new());
        InterpolationCurves.Add(InterpolationItem.YPosition, new());
        InterpolationCurves.Add(InterpolationItem.ZPosition, new());
        InterpolationCurves.Add(InterpolationItem.Rotation, new());
        InterpolationCurves.Add(InterpolationItem.Distance, new());
        InterpolationCurves.Add(InterpolationItem.ViewAngle, new());
    }

    /// <summary>
    /// VMD形式から読み込み
    /// </summary>
    public override void Read(BinaryReader reader)
    {
        Frame = reader.ReadUInt32();
        Distance = reader.ReadSingle();
        Position = reader.ReadVector3();
        Rotation = reader.ReadVector3();

        //補間曲線を読み込み
        var interpolationMatrix = reader.ReadBytes(24);
        InterpolationCurves = InterpolationCurve.CreateByVMDFormat(interpolationMatrix, FrameType);

        ViewAngle = reader.ReadUInt32();
        IsPerspectiveOff = reader.ReadBoolean();
    }

    /// <summary>
    /// VMD形式に書き込み
    /// </summary>
    public override void Write(BinaryWriter writer)
    {
        writer.Write(Frame);
        writer.Write(Distance);
        writer.Write(Position);
        writer.Write(Rotation);
        writer.Write(InterpolationCurve.CreateVMDFormatBytes(InterpolationCurves, FrameType));
        writer.Write(ViewAngle);
        writer.Write(IsPerspectiveOff);
    }

    public override object Clone() => new VmdCameraFrame(Frame)
    {
        Distance = Distance,
        InterpolationCurves = InterpolationCurve.Clone(InterpolationCurves),
        IsPerspectiveOff = IsPerspectiveOff,
        Name = Name,
        Position = Position,
        Rotation = Rotation,
        ViewAngle = ViewAngle
    };
}
