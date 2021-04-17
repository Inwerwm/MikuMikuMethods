using MikuMikuMethods.Extension;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;

namespace MikuMikuMethods.PMM
{
    /// <summary>
    /// ボーンフレーム情報
    /// </summary>
    public class PmmBoneFrame : PmmFrame
    {
        /// <summary>
        /// 移動量
        /// </summary>
        public Vector3 Offset { get; set; }

        /// <summary>
        /// 回転量
        /// </summary>
        public Quaternion Rotation { get; set; }

        /// <summary>
        /// 補間曲線
        /// </summary>
        public ReadOnlyDictionary<InterpolationItem,InterpolationCurve> InterpolationCurces { get; init; }

        /// <summary>
        /// 物理が有効か
        /// </summary>
        public bool EnablePhysic { get; set; }

        /// <summary>
        /// コンストラクタ
        /// </summary>
        public PmmBoneFrame()
        {
            Dictionary<InterpolationItem, InterpolationCurve> curveDic = new(4);
            curveDic.Add(InterpolationItem.XPosition, new());
            curveDic.Add(InterpolationItem.YPosition, new());
            curveDic.Add(InterpolationItem.ZPosition, new());
            curveDic.Add(InterpolationItem.Rotation, new());

            InterpolationCurces = new(curveDic);
        }

        /// <summary>
        /// バイナリデータから読み込み
        /// </summary>
        /// <param name="reader">読み込むファイル</param>
        /// <param name="index">フレームID</param>
        public void Read(BinaryReader reader, int? index)
        {
            Index = index;

            Frame = reader.ReadInt32();
            PreviousFrameIndex = reader.ReadInt32();
            NextFrameIndex = reader.ReadInt32();
            
            InterpolationCurces[InterpolationItem.XPosition].FromBytes(reader.ReadBytes(4));
            InterpolationCurces[InterpolationItem.YPosition].FromBytes(reader.ReadBytes(4));
            InterpolationCurces[InterpolationItem.ZPosition].FromBytes(reader.ReadBytes(4));
            InterpolationCurces[InterpolationItem.Rotation].FromBytes(reader.ReadBytes(4));

            Offset = reader.ReadVector3();
            Rotation = reader.ReadQuaternion();
            IsSelected = reader.ReadBoolean();
            EnablePhysic = !reader.ReadBoolean();
        }

        /// <summary>
        /// ファイルに書込
        /// </summary>
        /// <param name="writer">出力対象バイナリファイル</param>
        public void Write(BinaryWriter writer)
        {
            if (Index.HasValue)
                writer.Write(Index.Value);

            writer.Write(Frame);
            writer.Write(PreviousFrameIndex);
            writer.Write(NextFrameIndex);

            writer.Write(InterpolationCurces[InterpolationItem.XPosition].ToBytes());
            writer.Write(InterpolationCurces[InterpolationItem.YPosition].ToBytes());
            writer.Write(InterpolationCurces[InterpolationItem.ZPosition].ToBytes());
            writer.Write(InterpolationCurces[InterpolationItem.Rotation].ToBytes());

            writer.Write(Offset);
            writer.Write(Rotation);
            writer.Write(IsSelected);
            writer.Write(!EnablePhysic);
        }
    }
}
