using System.Numerics;

namespace MikuMikuMethods.PMM.ElementState
{
    public class PmmGravityState
    {

        /// <summary>
        /// ノイズ量
        /// nullならなし
        /// </summary>
        public int? Noize { get; set; } = null;
        /// <summary>
        /// 現在の加速度
        /// </summary>
        public float Acceleration { get; set; } = 9.8f;
        /// <summary>
        /// 現在の方向
        /// </summary>
        public Vector3 Direction { get; set; } = new(0, -1, 0);
    }
}