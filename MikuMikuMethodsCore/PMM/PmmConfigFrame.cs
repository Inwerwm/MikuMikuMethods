using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MikuMikuMethods.PMM
{
    /// <summary>
    /// 表示・IK・外観フレーム情報
    /// </summary>
    public class PmmConfigFrame : PmmFrame
    {
        /// <summary>
        /// 表示
        /// </summary>
        public bool Visible { get; set; }

        /// <summary>
        /// IKが有効か
        /// </summary>
        public List<bool> EnableIK { get; init; }

        /// <summary>
        /// 外部親設定
        /// </summary>
        public List<(int ModelId, int BoneId)> ParentSettings { get; init; }

        /// <summary>
        /// コンストラクタ
        /// </summary>
        public PmmConfigFrame()
        {
            EnableIK = new();
            ParentSettings = new();
        }

        /// <summary>
        /// バイナリデータから読み込み
        /// </summary>
        /// <param name="reader">読み込むファイル</param>
        /// <param name="index">フレームID</param>
        /// <param name="ikCount">IKボーンの数</param>
        /// <param name="parentCount">外部親設定可能ボーンの数</param>
        public void Read(BinaryReader reader, int? index, int ikCount, int parentCount)
        {
            Index = index;

            Frame = reader.ReadInt32();
            PreviousFrameIndex = reader.ReadInt32();
            NextFrameIndex = reader.ReadInt32();
            Visible = reader.ReadBoolean();

            for (int i = 0; i < ikCount; i++)
            {
                EnableIK.Add(reader.ReadBoolean());
            }

            for (int i = 0; i < parentCount; i++)
            {
                ParentSettings.Add((reader.ReadInt32(), reader.ReadInt32()));
            }

            IsSelected = reader.ReadBoolean();
        }

        /// <summary>
        /// ファイルに書込
        /// </summary>
        /// <param name="writer">出力対象バイナリファイル</param>
        public void Write(BinaryWriter writer)
        {
            writer.Write(Frame);
            writer.Write(PreviousFrameIndex);
            writer.Write(NextFrameIndex);
            writer.Write(Visible);

            foreach (var f in EnableIK)
            {
                writer.Write(f);
            }

            foreach (var f in ParentSettings)
            {
                writer.Write(f.ModelId);
                writer.Write(f.BoneId);
            }

            writer.Write(IsSelected);
        }
    }
}
