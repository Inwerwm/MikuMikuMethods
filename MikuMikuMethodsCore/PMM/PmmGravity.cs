using MikuMikuMethods.Extension;
using MikuMikuMethods.PMM.Frame;
using System.Collections.Generic;
using System.IO;
using System.Numerics;

namespace MikuMikuMethods.PMM
{
    /// <summary>
    /// PMMの重力情報
    /// </summary>
    public class PmmGravity
    {
        /// <summary>
        /// 現在のノイズ不可On/Off
        /// </summary>
        public bool EnableNoize { get; set; }
        /// <summary>
        /// 現在のノイズ量
        /// </summary>
        public int NoizeAmount { get; set; }
        /// <summary>
        /// 現在の加速度
        /// </summary>
        public float Acceleration { get; set; }
        /// <summary>
        /// 現在の方向
        /// </summary>
        public Vector3 Direction { get; set; }

        /// <summary>
        /// 初期位置の重力フレーム
        /// </summary>
        public PmmGravityFrame InitialFrame { get; set; }
        /// <summary>
        /// 重力のキーフレーム
        /// </summary>
        public List<PmmGravityFrame> Frames { get; init; }

        /// <summary>
        /// デフォルトコンストラクタ
        /// </summary>
        public PmmGravity()
        {
            Frames = new();
        }

        /// <summary>
        /// バイナリデータから読み込み
        /// </summary>
        /// <param name="reader">読み込むファイル</param>
        public PmmGravity(BinaryReader reader)
        {
            Read(reader);
        }

        /// <summary>
        /// バイナリデータから読み込み
        /// </summary>
        /// <param name="reader">読み込むファイル</param>
        public void Read(BinaryReader reader)
        {
            Acceleration = reader.ReadSingle();
            NoizeAmount = reader.ReadInt32();
            Direction = reader.ReadVector3();
            EnableNoize = reader.ReadBoolean();

            InitialFrame = new(reader, null);

            var frameCount = reader.ReadInt32();
            for (int i = 0; i < frameCount; i++)
                Frames.Add(new(reader, reader.ReadInt32()));
        }
        /// <summary>
        /// ファイルに書込
        /// </summary>
        /// <param name="writer">出力対象バイナリファイル</param>
        public void Write(BinaryWriter writer)
        {
            writer.Write(Acceleration);
            writer.Write(NoizeAmount);
            writer.Write(Direction);
            writer.Write(EnableNoize);

            InitialFrame.Write(writer);

            writer.Write(Frames.Count);
            foreach (var frame in Frames)
                frame.Write(writer);
        }
    }
}
