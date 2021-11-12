using MikuMikuMethods.Pmm.ElementState;
using MikuMikuMethods.Pmm.Frame;
using System.Collections.Generic;
using System.Numerics;

namespace MikuMikuMethods.Pmm
{
    public class PmmPhysics
    {
        public PmmGravityState CurrentGravity { get; } = new()
        {
            Noize = null,
            Acceleration = 9.8f,
            Direction = new(0, -1, 0)
        };

        /// <summary>
        /// 重力フレーム
        /// </summary>
        public List<PmmGravityFrame> GravityFrames { get; } = new();

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
        public PhysicsMode CalculationMode { get; set; } = PhysicsMode.Always;
        /// <summary>
        /// 物理床のOn/Off
        /// </summary>
        public bool EnableGroundPhysics { get; set; } = true;
    }
}