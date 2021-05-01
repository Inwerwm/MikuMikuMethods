using System.IO;

namespace MikuMikuMethods.PMM.Frame
{
    /// <summary>
    /// セルフ影フレーム情報
    /// </summary>
    public class PmmSelfShadowFrame : PmmFrame
    {
        /// <summary>
        /// 影モード
        /// </summary>
        public Shadow ShadowMode { get; set; }
        /// <summary>
        /// 影範囲
        /// </summary>
        public float ShadowRange { get; set; }

        /// <summary>
        /// バイナリデータから読み込み
        /// </summary>
        /// <param name="reader">読み込むファイル</param>
        /// <param name="index">フレームID</param>
        public PmmSelfShadowFrame(BinaryReader reader, int? index)
        {
            Read(reader, index);
        }

        /// <summary>
        /// バイナリデータから読み込み
        /// </summary>
        /// <param name="reader">読み込むファイル</param>
        /// <param name="index">フレームID</param>
        public void Read(BinaryReader reader, int? index)
        {
            Index = index;

            Frame = reader.ReadInt32();
            PreviousFrameIndex = reader.ReadInt32();
            NextFrameIndex = reader.ReadInt32();

            ShadowMode = (Shadow)reader.ReadByte();
            ShadowRange = reader.ReadSingle();

            IsSelected = reader.ReadBoolean();
        }
        /// <summary>
        /// ファイルに書込
        /// </summary>
        /// <param name="writer">出力対象バイナリファイル</param>
        public void Write(BinaryWriter writer)
        {
            if (Index.HasValue)
                writer.Write(Index.Value);

            writer.Write(Frame);
            writer.Write(PreviousFrameIndex);
            writer.Write(NextFrameIndex);

            writer.Write((byte)ShadowMode);
            writer.Write(ShadowRange);

            writer.Write(IsSelected);
        }

        /// <summary>
        /// 影のモード
        /// </summary>
        public enum Shadow : byte
        {
            /// <summary>
            /// 影なし
            /// </summary>
            None,
            /// <summary>
            /// モード1
            /// </summary>
            Mode1,
            /// <summary>
            /// モード2
            /// </summary>
            Mode2,
        }
    }
}
