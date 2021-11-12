using System.IO;

namespace MikuMikuMethods.Vmd
{
    /// <summary>
    /// 影フレーム
    /// </summary>
    public class VmdShadowFrame : VmdCameraTypeFrame
    {
        /// <summary>
        /// フレームの種類
        /// </summary>
        public override VmdFrameType FrameType => VmdFrameType.Shadow;

        /// <summary>
        /// セルフ影モード
        /// </summary>
        public byte Mode { get; set; }

        /// <summary>
        /// 影範囲
        /// </summary>
        public float Range { get; set; }

        /// <summary>
        /// コンストラクタ
        /// </summary>
        /// <param name="frame">フレーム</param>
        public VmdShadowFrame(uint frame = 0)
        {
            Name = "セルフ影";
            Frame = frame;
            Mode = 1;
            Range = 8875 * 0.00001f;
        }

        /// <summary>
        /// バイナリから読み込むコンストラクタ
        /// </summary>
        public VmdShadowFrame(BinaryReader reader)
        {
            Read(reader);
        }

        /// <summary>
        /// VMD形式から読み込み
        /// </summary>
        public override void Read(BinaryReader reader)
        {
            Frame = reader.ReadUInt32();
            Mode = reader.ReadByte();
            Range = reader.ReadSingle();
        }

        /// <summary>
        /// VMD形式に書き込み
        /// </summary>
        public override void Write(BinaryWriter writer)
        {
            writer.Write(Frame);
            writer.Write(Mode);
            writer.Write(Range);
        }

        public override object Clone() => new VmdShadowFrame(Frame)
        {
            Mode = Mode,
            Name = Name,
            Range = Range
        };
    }
}
