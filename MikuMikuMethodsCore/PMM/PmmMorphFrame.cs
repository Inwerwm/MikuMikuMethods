using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MikuMikuMethods.PMM
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
        public override void Read(BinaryReader reader, int? index)
        {
            Index = index;

            Frame = reader.ReadInt32();
            PreviousFrameIndex = reader.ReadInt32();
            NextFrameIndex = reader.ReadInt32();

            Weight = reader.ReadSingle();

            IsSelected = reader.ReadByte() == 1;
        }
    }
}
