using System.Numerics;

namespace MikuMikuMethods.Vmd;

/// <summary>
/// モーションフレーム
/// </summary>
public class VmdMotionFrame : VmdModelTypeFrame, IVmdInterpolatable
{
    /// <summary>
    /// フレームの種類
    /// </summary>
    public override VmdFrameKind FrameKind => VmdFrameKind.Motion;

    /// <summary>
    /// 移動量
    /// </summary>
    public Vector3 Position { get; set; }
    /// <summary>
    /// 回転量
    /// </summary>
    public Quaternion Rotation { get; set; }

    /// <summary>
    /// 補間曲線
    /// </summary>
    public Dictionary<InterpolationItem, InterpolationCurve> InterpolationCurves { get; internal init; }

    /// <summary>
    /// コンストラクタ
    /// </summary>
    /// <param name="name">ボーン名</param>
    /// <param name="frame">フレーム時間</param>
    public VmdMotionFrame(string name, uint frame = 0) : base(name)
    {
        Frame = frame;

        InterpolationCurves = new();
        InterpolationCurves.Add(InterpolationItem.XPosition, new());
        InterpolationCurves.Add(InterpolationItem.YPosition, new());
        InterpolationCurves.Add(InterpolationItem.ZPosition, new());
        InterpolationCurves.Add(InterpolationItem.Rotation, new());
    }

    public override object Clone() => new VmdMotionFrame(Name, Frame)
    {
        Position = Position,
        Rotation = Rotation,
        InterpolationCurves = InterpolationCurve.Clone(InterpolationCurves)
    };
}
