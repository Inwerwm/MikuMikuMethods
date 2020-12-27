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
        public Quaternion Rotate { get; set; }

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
        public override void Read(BinaryReader reader, int? index)
        {

        }
    }
}
