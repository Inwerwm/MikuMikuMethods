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
        /// ファイルに書込
        /// </summary>
        /// <param name="writer">出力対象バイナリファイル</param>
        internal void Write(BinaryWriter writer)
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
