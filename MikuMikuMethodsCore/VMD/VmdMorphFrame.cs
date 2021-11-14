using MikuMikuMethods.Extension;

namespace MikuMikuMethods.Vmd;

/// <summary>
/// モーフフレーム
/// </summary>
public class VmdMorphFrame : VmdModelTypeFrame
{
    /// <summary>
    /// フレームの種類
    /// </summary>
    public override VmdFrameType FrameType => VmdFrameType.Morph;

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

    /// <summary>
    /// バイナリから読み込むコンストラクタ
    /// </summary>
    public VmdMorphFrame(BinaryReader reader) : base("Morph")
    {
        Read(reader);
    }

    /// <summary>
    /// VMD形式から読み込み
    /// </summary>
    public override void Read(BinaryReader reader)
    {
        Name = reader.ReadString(VmdConstants.MorphNameLength, Encoding.ShiftJIS, '\0');
        Frame = reader.ReadUInt32();
        Weight = reader.ReadSingle();
    }

    /// <summary>
    /// VMD形式に書き込み
    /// </summary>
    public override void Write(BinaryWriter writer)
    {
        writer.Write(Name, VmdConstants.MorphNameLength, Encoding.ShiftJIS);
        writer.Write(Frame);
        writer.Write(Weight);
    }

    public override object Clone() => new VmdMorphFrame(Name, Frame)
    {
        Weight = Weight
    };
}
