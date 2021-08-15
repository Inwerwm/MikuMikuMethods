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
    /// PMMの照明情報
    /// </summary>
    public class PmmLight
    {
        /// <summary>
        /// 初期位置の照明フレーム
        /// </summary>
        public PmmLightFrame InitialFrame { get; set; }
        /// <summary>
        /// 照明のキーフレーム
        /// </summary>
        public List<PmmLightFrame> Frames { get; init; }
        /// <summary>
        /// 未確定編集状態
        /// </summary>
        public TemporaryLightEditState Uncomitted { get; set; }

        /// <summary>
        /// コンストラクタ
        /// </summary>
        public PmmLight()
        {
            Frames = new();
            Uncomitted = new();
        }
    }

    /// <summary>
    /// 未確定のライト編集状態
    /// </summary>
    public class TemporaryLightEditState
    {
        /// <summary>
        /// 色(RGBのみ使用)
        /// </summary>
        public ColorF Color { get; set; }
        /// <summary>
        /// 位置
        /// </summary>
        public Vector3 Position { get; set; }
    }
}
