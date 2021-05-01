using MikuMikuMethods.PMM.Frame;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MikuMikuMethods.PMM
{
    /// <summary>
    /// セルフ影情報
    /// </summary>
    public class PmmSelfShadow
    {
        /// <summary>
        /// セルフ影のOn/Off
        /// </summary>
        public bool EnableSelfShadow { get; set; }
        /// <summary>
        /// 影範囲
        /// </summary>
        public float ShadowLimit { get; set; }

        /// <summary>
        /// 初期位置のセルフ影フレーム
        /// </summary>
        public PmmSelfShadowFrame InitialFrame { get; set; }
        /// <summary>
        /// セルフ影のキーフレーム
        /// </summary>
        public List<PmmSelfShadowFrame> Frames { get; init; } = new();

        /// <summary>
        /// バイナリデータから読み込み
        /// </summary>
        /// <param name="reader">読み込むファイル</param>
        public void Read(BinaryReader reader)
        {
            EnableSelfShadow = reader.ReadBoolean();
            ShadowLimit = reader.ReadSingle();

            InitialFrame = new(reader, null);

            var selfShadowCount = reader.ReadInt32();
            for (int i = 0; i < selfShadowCount; i++)
                Frames.Add(new(reader, reader.ReadInt32()));
        }

        /// <summary>
        /// ファイルに書込
        /// </summary>
        /// <param name="writer">出力対象バイナリファイル</param>
        public void Write(BinaryWriter writer)
        {
            writer.Write(EnableSelfShadow);
            writer.Write(ShadowLimit);

            InitialFrame.Write(writer);

            writer.Write(Frames.Count);
            foreach (var frame in Frames)
                frame.Write(writer);
        }
    }
}
