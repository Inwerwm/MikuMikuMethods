using MikuMikuMethods.Extension;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MikuMikuMethods.PMM
{
    /// <summary>
    /// PMMのアクセサリ情報
    /// </summary>
    public class PmmAccessory
    {
        /// <summary>
        /// アクセサリ管理番号
        /// </summary>
        public byte Index { get; set; }
        /// <summary>
        /// 名前
        /// </summary>
        public string Name { get; set; }
        /// <summary>
        /// ファイルパス
        /// </summary>
        public string Path { get; set; }
        /// <summary>
        /// 描画順
        /// </summary>
        public byte RenderOrder { get; set; }
        /// <summary>
        /// 加算合成のOn/Off
        /// </summary>
        public bool EnableAlphaBlend { get; set; }

        /// <summary>
        /// 初期位置のアクセサリフレーム
        /// </summary>
        public PmmAccessoryFrame InitialFrame { get; set; }
        /// <summary>
        /// アクセサリのキーフレーム
        /// </summary>
        public List<PmmAccessoryFrame> Frames { get; init; }
        /// <summary>
        /// 未確定編集状態
        /// </summary>
        public PmmAccessoryFrame Uncomitted { get; set; }

        /// <summary>
        /// コンストラクタ
        /// </summary>
        public PmmAccessory()
        {
            Frames = new();
        }

        /// <summary>
        /// バイナリデータから読み込み
        /// </summary>
        /// <param name="reader">読み込むファイル</param>
        public PmmAccessory(BinaryReader reader) : this()
        {
            Read(reader);
        }

        /// <summary>
        /// バイナリデータから読み込み
        /// </summary>
        /// <param name="reader">読み込むファイル</param>
        public void Read(BinaryReader reader)
        {
            Index = reader.ReadByte();

            Name = reader.ReadString(100, Encoding.ShiftJIS, '\0');
            Path = reader.ReadString(256, Encoding.ShiftJIS, '\0');

            RenderOrder = reader.ReadByte();

            InitialFrame = new(reader, null);

            var accessoryCount = reader.ReadInt32();
            for (int i = 0; i < accessoryCount; i++)
                Frames.Add(new(reader, i));

            Uncomitted = new(reader, null);

            EnableAlphaBlend = reader.ReadBoolean();
        }

        /// <summary>
        /// ファイルに書込
        /// </summary>
        /// <param name="writer">出力対象バイナリファイル</param>
        public void Write(BinaryWriter writer)
        {
            writer.Write(Index);

            writer.Write(Name, 100, Encoding.ShiftJIS, '\0');
            writer.Write(Path, 256, Encoding.ShiftJIS, '\0');

            writer.Write(RenderOrder);

            InitialFrame.Write(writer);

            foreach (var frame in Frames)
                frame.Write(writer);

            Uncomitted.Write(writer);

            writer.Write(EnableAlphaBlend);
        }
    }
}
