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
    }
}
