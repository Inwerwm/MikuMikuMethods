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
    /// コンストラクタ
    /// </summary>
    /// <param name="name">ボーン名</param>
    /// <param name="frame">フレーム時間</param>
    public VmdMorphFrame(string name, uint frame = 0) : base(name)
    {
        Frame = frame;
    }

    public override object Clone() => new VmdMorphFrame(Name, Frame)
    {
        Weight = Weight
    };
}
