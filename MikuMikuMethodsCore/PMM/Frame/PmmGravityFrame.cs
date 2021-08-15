using MikuMikuMethods.Extension;
using System.IO;
using System.Numerics;

namespace MikuMikuMethods.PMM.Frame
{
    /// <summary>
    /// 重力フレーム情報
    /// </summary>
    public class PmmGravityFrame : PmmFrame
    {

        /// <summary>
        /// ノイズ不可On/Off
        /// </summary>
        public bool EnableNoize { get; set; }
        /// <summary>
        /// ノイズ量
        /// </summary>
        public int NoizeAmount { get; set; }
        /// <summary>
        /// 加速度
        /// </summary>
        public float Acceleration { get; set; }
        /// <summary>
        /// 方向
        /// </summary>
        public Vector3 Direction { get; set; }

        /// <summary>
        /// デフォルトコンストラクタ
        /// </summary>
        public PmmGravityFrame()
        {
            EnableNoize = false;
            NoizeAmount = 10;
            Acceleration = 9.8f;
            Direction = new(0, -1, 0);
        }

        /// <summary>
        /// バイナリデータから読み込み
        /// </summary>
        /// <param name="reader">読み込むファイル</param>
        /// <param name="index">フレームID</param>
        internal PmmGravityFrame(BinaryReader reader, int? index)
        {
            Read(reader, index);
        }

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

            EnableNoize = reader.ReadBoolean();
            NoizeAmount = reader.ReadInt32();
            Acceleration = reader.ReadSingle();
            Direction = reader.ReadVector3();

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

            writer.Write(EnableNoize);
            writer.Write(NoizeAmount);
            writer.Write(Acceleration);
            writer.Write(Direction);

            writer.Write(IsSelected);
        }
    }
}
