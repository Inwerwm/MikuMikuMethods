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
    /// ボーン名と初期フレーム時間を指定して <see cref="VmdMotionFrame"/> クラスの新しいインスタンスを初期化します。
    /// </summary>
    /// <param name="name">ボーン名</param>
    /// <param name="frame">フレーム時間</param>
    public VmdMotionFrame(string name, uint frame = 0) : base(name)
    {
        Frame = frame;
    }

    /// <summary>
    /// ボーン名、初期フレーム時間、補間曲線のディクショナリを指定して <see cref="VmdMotionFrame"/> クラスの新しいインスタンスを初期化します。
    /// </summary>
    /// <param name="name">ボーン名</param>
    /// <param name="frame">フレーム時間</param>
    /// <param name="interpolationCurves">フレーム間の補間曲線のディクショナリ</param>
    public VmdMotionFrame(string name, uint frame, IDictionary<InterpolationItem, InterpolationCurve> interpolationCurves) : this(name, frame)
    {
        var mutableCurves = InterpolationCurve.CreateMutableBoneCurves();
        InterpolationCurve.CopyCurves(interpolationCurves, mutableCurves);

        InterpolationCurves = new(mutableCurves);
    }

    /// <inheritdoc/>
    public override object Clone() => new VmdMotionFrame(Name, Frame)
    {
        Position = Position,
        Rotation = Rotation,
        InterpolationCurves = InterpolationCurve.Clone(InterpolationCurves)
    };
}
