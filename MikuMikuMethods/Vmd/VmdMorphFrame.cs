namespace MikuMikuMethods.Vmd;

/// <summary>
/// モーフフレーム
/// </summary>
public class VmdMorphFrame : VmdModelTypeFrame
{
    /// <summary>
    /// フレームの種類
    /// </summary>
    public override VmdFrameKind FrameKind => VmdFrameKind.Morph;

    /// <summary>
    /// モーフ適用係数
    /// </summary>
    public float Weight { get; set; }

    /// <summary>
    /// モーフ名と初期フレーム時間を指定して <see cref="VmdMorphFrame"/> クラスの新しいインスタンスを初期化します。
    /// </summary>
    /// <param name="name">モーフ名</param>
    /// <param name="frame">フレーム時間</param>
    public VmdMorphFrame(string name, uint frame = 0) : base(name)
    {
        Frame = frame;
    }

    /// <inheritdoc/>
    public override object Clone() => new VmdMorphFrame(Name, Frame)
    {
        Weight = Weight
    };
}
