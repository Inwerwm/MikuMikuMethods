using System.Collections.ObjectModel;
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
    public ReadOnlyDictionary<InterpolationItem, InterpolationCurve> InterpolationCurves { get; internal init; } = InterpolationCurve.CreateBoneCurves();

    /// <summary>
    /// コンストラクタ
    /// </summary>
    /// <param name="name">ボーン名</param>
    /// <param name="frame">フレーム時間</param>
    public VmdMotionFrame(string name, uint frame = 0) : base(name)
    {
        Frame = frame;
    }

    public override object Clone() => new VmdMotionFrame(Name, Frame)
    {
        Position = Position,
        Rotation = Rotation,
        InterpolationCurves = InterpolationCurve.Clone(InterpolationCurves)
    };
}
