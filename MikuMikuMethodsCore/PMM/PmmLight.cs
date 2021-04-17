using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MikuMikuMethods.PMM
{
    /// <summary>
    /// PMMの照明情報
    /// </summary>
    public class PmmLight
    {
        /// <summary>
        /// 初期位置の照明フレーム
        /// </summary>
        public PmmLightFrame InitialFrame { get; set; }
        /// <summary>
        /// 照明のキーフレーム
        /// </summary>
        public List<PmmLightFrame> Frames { get; init; }
        /// <summary>
        /// 未確定編集状態
        /// </summary>
        public PmmLightFrame Uncomitted { get; set; }

        /// <summary>
        /// コンストラクタ
        /// </summary>
        public PmmLight()
        {
            Frames = new();
        }

        /// <summary>
        /// バイナリデータから照明情報を読み込み
        /// </summary>
        /// <param name="reader">読み込むファイル</param>
        public PmmLight(BinaryReader reader) : this()
        {
            Read(reader);
        }

        /// <summary>
        /// バイナリデータから読み込み
        /// </summary>
        /// <param name="reader">読み込むファイル</param>
        public void Read(BinaryReader reader)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// ファイルに書込
        /// </summary>
        /// <param name="writer">出力対象バイナリファイル</param>
        public void Write(BinaryWriter writer)
        {
            throw new NotImplementedException();
        }
    }
}
