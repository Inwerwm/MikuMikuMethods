using System;
using System.IO;
using System.Numerics;
using MikuMikuMethods.Extension;

namespace MikuMikuMethods.PMM.Frame
{
    /// <summary>
    /// 照明フレーム情報
    /// </summary>
    public class PmmLightFrame : PmmFrame
    {
        /// <summary>
        /// 色(RGBのみ使用)
        /// </summary>
        public ColorF Color { get; set; }
        /// <summary>
        /// 位置
        /// </summary>
        public Vector3 Position { get; set; }

        /// <summary>
        /// コンストラクタ
        /// </summary>
        public PmmLightFrame()
        {
            Color = ColorF.FromARGB(154, 154, 154);
        }

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

            writer.Write(Color, false);
            writer.Write(Position);

            writer.Write(IsSelected);
        }
    }
}
