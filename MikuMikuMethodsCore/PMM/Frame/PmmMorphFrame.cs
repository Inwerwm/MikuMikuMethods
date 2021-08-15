using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MikuMikuMethods.PMM.Frame
{
    /// <summary>
    /// モーフフレーム情報
    /// </summary>
    public class PmmMorphFrame : PmmFrame
    {

        /// <summary>
        /// モーフ適用係数
        /// </summary>
        public float Weight { get; set; }

        /// <summary>
        /// バイナリデータから読み込み
        /// </summary>
        /// <param name="reader">読み込むファイル</param>
        /// <param name="index">フレームID</param>
        internal void Read(BinaryReader reader, int? index)
        {
            Index = index;

            Frame = reader.ReadInt32();
            PreviousFrameIndex = reader.ReadInt32();
            NextFrameIndex = reader.ReadInt32();

            Weight = reader.ReadSingle();

            IsSelected = reader.ReadBoolean();
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

            writer.Write(Weight);

            writer.Write(IsSelected);
        }
    }
}
