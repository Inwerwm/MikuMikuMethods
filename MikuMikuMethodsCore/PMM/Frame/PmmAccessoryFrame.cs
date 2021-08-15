using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;
using MikuMikuMethods.Extension;

namespace MikuMikuMethods.PMM.Frame
{
    /// <summary>
    /// アクセサリフレーム情報
    /// </summary>
    public class PmmAccessoryFrame : PmmFrame
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

            writer.Write(OpacityAndVisible);

            writer.Write(ParentModelIndex);
            writer.Write(ParentBoneIndex);

            writer.Write(Position);
            writer.Write(Rotation);
            writer.Write(Scale);

            writer.Write(EnableShadow);

            writer.Write(IsSelected);
        }
    }
}
