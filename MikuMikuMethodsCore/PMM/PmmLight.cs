using MikuMikuMethods.Extension;
using MikuMikuMethods.PMM.Frame;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Numerics;
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
        public TemporaryLightEditState Uncomitted { get; set; }

        /// <summary>
        /// コンストラクタ
        /// </summary>
        public PmmLight()
        {
            Frames = new();
            Uncomitted = new();
        }

        /// <summary>
        /// バイナリデータから照明情報を読み込み
        /// </summary>
        /// <param name="reader">読み込むファイル</param>
        internal PmmLight(BinaryReader reader) : this()
        {
            Read(reader);
        }

        /// <summary>
        /// バイナリデータから読み込み
        /// </summary>
        /// <param name="reader">読み込むファイル</param>
        internal void Read(BinaryReader reader)
        {
            InitialFrame = new(reader, null);

            var lightCount = reader.ReadInt32();
            for (int i = 0; i < lightCount; i++)
                Frames.Add(new(reader, reader.ReadInt32()));

            Uncomitted.Color = reader.ReadSingleRGB();
            Uncomitted.Position = reader.ReadVector3();
        }

        /// <summary>
        /// ファイルに書込
        /// </summary>
        /// <param name="writer">出力対象バイナリファイル</param>
        internal void Write(BinaryWriter writer)
        {
            InitialFrame.Write(writer);

            writer.Write(Frames.Count);
            foreach (var frame in Frames)
                frame.Write(writer);

            writer.Write(Uncomitted.Color, false);
            writer.Write(Uncomitted.Position);
        }
    }

    /// <summary>
    /// 未確定のライト編集状態
    /// </summary>
    public class TemporaryLightEditState
    {
        /// <summary>
        /// 色(RGBのみ使用)
        /// </summary>
        public ColorF Color { get; set; }
        /// <summary>
        /// 位置
        /// </summary>
        public Vector3 Position { get; set; }
    }
}
