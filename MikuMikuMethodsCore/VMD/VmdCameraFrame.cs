using System.Collections.ObjectModel;
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
    public ReadOnlyDictionary<InterpolationItem, InterpolationCurve> InterpolationCurves { get; internal init; } = InterpolationCurve.CreateCameraCurves();

    /// <summary>
    /// コンストラクタ
    /// </summary>
    /// <param name="frame">フレーム時間</param>
    public VmdCameraFrame(uint frame = 0) : base("Camera")
    {
        Frame = frame;
        ViewAngle = 30;
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
