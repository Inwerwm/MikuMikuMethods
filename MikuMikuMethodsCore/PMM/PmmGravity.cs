using MikuMikuMethods.PMM.Frame;
using System.Collections.Generic;
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
    }
}
