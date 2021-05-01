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
        public TemporaryAccessoryEditState Uncomitted { get; set; }

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
                Frames.Add(new(reader, reader.ReadInt32()));

            Uncomitted = new(reader);

            EnableAlphaBlend = reader.ReadBoolean();
        }

        /// <summary>
        /// ファイルに書込
        /// </summary>
        /// <param name="writer">出力対象バイナリファイル</param>
        public void Write(BinaryWriter writer)
        {
            writer.Write(Index);

            writer.Write(Name, 100, Encoding.ShiftJIS, 'x');
            writer.Write(Path, 256, Encoding.ShiftJIS, 'x');

            writer.Write(RenderOrder);

            InitialFrame.Write(writer);

            writer.Write(Frames.Count);
            foreach (var frame in Frames)
                frame.Write(writer);

            Uncomitted.Write(writer);

            writer.Write(EnableAlphaBlend);
        }
    }

    /// <summary>
    /// 未確定のアクセサリ編集状態
    /// </summary>
    public class TemporaryAccessoryEditState
    {
        /// <summary>
        /// <para>上位7bit : 半透明度</para>
        /// <para>最下位1bit : 表示/非表示</para>
        /// </summary>
        public byte OpacityAndVisible { get; set; }
        /// <summary>
        /// <para>親モデルのインデックス</para>
        /// <para>-1 なら親なし</para>
        /// </summary>
        public int ParentModelIndex { get; set; }
        /// <summary>
        /// 親ボーンのインデックス
        /// </summary>
        public int ParentBoneIndex { get; set; }
        /// <summary>
        /// 位置
        /// </summary>
        public Vector3 Position { get; set; }
        /// <summary>
        /// 回転
        /// </summary>
        public Vector3 Rotation { get; set; }
        /// <summary>
        /// 拡縮
        /// </summary>
        public float Scale { get; set; }
        /// <summary>
        /// 影のOn/Off
        /// </summary>
        public bool EnableShadow { get; set; }

        /// <summary>
        /// 何もしないコンストラクタ
        /// </summary>
        public TemporaryAccessoryEditState(){}

        /// <summary>
        /// バイナリデータから読み込み
        /// </summary>
        /// <param name="reader">読み込むファイル</param>
        public TemporaryAccessoryEditState(BinaryReader reader) : this()
        {
            Read(reader);
        }

        /// <summary>
        /// バイナリデータから読み込み
        /// </summary>
        /// <param name="reader">読み込むファイル</param>
        public void Read(BinaryReader reader)
        {
            OpacityAndVisible = reader.ReadByte();

            ParentModelIndex = reader.ReadInt32();
            ParentBoneIndex = reader.ReadInt32();

            Position = reader.ReadVector3();
            Rotation = reader.ReadVector3();
            Scale = reader.ReadSingle();

            EnableShadow = reader.ReadBoolean();
        }

        /// <summary>
        /// ファイルに書込
        /// </summary>
        /// <param name="writer">出力対象バイナリファイル</param>
        public void Write(BinaryWriter writer)
        {
            writer.Write(OpacityAndVisible);

            writer.Write(ParentModelIndex);
            writer.Write(ParentBoneIndex);

            writer.Write(Position);
            writer.Write(Rotation);
            writer.Write(Scale);

            writer.Write(EnableShadow);
        }
    }
}
