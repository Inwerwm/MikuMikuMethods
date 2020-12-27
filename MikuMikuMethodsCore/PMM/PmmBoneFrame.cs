using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;

namespace MikuMikuMethods.PMM
{
    /// <summary>
    /// ボーンフレーム情報
    /// </summary>
    public class PmmBoneFrame
    {
        /// <summary>
        /// <para>フレーム番号</para>
        /// <para>初期フレームには振られないのでnullを入れる</para>
        /// </summary>
        public int? Index { get; set; }

        /// <summary>
        /// 所在フレーム
        /// </summary>
        public int Frame { get; set; }

        /// <summary>
        /// <para>直前のキーフレームのID</para>
        /// <para>存在しなければ0</para>
        /// </summary>
        public int PreviousFrameIndex { get; set; }
        /// <summary>
        /// 直後のキーフレームのID
        /// </summary>
        public int NextFrameIndex { get; set; }

        /// <summary>
        /// 補間曲線
        /// </summary>
        public ReadOnlyDictionary<InterpolationItem,InterpolationCurve> InterpolationCurces { get; init; }

        /// <summary>
        /// 移動量
        /// </summary>
        public Vector3 Offset { get; set; }

        /// <summary>
        /// 回転量
        /// </summary>
        public Quaternion Rotate { get; set; }

        /// <summary>
        /// 選択されているか
        /// </summary>
        public bool IsSelected { get; set; }

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
