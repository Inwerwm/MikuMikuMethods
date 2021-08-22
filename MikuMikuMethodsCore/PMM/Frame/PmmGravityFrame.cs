using System.Numerics;

namespace MikuMikuMethods.PMM.Frame
{
    public class PmmGravityFrame : IPmmFrame
    {
        public int Position { get; set; } = 0;
        public bool IsSelected { get; set; } = false;

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

        public bool Equals(IPmmFrame other) =>
            other is PmmGravityFrame f && Position == f.Position;
    }
}
