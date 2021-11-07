using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Numerics;

namespace MikuMikuMethods.PMM.Binary.Frame
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
        public ReadOnlyDictionary<InterpolationItem, InterpolationCurve> InterpolationCurces { get; init; }

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
    }
}
