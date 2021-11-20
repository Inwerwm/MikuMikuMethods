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
    public override VmdFrameKind FrameKind => Vmd.VmdFrameKind.Camera;

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
    public Dictionary<InterpolationItem, InterpolationCurve> InterpolationCurves { get; internal init; }

    /// <summary>
    /// コンストラクタ
    /// </summary>
    /// <param name="frame">フレーム時間</param>
    public VmdCameraFrame(uint frame = 0) : base("Camera")
    {
        Frame = frame;
        ViewAngle = 30;

        InterpolationCurves = new();
        InterpolationCurves.Add(InterpolationItem.XPosition, new());
        InterpolationCurves.Add(InterpolationItem.YPosition, new());
        InterpolationCurves.Add(InterpolationItem.ZPosition, new());
        InterpolationCurves.Add(InterpolationItem.Rotation, new());
        InterpolationCurves.Add(InterpolationItem.Distance, new());
        InterpolationCurves.Add(InterpolationItem.ViewAngle, new());
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
