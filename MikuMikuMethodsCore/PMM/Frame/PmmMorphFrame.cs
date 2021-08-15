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
        public float Ratio { get; set; }

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

            writer.Write(Ratio);

            writer.Write(IsSelected);
        }
    }
}
