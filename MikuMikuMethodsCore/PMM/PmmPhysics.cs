using MikuMikuMethods.PMM.Frame;
using System.Collections.Generic;
using System.Numerics;

namespace MikuMikuMethods.PMM
{
    public class PmmPhysics
    {
        /// <summary>
        /// ノイズ量
        /// nullならなし
        /// </summary>
        public int? GravityNoize { get; set; } = null;
        /// <summary>
        /// 現在の加速度
        /// </summary>
        public float GravityAcceleration { get; set; } = 9.8f;
        /// <summary>
        /// 現在の方向
        /// </summary>
        public Vector3 GravityDirection { get; set; } = new(0, -1, 0);

        /// <summary>
        /// 重力フレーム
        /// </summary>
        public List<PmmGravityFrame> Frames { get; } = new();

        /// <summary>
        /// 物理演算モード
        /// </summary>
        public enum PhysicsMode : byte
        {
            /// <summary>
            /// 演算しない
            /// </summary>
            Disable,
            /// <summary>
            /// 常に演算
            /// </summary>
            Always,
            /// <summary>
            /// オン/オフモード
            /// </summary>
            Switchable,
            /// <summary>
            /// トレースモード
            /// </summary>
            Trace
        }
        /// <summary>
        /// 物理演算モード
        /// </summary>
        public PhysicsMode CurrentPhysicsMode { get; set; } = PhysicsMode.Always;
        /// <summary>
        /// 物理床のOn/Off
        /// </summary>
        public bool EnableGroundPhysics { get; set; } = true;
    }
}